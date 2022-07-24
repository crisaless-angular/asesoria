using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Web.Business.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    public class RecuperarCuenta : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMail _serviceMail;
        private IConfigurationRoot _ConfigRoot;

        public RecuperarCuenta(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IMail serviceMail, IConfiguration configRoot)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _serviceMail = serviceMail;
            _ConfigRoot = (IConfigurationRoot)configRoot;
        }
        
        public IActionResult Index()
        {
            ViewData["Title"] = "¿Ha olvidado su contraseña?";
            return View();
        }

        [HttpPost]
        public async Task<string> RecuperarContraseña(string Email)
        {
            IdentityUser result = await _userManager.FindByEmailAsync(Email);
            
            if(result != null)
            {
                //JsonMail request = new JsonMail();
                //request.email = Email;
                //request.asunto = _ConfigRoot["Data_Mail:Subject"];
                //request.cuerpo = "Prueba body";
                //await _serviceMail.SendEmailAsync(request);
                var token = _userManager.GeneratePasswordResetTokenAsync(result);
                return "ok";
            }

            return "fail";
            
        }

        public IActionResult CambiarPassword()
        {
            ViewData["Title"] = "Cambiar contraseña";
            return View();
        }

        [HttpPost]
        public IActionResult CambiarPasswordModel()
        {
            ViewData["Title"] = "Cambiar contraseña";
            return RedirectToAction("Index", "Home");
        }

    }
}
