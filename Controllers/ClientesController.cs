using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
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
using Web.Utilidades;

namespace Web.Views.Clientes
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly ILeerExcel _LeerExcel;
        private readonly IAuditoria _Auditoria;
        private readonly UserManager<IdentityUser> _userManager;
        private IConfigurationRoot _ConfigRoot;
        private IHubContext<NotificacionesHub> _hubContext;

        public ClientesController(IUnitOfWork UnitOfWork, ILeerExcel LeerExcel, IAuditoria Auditoria,
            UserManager<IdentityUser> userManager, IConfiguration configRoot, IHubContext<NotificacionesHub> hubContext)
        {
            this._UnitOfWork = UnitOfWork;
            this._LeerExcel = LeerExcel;
            this._Auditoria = Auditoria;
            this._userManager = userManager;
            this._ConfigRoot = (IConfigurationRoot)configRoot;
            _hubContext = hubContext;
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

            ClientesViewModel model = new ClientesViewModel();

            return View(model);
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

        public IActionResult Detalle(string codigoContabilidad)
        {
            IEnumerable<Cliente> Findcliente = _UnitOfWork.ClienteRepository.GetAll().Where(x => x.CodigoContabilidad == codigoContabilidad);
            Cliente cliente = null;
            
            if(Findcliente.Count() > 0)
            {
                cliente = Findcliente.FirstOrDefault();

                string Usuario = _userManager.GetUserName(User);
                _Auditoria.GuardarAuditoria(new AuditoriaModel() { Accion = $"Accesso a detalle cliente: {cliente.CodigoContabilidad}", Fecha = DateTime.Now, Usuario = Usuario });
            }
            else
                return RedirectToAction("Index", "Clientes");

            ViewData["PAISES"] = _UnitOfWork.PaisesRepository.GetAll();
            ViewData["TIPO_IDENTIFICACION_FISCAL_ITEMS"] = _UnitOfWork.TipoIdentificacionFiscalRepository.GetAll();
            ViewData["TIPO_CLIENTE"] = _UnitOfWork.TipoClienteRepository.GetAll();
            ViewData["AGENTE"] = _UnitOfWork.AgenteRepository.GetAll();
            ViewData["FORMA_PAGO"] = _UnitOfWork.FormasPagoRepository.GetAll();
            ViewData["ACTIVIDAD"] = _UnitOfWork.ActividadRepository.GetAll();
            ViewData["Title"] = $"Detalle del cliente: {(cliente.NombreComercial == null ? cliente.NombreFiscal : cliente.NombreComercial)}";
            
            return View(Cast_ViewCliente_Cliente(cliente));
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
                Modificado = model.FECHA_ALTA,
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

        public ClientesViewModel Cast_ViewCliente_Cliente(Cliente model)
        {
            
            ClientesViewModel cliente = new ClientesViewModel() 
            {
                CODIGO_CLIENTE = model.CodigoCliente,
                CODIGO_CONTABILIDAD = model.CodigoContabilidad,
                TIPO_IDENTIFICACION_FISCAL = model.IdIdentificacionFiscal == null ? ReturnNoData() : model.IdIdentificacionFiscal.ToString(),
                NOMBRE_FISCAL = model.NombreFiscal,
                NOMBRE_COMERCIAL = model.NombreComercial,
                DOMICILIO = model.Domicilio,
                CODIGO_POSTAL = model.CodigoPostal,
                POBLACION = model.Poblacion,
                PROVINCIA = model.Provincia,
                PAIS = model.IdPais.ToString(),
                TELEFONO = model.Telefono,
                MOVIL = model.Movil,
                OBSERVACIONES = model.Observaciones,
                FECHA_ALTA = model.FechaAlta.Value,
                MODIFICADO = model.Modificado.Value,
                DIRECCION_WEB = model.DireccionWeb,
                MENSAJE_EMERGENTE = model.MensajeEmergente,
                CODIGO_PROVEEDOR = model.CodigoProveedor,
                NO_FACTURAS = model.NoFacturas.Value,
                CREAR_RECIBO = model.CrearRecibo.Value,
                ACEPTA_FACTURA_ELECTRONICA = model.AceptaFacturaElectronica.Value,
                NO_VENDER = model.NoVender.Value,
                NO_IMPRIMIR_EN_LISTADOS = model.NoImprimirEnListados.Value,
                CESION_DATOS = model.CesionDatos.Value,
                ENVIOO_COMUNICACIONES = model.EnviooComunicaciones.Value,
                CUENTA_CONTABLE_TRES_DIGITOS = model.CuentaContableTresDigitos,
                IDENTIFICACION_FISCAL = model.IdentificacionFiscal
            };

            return cliente;
        }

        public string ReturnNoData()
        {
            return "No datos";
        }

        [HttpPost]
        public async void mensajeInstantaneo(string mensaje)
        {
            //envio mesaje realtime
            await _hubContext.Clients.All.SendAsync("RecibirMensaje", mensaje);
            //envio mesaje realtime
        }

        [HttpPost]
        public async void GDriveModule()
        {
            Gdrive.Coonnect();
        }

    }

}
