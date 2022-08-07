using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Business.Interfaces;
using Web.Data;
using Web.Models;

namespace Web.Views.Clientes
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly ILeerExcel _LeerExcel;
        private readonly IAuditoria _Auditoria;
        private readonly UserManager<IdentityUser> _userManager;

        public ClientesController(IUnitOfWork UnitOfWork, ILeerExcel LeerExcel, IAuditoria Auditoria, UserManager<IdentityUser> userManager)
        {
            this._UnitOfWork = UnitOfWork;
            this._LeerExcel = LeerExcel;
            this._Auditoria = Auditoria;
            this._userManager = userManager;
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
                            join Agentes in this._UnitOfWork.AgenteRepository.GetAll().DefaultIfEmpty()
                            on clientes.Agente equals Agentes.IdAgente
                            join Tipocliente in this._UnitOfWork.TipoClienteRepository.GetAll().DefaultIfEmpty()
                            on clientes.IdTipoCliente equals Tipocliente.IdTipoCliente
                            where EmailCliente.Activo == true

                            select new ClientesViewModel()
                            {
                                CODIGO_CONTABILIDAD = clientes.CodigoContabilidad,
                                NOMBRE_FISCAL = clientes.NombreFiscal,
                                NOMBRE_COMERCIAL = clientes.NombreComercial,
                                MOVIL = clientes.Movil,
                                EMAILPRINCIPAL = EmailCliente.Email,
                                IDENTIFICACION_FISCAL = clientes.IdentificacionFiscal,
                                AGENTE = Agentes.Agente1,
                                TIPO_CLIENTE = Tipocliente.TipoCliente1
                            }

                            ).ToList();

        }

        public IActionResult Crear()
        {
            ClientesViewModel model = new ClientesViewModel();
            model.PAISES = _UnitOfWork.PaisesRepository.GetAll();
            model.TIPO_IDENTIFICACION_FISCAL_ITEMS = _UnitOfWork.TipoIdentificacionFiscalRepository.GetAll();
            ViewData["Title"] = "Crear un cliente";
            return View(model);
        }

        [HttpPost]
        public IActionResult CrearCliente(ClientesViewModel model)
        {
            return Ok();
        }

        public IActionResult CargarClientes()
        {
            ViewData["Title"] = "Cargar clientes";
            return View();
        }


        [HttpPost]
        public IActionResult RecibirExcel([FromForm] IFormFile ArchivoExcel)
        {
            Stream stream = ArchivoExcel.OpenReadStream();

            IWorkbook MiExcel = null;

            if (Path.GetExtension(ArchivoExcel.FileName) == ".xlsx")
            {
                MiExcel = new XSSFWorkbook(stream);
            }
            else
            {
                MiExcel = new HSSFWorkbook(stream);
            }

            _LeerExcel.Leerexcel(MiExcel);


            return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
        }

        public IActionResult Detalle(int IdCliente)
        {
            string Usuario = _userManager.GetUserName(User);
            _Auditoria.GuardarAuditoria(new AuditoriaModel() { Accion = $"Accesso a detalle cliente: {IdCliente}", Fecha = DateTime.Now, Usuario = Usuario});

            ViewData["Title"] = "Detalle del cliente";
            return View();
        }


    }

}
