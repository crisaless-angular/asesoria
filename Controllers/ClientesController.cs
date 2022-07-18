using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Business.Interfaces;
using Web.Data;
using Web.Models;

namespace Web.Views.Clientes
{
    public class ClientesController : Controller
    {
        public IUnitOfWork _UnitOfWork;

        public ClientesController(IUnitOfWork UnitOfWork)
        {
            this._UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Listado de clientes";
            return View();
        }

        [HttpPost]
        public async Task<IQueryable<Cliente>> ObtenerClientesIndex()
        {
            return await _UnitOfWork.ClienteRepository.GetAllAsync();
        }

    }

    

}
