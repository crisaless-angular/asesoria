using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
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
                        Bic = model.BIC,
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

                cliente.ClienteMails = _UnitOfWork.ClienteEmailRepository.GetAll().Where(x => x.IdCliente == cliente.CodigoCliente).ToList();
                cliente.ClienteCuenta = _UnitOfWork.ClienteCuentaRepository.GetAll().Where(x => x.IdCliente == cliente.CodigoCliente).ToList();
                
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

        [HttpPost]
        public IActionResult Detalle(ClientesViewModel model, bool general = false)
        {
            try
            {
                Cliente cliente = _UnitOfWork.ClienteRepository.GetEntity(model.CODIGO_CLIENTE);
                
                ViewData["PAISES"] = _UnitOfWork.PaisesRepository.GetAll();
                ViewData["TIPO_IDENTIFICACION_FISCAL_ITEMS"] = _UnitOfWork.TipoIdentificacionFiscalRepository.GetAll();
                ViewData["TIPO_CLIENTE"] = _UnitOfWork.TipoClienteRepository.GetAll();
                ViewData["AGENTE"] = _UnitOfWork.AgenteRepository.GetAll();
                ViewData["FORMA_PAGO"] = _UnitOfWork.FormasPagoRepository.GetAll();
                ViewData["ACTIVIDAD"] = _UnitOfWork.ActividadRepository.GetAll();
                ViewData["Title"] = $"Detalle del cliente: {(model.NOMBRE_COMPLETO == null ? model.NOMBRE_COMERCIAL : model.NOMBRE_COMPLETO)}";
                
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

                    Cliente modelInsertado = Cast_Cliente_ViewCliente_update(model, cliente);
                    _UnitOfWork.ClienteRepository.Update(modelInsertado);
                    _UnitOfWork.Save();
                    model.CODIGO_CLIENTE = modelInsertado.CodigoCliente;
                    Save = true;

                    //if (model.EMAILPRINCIPAL != null && Save)
                    //{
                    //    var clienteEmail = _UnitOfWork.ClienteEmailRepository.GetAll().Where(x => x.IdCliente == model.CODIGO_CLIENTE).ToList();

                    //    foreach (var cuentaCliente in clienteEmail)
                    //    {
                             
                    //    }

                    //    Email ClienteMail = new Email()
                    //    {
                    //        Email1 = model.EMAILPRINCIPAL,
                    //        Activo = true
                    //    };

                    //    //_UnitOfWork.EmailRepository.Add(ClienteMail);
                    //    //_UnitOfWork.Save();

                    //    //ClienteMail clienteEmail = new ClienteMail();
                    //    //clienteEmail.IdCliente = model.CODIGO_CLIENTE;
                    //    //clienteEmail.IdMail = ClienteMail.IdEmailCliente;

                    //    //_UnitOfWork.ClienteEmailRepository.Add(clienteEmail);
                    //    //_UnitOfWork.Save();

                    //}

                    //if (model.IBAN != null && Save)
                    //{
                    //    Cuenta Cuenta = new Cuenta()
                    //    {
                    //        Ccc = model.IBAN.Replace(" ", "").Remove(0, 4),
                    //        Iban = model.IBAN.Replace(" ", ""),
                    //        Banco = model.BANCO,
                    //        Bic = model.BIC,
                    //        Activa = true
                    //    };

                    //    _UnitOfWork.CuentaRepository.Add(Cuenta);
                    //    _UnitOfWork.Save();

                    //    ClienteCuenta clienteCuenta = new ClienteCuenta();
                    //    clienteCuenta.IdCuenta = Cuenta.IdCuenta;
                    //    clienteCuenta.IdCliente = model.CODIGO_CLIENTE;

                    //    _UnitOfWork.ClienteCuentaRepository.Add(clienteCuenta);
                    //    _UnitOfWork.Save();

                    //    List<ClienteCuenta> ListCuentas = _UnitOfWork.ClienteCuentaRepository.GetAll().Where(x => x.IdCliente == model.CODIGO_CLIENTE && x.IdCuenta != Cuenta.IdCuenta).ToList();

                    //    for (int i = 0; i < ListCuentas.Count(); i++)
                    //    {
                    //        Cuenta cuenta = _UnitOfWork.CuentaRepository.GetEntity(ListCuentas[i].IdCuenta);
                    //        cuenta.Activa = false;
                    //        _UnitOfWork.CuentaRepository.Update(cuenta);
                    //        _UnitOfWork.Save();
                    //    }
                        

                    //}

                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message.ToString());
                }

                if (!ModelState.IsValid)
                    return View(model);
                
            }
            catch (Exception e)
            {
                return View(model);
            }


            model.MostrarMensajeEmergente = true;
            model.MENSAJE_EMERGENTE = "Datos actualizados correctamente!";
            
            return View(model);
        }
        
        public Cliente Cast_Cliente_ViewCliente(ClientesViewModel model)
        {
            Cliente cliente = new Cliente()
            {
                IdIdentificacionFiscal = model.TIPO_IDENTIFICACION_FISCAL == null ? 1 : int.Parse(model.TIPO_IDENTIFICACION_FISCAL),
                NombreCompleto = model.NOMBRE_COMPLETO,
                NombreComercial = model.NOMBRE_COMERCIAL,
                Domicilio = model.DOMICILIO,
                CodigoPostal = model.CODIGO_POSTAL,
                Poblacion = model.POBLACION,
                Provincia = model.PROVINCIA,
                IdPais = model.PAIS == null ? 74 : int.Parse(model.PAIS),
                Telefono = model.TELEFONO,
                Movil = model.MOVIL,
                Observaciones = model.OBSERVACIONES,
                FechaAlta = model.FECHA_ALTA,
                Modificado = model.FECHA_ALTA,
                IdentificacionFiscal = model.IDENTIFICACION_FISCAL,
                IdFormaPago = model.FORMA_PAGO,
                IdTipoCliente = model.TIPO_CLIENTE == null ? 1 : int.Parse(model.TIPO_CLIENTE),
                IdActividad = model.ACTIVIDAD == null ? 1 : int.Parse(model.ACTIVIDAD),
                Iva = model.IVA,
                Agente = model.AGENTE == null ? 1 : int.Parse(model.AGENTE),
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
                IdPaisActividad = model.PAIS_ACTIVIDAD == null ? 74 : int.Parse(model.PAIS_ACTIVIDAD),
            };

            return cliente;

        }

        private Cuenta ReturnCuenta(Cliente model)
        {
            for (int i = 0; i < model.ClienteCuenta.Count(); i++)
            {
                Cuenta FindCuenta = _UnitOfWork.CuentaRepository.GetEntity(model.ClienteCuenta.ElementAt(i).IdCuenta);

                if (FindCuenta.Activa == true)
                {
                    return FindCuenta;
                }
            }
            return null;
        }

        private Email ReturEmail(Cliente model)
        {
            for (int i = 0; i < model.ClienteMails.Count(); i++)
            {
                Email FindEmail = _UnitOfWork.EmailRepository.GetEntity(model.ClienteMails.ElementAt(i).IdMail);

                if (FindEmail.Activo == true)
                {
                    return FindEmail;
                }
            }
            return null;
        }

        public Cliente Cast_Cliente_ViewCliente_update(ClientesViewModel model, Cliente cliente)
        {
            cliente.IdIdentificacionFiscal = model.TIPO_IDENTIFICACION_FISCAL == null ? 1 : int.Parse(model.TIPO_IDENTIFICACION_FISCAL);
            cliente.NombreCompleto = model.NOMBRE_COMPLETO;
            cliente.NombreComercial = model.NOMBRE_COMERCIAL;
            cliente.Domicilio = model.DOMICILIO;
            cliente.CodigoPostal = model.CODIGO_POSTAL;
            cliente.Poblacion = model.POBLACION;
            cliente.Provincia = model.PROVINCIA;
            cliente.IdPais = model.PAIS == null ? 74 : int.Parse(model.PAIS);
            cliente.Telefono = model.TELEFONO;
            cliente.Movil = model.MOVIL;
            cliente.Observaciones = model.OBSERVACIONES;
            cliente.FechaAlta = model.FECHA_ALTA;
            cliente.Modificado = model.FECHA_ALTA;
            cliente.IdentificacionFiscal = model.IDENTIFICACION_FISCAL;
            cliente.IdFormaPago = model.FORMA_PAGO;
            cliente.IdTipoCliente = model.TIPO_CLIENTE == null ? 1 : int.Parse(model.TIPO_CLIENTE);
            cliente.IdActividad = model.ACTIVIDAD == null ? 1 : int.Parse(model.ACTIVIDAD);
            cliente.Iva = model.IVA;
            cliente.Agente = model.AGENTE == null ? 1 : int.Parse(model.AGENTE);
            cliente.ApellidoUno = model.APELLIDO_UNO;
            cliente.ApellidoDos = model.APELLIDO_DOS;
            cliente.FechaContratacionTh = model.FECHA_CONTRATACION_TH;
            cliente.FechaAltaActividad = model.FECHA_ALTA_ACTIVIDAD;
            cliente.Iae = model.IAE;
            cliente.Cnae = model.CNAE;
            cliente.CuotaMensual = model.CUOTA_MENSUAL;
            cliente.DomicilioActividad = model.DOMICILIO_ACTIVIDAD;
            cliente.CodigoPostalActividad = model.CODIGO_POSTAL_ACTIVIDAD;
            cliente.PoblacionActividad = model.POBLACION_ACTIVIDAD;
            cliente.ProvinciaActividad = model.PROVINCIA_ACTIVIDAD;
            cliente.IdPaisActividad = model.PAIS_ACTIVIDAD == null ? 74 : int.Parse(model.PAIS_ACTIVIDAD);
            
            return cliente;

        }

        public ClientesViewModel Cast_ViewCliente_Cliente(Cliente model)
        {
            PersonasContacto personaContacto = _UnitOfWork.PersonaContactoRepository.GetEntity(model.PersonaContacto.Value);
            var objetoPerson = new
            {
                idPersona = personaContacto.IdPersonaContacto,
                NombrePersona = personaContacto.Nombre,
                TelefonoPersona = personaContacto.Telefono,
                EmailPersona = personaContacto.Email,
            };

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
                IAE = model.Iae,
                CNAE = model.Cnae,
                CUOTA_MENSUAL = model.CuotaMensual,
                DOMICILIO_ACTIVIDAD = model.DomicilioActividad,
                CODIGO_POSTAL_ACTIVIDAD = model.CodigoPostalActividad,
                POBLACION_ACTIVIDAD = model.PoblacionActividad,
                PROVINCIA_ACTIVIDAD = model.ProvinciaActividad,
                PAIS_ACTIVIDAD = model.IdPaisActividad == null ? "34" : model.IdPaisActividad.Value.ToString(),
                EMAILPRINCIPAL = ReturEmail(model).Email1,
                IBAN = ReturnCuenta(model).Iban,
                BANCO = ReturnCuenta(model).Banco,
                BIC = ReturnCuenta(model).Bic,
                PERSONA_CONTACTO = JsonConvert.SerializeObject(objetoPerson)
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

        [HttpGet]
        public List<Email> ReturnEmailsCliente(int codCliente)
        {
            //string userId = String.Empty;

            //if(HttpContext.User.Identity.IsAuthenticated)
            //{
            //    userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //}

            //return userId;

            return (from clientesEmail in this._UnitOfWork.ClienteEmailRepository.GetAll()
                    join email in this._UnitOfWork.EmailRepository.GetAll()
                    on clientesEmail.IdMail equals email.IdEmailCliente
                    where clientesEmail.IdCliente == codCliente

                    select new Email()
                    {
                        IdEmailCliente = email.IdEmailCliente,
                        Email1 = email.Email1,
                        Activo = email.Activo,
                    }
                ).OrderByDescending(x => x.Activo).ToList();
            
        }

        [HttpPost]
        public string CambiarEmailCliente(int idEmailCliente)
        {
            int IdCliente = _UnitOfWork.ClienteEmailRepository.GetAll().Where(x => x.IdMail == idEmailCliente).FirstOrDefault().IdCliente;

            inhabilitarEmails(IdCliente);

            Email email = _UnitOfWork.EmailRepository.GetEntity(idEmailCliente);
            email.Activo = true;

            _UnitOfWork.EmailRepository.Update(email);
            _UnitOfWork.Save();

            return email.Email1;
        }

        [HttpGet]
        public List<Cuenta> ReturnCuentasCliente(int codCliente)
        {
            return (from clientesCuentas in this._UnitOfWork.ClienteCuentaRepository.GetAll()
                    join Cuenta in this._UnitOfWork.CuentaRepository.GetAll()
                    on clientesCuentas.IdCuenta equals Cuenta.IdCuenta
                    where clientesCuentas.IdCliente == codCliente

                    select new Cuenta()
                    {
                        IdCuenta = Cuenta.IdCuenta,
                        Iban = Cuenta.Iban,
                        Bic = Cuenta.Bic,
                        Banco = Cuenta.Banco,
                        Activa = Cuenta.Activa
                    }
                ).OrderByDescending(x => x.Activa).ToList();

        }

        [HttpPost]
        public string[] CambiarCuentaCliente(int idCuentaCliente)
        {
            int IdCliente = _UnitOfWork.ClienteCuentaRepository.GetAll().Where(x => x.IdCuenta == idCuentaCliente).FirstOrDefault().IdCliente;

            inhabilitarCuentas(IdCliente);

            Cuenta cuenta = _UnitOfWork.CuentaRepository.GetEntity(idCuentaCliente);
            cuenta.Activa = true;

            _UnitOfWork.CuentaRepository.Update(cuenta);
            _UnitOfWork.Save();

            string[] nuevosdatos = { cuenta.Iban, cuenta.Banco, cuenta.Bic };
            return nuevosdatos;
        }

        [HttpPost]
        public void AddCuentaCliente(int idCodigoCliente, string iban, string banco, string bic)
        {
            Cliente Cliente = _UnitOfWork.ClienteRepository.GetEntity(idCodigoCliente);

            inhabilitarCuentas(Cliente.CodigoCliente);
            
            Cuenta cuenta = new Cuenta()
            {
                Iban = iban,
                Banco = banco,
                Bic = bic,
                Activa = true
            };

            _UnitOfWork.CuentaRepository.Add(cuenta);
            _UnitOfWork.Save();

            ClienteCuenta clienteCuenta = new ClienteCuenta()
            {
                IdCliente = idCodigoCliente,
                IdCuenta = cuenta.IdCuenta
            };

            _UnitOfWork.ClienteCuentaRepository.Add(clienteCuenta);
            _UnitOfWork.Save();
            
        }

        [HttpPost]
        public void AddEmailCliente(int idCodigoCliente, string email)
        {
            Cliente Cliente = _UnitOfWork.ClienteRepository.GetEntity(idCodigoCliente);

            inhabilitarEmails(Cliente.CodigoCliente);

            Email emailCliente = new Email()
            {
                Email1 = email,
                Activo = true
            };

            _UnitOfWork.EmailRepository.Add(emailCliente);
            _UnitOfWork.Save();

            ClienteMail clienteEmail = new ClienteMail()
            {
                IdCliente = idCodigoCliente,
                IdMail = emailCliente.IdEmailCliente
            };

            _UnitOfWork.ClienteEmailRepository.Add(clienteEmail);
            _UnitOfWork.Save();
            
        }

        private void inhabilitarCuentas(int IdCliente)
        {
            List<ClienteCuenta> ListClienteCuentas = _UnitOfWork.ClienteCuentaRepository.GetAll().Where(x => x.IdCliente == IdCliente).ToList();

            foreach (ClienteCuenta item in ListClienteCuentas)
            {
                Cuenta cuentaCambio = _UnitOfWork.CuentaRepository.GetAll().Where(x => x.IdCuenta == item.IdCuenta).FirstOrDefault();
                cuentaCambio.Activa = false;
                _UnitOfWork.CuentaRepository.Update(cuentaCambio);
                _UnitOfWork.Save();
            }
        }

        private void inhabilitarEmails(int IdCliente)
        {
            List<ClienteMail> ListClienteEMails = _UnitOfWork.ClienteEmailRepository.GetAll().Where(x => x.IdCliente == IdCliente).ToList();

            foreach (ClienteMail item in ListClienteEMails)
            {
                Email emailCambio = _UnitOfWork.EmailRepository.GetAll().Where(x => x.IdEmailCliente == item.IdMail).FirstOrDefault();
                emailCambio.Activo = false;
                _UnitOfWork.EmailRepository.Update(emailCambio);
                _UnitOfWork.Save();
            }
        }

        [HttpGet]
        public List<PersonasContacto> ReturnPersonasCliente(int codCliente)
        {
            return (from Personas in this._UnitOfWork.PersonaContactoRepository.GetAll()
                    join Cliente in this._UnitOfWork.ClienteRepository.GetAll()
                    on Personas.IdPersonaContacto equals Cliente.PersonaContacto
                    where Cliente.CodigoCliente == codCliente

                    select new PersonasContacto()
                    {
                        IdPersonaContacto = Personas.IdPersonaContacto,
                        Nombre = Personas.Nombre,
                        Telefono = Personas.Telefono,
                        Email = Personas.Email,
                    }
                ).OrderByDescending(x => x.IdPersonaContacto).ToList();

        }

        [HttpPost]
        public void CambiarPersonaContacto(string objetopersona)
        {
            var objecto = JsonConvert.DeserializeObject<PersonasContacto>(objetopersona);
            PersonasContacto persona = _UnitOfWork.PersonaContactoRepository.GetEntity(objecto.IdPersonaContacto);

            persona.Nombre = objecto.Nombre;
            persona.Telefono = objecto.Telefono;
            persona.Email = objecto.Email;
            

            if (persona != null)
            {
                _UnitOfWork.PersonaContactoRepository.Update(persona);
                _UnitOfWork.Save();
            }

        }

    }

}
