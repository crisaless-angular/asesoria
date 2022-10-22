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
                    join Email in this._UnitOfWork.EmailRepository.GetAll().DefaultIfEmpty()
                    on EmailCliente.IdMail equals Email.IdEmailCliente
                    join Agentes in this._UnitOfWork.AgenteRepository.GetAll().DefaultIfEmpty()
                    on clientes.Agente equals Agentes.IdAgente
                    join Tipocliente in this._UnitOfWork.TipoClienteRepository.GetAll().DefaultIfEmpty()
                    on clientes.IdTipoCliente equals Tipocliente.IdTipoCliente
                    where Email.Activo == true

                    select new ClientesViewModel()
                    {
                        NOMBRE_COMPLETO = clientes.NombreCompleto,
                        FECHA_CONTRATACION_TH = clientes.FechaContratacionTh.Value,
                        MOVIL = clientes.Movil,
                        EMAILPRINCIPAL = Email.Email1,
                        IDENTIFICACION_FISCAL = clientes.IdentificacionFiscal,
                        AGENTE = Agentes.Agente1,
                        TIPO_CLIENTE = Tipocliente.TipoCliente1,
                        CODIGO_CLIENTE = clientes.CodigoCliente,
                    }

                            ).ToList().OrderBy(x => x.NOMBRE_COMPLETO).ToList();

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

            try
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

                string[] camposVacios = ValidarCamposCrearCliente(model);
                if (camposVacios.Length > 0)
                {
                    for (int i = 0; i < camposVacios.Length; i++)
                    {
                        ModelState.AddModelError(camposVacios[i], $"El campo {camposVacios[i]} es obligatorio");
                        ModelState.AddModelError("", $"El campo {camposVacios[i]} es obligatorio");
                    }
                }

                try
                {

                    if (!ModelState.IsValid)
                        return View(model);

                    Cliente modelInsertado = Cast_Cliente_ViewCliente(model);
                    _UnitOfWork.ClienteRepository.Add(modelInsertado);
                    _UnitOfWork.Save();
                    model.CODIGO_CLIENTE = modelInsertado.CodigoCliente;
                    Save = true;
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message.ToString());
                }

                if (!ModelState.IsValid)
                    return View(model);

                if (model.EMAILPRINCIPAL != null && Save)
                {
                    Email ClienteMail = new Email()
                    {
                        Email1 = model.EMAILPRINCIPAL,
                        Activo = true
                    };
                    
                    _UnitOfWork.EmailRepository.Add(ClienteMail);
                    _UnitOfWork.Save();

                    ClienteMail clienteEmail = new ClienteMail();
                    clienteEmail.IdCliente = model.CODIGO_CLIENTE;
                    clienteEmail.IdMail = ClienteMail.IdEmailCliente;

                    _UnitOfWork.ClienteEmailRepository.Add(clienteEmail);
                    _UnitOfWork.Save();

                }

                if (model.IBAN != null && Save)
                {
                    Cuenta Cuenta = new Cuenta()
                    {
                        Ccc = model.IBAN.Replace(" ", "").Remove(0, 4),
                        Iban = model.IBAN.Replace(" ", ""),
                        Banco = model.BANCO,
                        Activa = true
                    };

                    _UnitOfWork.CuentaRepository.Add(Cuenta);
                    _UnitOfWork.Save();

                    ClienteCuenta clienteCuenta = new ClienteCuenta();
                    clienteCuenta.IdCuenta = Cuenta.IdCuenta;
                    clienteCuenta.IdCliente = model.CODIGO_CLIENTE;

                    _UnitOfWork.ClienteCuentaRepository.Add(clienteCuenta);
                    _UnitOfWork.Save();

                }

            }
            catch(Exception e)
            {
                return RedirectToAction("Index", "Clientes");
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

        public IActionResult Detalle(int CodigoCliente)
        {
            IEnumerable<Cliente> Findcliente = _UnitOfWork.ClienteRepository.GetAll().Where(x => x.CodigoCliente == CodigoCliente);
            Cliente cliente = null;

            if (Findcliente.Count() > 0)
            {
                cliente = Findcliente.FirstOrDefault();

                string Usuario = _userManager.GetUserName(User);
                _Auditoria.GuardarAuditoria(new AuditoriaModel() { Accion = $"Accesso a detalle cliente: {cliente.CodigoCliente}", Fecha = DateTime.Now, Usuario = Usuario });
            }
            else
                return RedirectToAction("Index", "Clientes");

            ViewData["PAISES"] = _UnitOfWork.PaisesRepository.GetAll();
            ViewData["TIPO_IDENTIFICACION_FISCAL_ITEMS"] = _UnitOfWork.TipoIdentificacionFiscalRepository.GetAll();
            ViewData["TIPO_CLIENTE"] = _UnitOfWork.TipoClienteRepository.GetAll();
            ViewData["AGENTE"] = _UnitOfWork.AgenteRepository.GetAll();
            ViewData["FORMA_PAGO"] = _UnitOfWork.FormasPagoRepository.GetAll();
            ViewData["ACTIVIDAD"] = _UnitOfWork.ActividadRepository.GetAll();
            ViewData["Title"] = $"Detalle del cliente: {(cliente.NombreCompleto == null ? cliente.NombreComercial : cliente.NombreCompleto)}";

            return View(Cast_ViewCliente_Cliente(cliente));
        }

        public Cliente Cast_Cliente_ViewCliente(ClientesViewModel model)
        {
            Cliente cliente = new Cliente()
            {
                IdIdentificacionFiscal = int.Parse(model.TIPO_IDENTIFICACION_FISCAL),
                NombreCompleto = model.NOMBRE_COMPLETO,
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
                IdentificacionFiscal = model.IDENTIFICACION_FISCAL,
                IdFormaPago = model.FORMA_PAGO,
                IdTipoCliente = int.Parse(model.TIPO_CLIENTE),
                IdActividad = int.Parse(model.ACTIVIDAD),
                Iva = model.IVA,
                Agente = int.Parse(model.AGENTE),
                ApellidoUno = model.APELLIDO_UNO,
                ApellidoDos = model.APELLIDO_DOS,
                FechaContratacionTh = model.FECHA_CONTRATACION_TH,
                FechaAltaActividad = model.FECHA_ALTA_ACTIVIDAD,
                Iae = model.IAE,
                Cnae = model.CNAE,
                CuotaMensual = model.CUOTA_MENSUAL,
                DomicilioActividad = model.DOMICILIO_ACTIVIDAD,
                CodigoPostalActividad = model.CODIGO_POSTAL_ACTIVIDAD,
                PoblacionActividad = model.POBLACION_ACTIVIDAD,
                ProvinciaActividad = model.PROVINCIA_ACTIVIDAD,
                IdPaisActividad = int.Parse(model.PAIS_ACTIVIDAD),
            };

            return cliente;

        }
        
        public ClientesViewModel Cast_ViewCliente_Cliente(Cliente model)
        {

            ClientesViewModel cliente = new ClientesViewModel()
            {
                
                CODIGO_CLIENTE = model.CodigoCliente,
                TIPO_IDENTIFICACION_FISCAL = model.IdIdentificacionFiscal == null ? ReturnNoData() : model.IdIdentificacionFiscal.ToString(),
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
                MENSAJE_EMERGENTE = model.MensajeEmergente,
                IDENTIFICACION_FISCAL = model.IdentificacionFiscal,
                NOMBRE_COMPLETO = model.NombreCompleto,
                APELLIDO_UNO = model.ApellidoUno,
                APELLIDO_DOS = model.ApellidoDos,
                FECHA_CONTRATACION_TH = model.FechaContratacionTh.Value,
                FECHA_ALTA_ACTIVIDAD = model.FechaAltaActividad.Value,
                
                //NombreCompleto = model.NOMBRE_COMPLETO,
                //ApellidoUno = model.APELLIDO_UNO,
                //ApellidoDos = model.APELLIDO_DOS,
                //Iae = model.IAE,
                //Cnae = model.CNAE,
                //CuotaMensual = model.CUOTA_MENSUAL,
                //DomicilioActividad = model.DOMICILIO_ACTIVIDAD,
                //CodigoPostalActividad = model.CODIGO_POSTAL_ACTIVIDAD,
                //PoblacionActividad = model.POBLACION_ACTIVIDAD,
                //ProvinciaActividad = model.PROVINCIA_ACTIVIDAD,
                //IdPaisActividad = int.Parse(model.PAIS_ACTIVIDAD),

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
            Configuracione ConfiguracionGdrive = _UnitOfWork.ConfiguracionRepository.GetAll().Where(x => x.NombreConfiguracion == "GDrive").FirstOrDefault();

            if (ConfiguracionGdrive != null && ConfiguracionGdrive.Activa == true)
            {
                //_ = await Gdrive.GuardarArchivo();
                //string IdCarpetaCreada = Gdrive.CrearCarpeta("Prueba");
                Gdrive.ListararchivosGdrive();
            }

        }

        private string[] ValidarCamposCrearCliente(ClientesViewModel model)
        {
            string[] camposError = { };

            if (model.EMAILPRINCIPAL == "" || model.EMAILPRINCIPAL == null)
            {
                camposError = camposError.Append("EMAILPRINCIPAL").ToArray();
            }

            if (model.IDENTIFICACION_FISCAL == "" || model.IDENTIFICACION_FISCAL == null)
            {
                camposError = camposError.Append("IDENTIFICACION_FISCAL").ToArray();
            }

            return camposError;
        }

    }

}
