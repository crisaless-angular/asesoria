using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class RecuperarCuenta : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public RecuperarCuenta(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                
            }

            return "";
            
        }

    }
}
