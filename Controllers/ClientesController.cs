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
            ViewData["PAISES"] = _UnitOfWork.PaisesRepository.GetAll();
            ViewData["TIPO_IDENTIFICACION_FISCAL_ITEMS"] = _UnitOfWork.TipoIdentificacionFiscalRepository.GetAll();
            ViewData["TIPO_CLIENTE"] = _UnitOfWork.TipoClienteRepository.GetAll();
            ViewData["AGENTE"] = _UnitOfWork.AgenteRepository.GetAll();
            ViewData["FORMA_PAGO"] = _UnitOfWork.FormasPagoRepository.GetAll();
            ViewData["ACTIVIDAD"] = _UnitOfWork.ActividadRepository.GetAll();
            ViewData["Title"] = "Crear un cliente";
            return View();
        }

        [HttpPost]
        public IActionResult Crear(ClientesViewModel model)
        {
            ViewData["PAISES"] = _UnitOfWork.PaisesRepository.GetAll();
            ViewData["TIPO_IDENTIFICACION_FISCAL_ITEMS"] = _UnitOfWork.TipoIdentificacionFiscalRepository.GetAll();
            ViewData["TIPO_CLIENTE"] = _UnitOfWork.TipoClienteRepository.GetAll();
            ViewData["AGENTE"] = _UnitOfWork.AgenteRepository.GetAll();
            ViewData["FORMA_PAGO"] = _UnitOfWork.FormasPagoRepository.GetAll();
            ViewData["ACTIVIDAD"] = _UnitOfWork.ActividadRepository.GetAll();

            bool Save = false;

            if (model.IBAN != null && !Utilidades.Utilidades.ValidateIban(model.IBAN))
            {
                ModelState.AddModelError("", "Campo IBAN no es valido");
                ModelState.AddModelError("IBAN", "El IBAN no es valido");
            }

            try
            {

                bool CodigoContabilidad = _UnitOfWork.ClienteRepository.GetAll().Where(x => x.CodigoContabilidad == model.CODIGO_CONTABILIDAD).Count() > 0;
                
                if(CodigoContabilidad)
                {
                    ModelState.AddModelError("", "Campo código contabilidad no es valido");
                    ModelState.AddModelError("CODIGO_CONTABILIDAD", "El código contabilidad ya existe");
                }

                if (!ModelState.IsValid)
                    return View(model);

                _UnitOfWork.ClienteRepository.Add(Cast_Cliente_ViewCliente(model));
                _UnitOfWork.Save();
                model.CODIGO_CLIENTE = _UnitOfWork.ClienteRepository.GetAll().Where(x => x.CodigoContabilidad == model.CODIGO_CONTABILIDAD).FirstOrDefault().CodigoCliente;
                Save = true;
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", e.Message.ToString());
            }

            if (!ModelState.IsValid)
                return View(model);

            if (model.EMAILPRINCIPAL != null && Save)
            {
                _UnitOfWork.ClienteEmailRepository.Add(Cast_ClienteMail_ViewCliente(model));
                _UnitOfWork.Save();
            }

            if(model.IBAN != null && Save)
            {
                _UnitOfWork.ClienteCuentaRepository.Add(Cast_ClienteCuenta_ViewCliente(model));
                _UnitOfWork.Save();
            }

            return RedirectToAction("Index", "Clientes");
            

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

        public Cliente Cast_Cliente_ViewCliente(ClientesViewModel model)
        {
            Cliente cliente = new Cliente()
            {
                CodigoContabilidad = model.CODIGO_CONTABILIDAD,
                IdIdentificacionFiscal = int.Parse(model.TIPO_IDENTIFICACION_FISCAL),
                NombreFiscal = model.NOMBRE_FISCAL,
                NombreComercial = model.NOMBRE_COMERCIAL,
                Domicilio = model.DOMICILIO,
                CodigoPostal = model.CODIGO_POSTAL,
                Poblacion = model.POBLACION,
                Provincia = model.PROVINCIA,
                IdPais = int.Parse(model.PAIS),
                Telefono = model.TELEFONO,
                Movil = model.MOVIL,
                Observaciones = model.OBSERVACIONES,
                FechaAlta = model.FECHA_ALTA,
                Modificado = DateTime.Now,
                DireccionWeb = model.DIRECCION_WEB,
                MensajeEmergente = model.MENSAJE_EMERGENTE,
                CodigoProveedor = model.CODIGO_PROVEEDOR,
                NoFacturas = model.NO_FACTURAS,
                CrearRecibo = model.CREAR_RECIBO,
                AceptaFacturaElectronica = model.ACEPTA_FACTURA_ELECTRONICA,
                NoVender = model.NO_VENDER,
                NoImprimirEnListados = model.NO_IMPRIMIR_EN_LISTADOS,
                CesionDatos = model.CESION_DATOS,
                EnviooComunicaciones = model.ENVIOO_COMUNICACIONES,
                CuentaContableTresDigitos = model.CUENTA_CONTABLE_TRES_DIGITOS,
                IdentificacionFiscal = model.IDENTIFICACION_FISCAL,
                IdFormaPago = model.FORMA_PAGO,
                IdTipoCliente = int.Parse(model.TIPO_CLIENTE),
                IdActividad = int.Parse(model.ACTIVIDAD),
                Iva = model.IVA,
                Recargo = model.RECARGO,
                Agente = int.Parse(model.AGENTE),
                PersonaContacto = model.PERSONA_CONTACTO
            };

            return cliente;
            
        }

        public ClienteEmail Cast_ClienteMail_ViewCliente(ClientesViewModel model)
        {
            ClienteEmail ClienteMail = new ClienteEmail()
            {
                IdCliente = model.CODIGO_CLIENTE,
                Email = model.EMAILPRINCIPAL,
                Activo = true
            };

            return ClienteMail;
        }

        public ClienteCuenta Cast_ClienteCuenta_ViewCliente(ClientesViewModel model)
        {
            ClienteCuenta clienteCuenta = new ClienteCuenta()
            {
                Ccc = model.IBAN.Replace(" ", "").Remove(0, 4),
                Iban = model.IBAN.Replace(" ", ""),
                Banco = model.BANCO,
                IdCliente = model.CODIGO_CLIENTE,
                Activa = true
            };

            return clienteCuenta;
        }

    }

}
