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
        public List<ClientesViewModel> ObtenerClientesIndex()
        {
            return (from clientes in this._UnitOfWork.ClienteRepository.GetAll()
                            join EmailCliente in this._UnitOfWork.ClienteEmailRepository.GetAll() 
                            on clientes.CodigoCliente equals EmailCliente.IdCliente
                            where EmailCliente.Activo == true

                            select new ClientesViewModel()
                            {
                                CODIGO_CONTABILIDAD = clientes.CodigoContabilidad,
                                NOMBRE_FISCAL = clientes.NombreFiscal,
                                NOMBRE_COMERCIAL = clientes.NombreComercial,
                                TELEFONO = clientes.Telefono,
                                FAX = clientes.Fax,
                                MOVIL = clientes.Movil,
                                EMAILPRINCIPAL = EmailCliente.Email,
                                IDENTIFICACION_FISCAL = clientes.IdentificacionFiscal
                            }

                            ).ToList();

        }

        public IActionResult Crear()
        {
            ViewData["Title"] = "Crear un cliente";
            return View();
        }

    }

    

}
