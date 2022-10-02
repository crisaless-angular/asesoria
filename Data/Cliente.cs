using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class Cliente
    {
        public Cliente()
        {
            ClienteCuenta = new HashSet<ClienteCuenta>();
            ClienteDirecciones = new HashSet<ClienteDireccione>();
            ClienteMails = new HashSet<ClienteMail>();
            Tickets = new HashSet<Ticket>();
        }

        public int CodigoCliente { get; set; }
        public int? IdIdentificacionFiscal { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreComercial { get; set; }
        public string Domicilio { get; set; }
        public string CodigoPostal { get; set; }
        public string Poblacion { get; set; }
        public string Provincia { get; set; }
        public int? IdPais { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Movil { get; set; }
        public string Observaciones { get; set; }
        public DateTime? FechaAlta { get; set; }
        public DateTime? Modificado { get; set; }
        public string DireccionWeb { get; set; }
        public string MensajeEmergente { get; set; }
        public string CodigoProveedor { get; set; }
        public bool? NoFacturas { get; set; }
        public bool? AceptaFacturaElectronica { get; set; }
        public bool? CesionDatos { get; set; }
        public bool? EnviooComunicaciones { get; set; }
        public string IdentificacionFiscal { get; set; }
        public int? PersonaContacto { get; set; }
        public int? IdFormaPago { get; set; }
        public int? IdTipoCliente { get; set; }
        public int? IdActividad { get; set; }
        public bool? Iva { get; set; }
        public int? Agente { get; set; }
        public string ApellidoUno { get; set; }
        public string ApellidoDos { get; set; }
        public DateTime? FechaContratacionTh { get; set; }
        public DateTime? FechaAltaActividad { get; set; }
        public int? Iae { get; set; }
        public int? Cnae { get; set; }
        public string CuotaMensual { get; set; }

        public virtual Agente AgenteNavigation { get; set; }
        public virtual Actividad IdActividadNavigation { get; set; }
        public virtual FormasPago IdFormaPagoNavigation { get; set; }
        public virtual TipoIdentificacionFiscal IdIdentificacionFiscalNavigation { get; set; }
        public virtual Paise IdPaisNavigation { get; set; }
        public virtual TipoCliente IdTipoClienteNavigation { get; set; }
        public virtual ICollection<ClienteCuenta> ClienteCuenta { get; set; }
        public virtual ICollection<ClienteDireccione> ClienteDirecciones { get; set; }
        public virtual ICollection<ClienteMail> ClienteMails { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
