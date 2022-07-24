using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Business.Interfaces;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    public class RecuperarCuenta : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMail _serviceMail;
        private IConfigurationRoot _ConfigRoot;
        private readonly IUnitOfWork _unitOfWork;

        public RecuperarCuenta(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IMail serviceMail, IConfiguration configRoot, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _serviceMail = serviceMail;
            _ConfigRoot = (IConfigurationRoot)configRoot;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "¿Ha olvidado su contraseña?";
            return View();
        }

        [HttpPost]
        public async Task<string> RecuperarContraseña(string Email)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(Email);
            
            if(user != null)
            {
                //JsonMail request = new JsonMail();
                //request.email = Email;
                //request.asunto = _ConfigRoot["Data_Mail:Subject"];
                //request.cuerpo = "Prueba body";
                //await _serviceMail.SendEmailAsync(request);

                try
                {
                    
                    AspNetUserToken UserToken = _unitOfWork.UserTokenRepository.GetAll().Where(x => x.UserId == user.Id).FirstOrDefault();
                    
                    if (UserToken != null)
                    {
                        _unitOfWork.UserTokenRepository.Delete(UserToken);
                        _unitOfWork.Save();
                    }

                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    token = token.Replace("+", "");

                    UserToken = new AspNetUserToken()
                    {
                        UserId = user.Id,
                        LoginProvider = "Asesoria",
                        Name = "TokenResetPassword",
                        Value = token
                    };

                    _unitOfWork.UserTokenRepository.Add(UserToken);
                    _unitOfWork.Save();

                    return token;

                }
                catch(Exception e)
                {
                    return "fail";
                }
                

                return "ok";
            }

            return "fail";
            
        }

        public IActionResult CambiarPassword(string token = "")
        {
            AspNetUserToken UserToken = _unitOfWork.UserTokenRepository.GetAll().Where(x => x.Value == token).FirstOrDefault();
            
            if(UserToken != null)
            {
                ChangePassword Model = new ChangePassword() { UserId = UserToken.UserId };

                ViewData["Title"] = "Cambiar contraseña";
                return View(Model);
            }

            return RedirectToAction("Index", "Home");


        }

        [HttpPost]
        public async Task<IActionResult> CambiarPasswordModel(ChangePassword model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("All", "Revise los campos, existen errores");
                return View();
            }

            IdentityUser user = await _userManager.FindByIdAsync(model.UserId);
            string TokenResetPassword = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, TokenResetPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    AspNetUserToken UserToken = new AspNetUserToken()
                    {
                        UserId = user.Id,
                        LoginProvider = "Asesoria",
                        Name = "TokenResetPassword",
                        Value = TokenResetPassword
                    };

                    _unitOfWork.UserTokenRepository.Delete(UserToken);
                    _unitOfWork.Save();
                }
                    
            }
               

            return RedirectToAction("Index", "Home");
        }

    }
}
