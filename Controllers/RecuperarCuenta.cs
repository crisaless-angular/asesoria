using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class RecuperarCuenta : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "¿Ha olvidado su contraseña?";
            return View();
        }
    }
}
