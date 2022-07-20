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
            ClienteEmails = new HashSet<ClienteEmail>();
            Tickets = new HashSet<Ticket>();
        }

        public int CodigoCliente { get; set; }
        public string CodigoContabilidad { get; set; }
        public int? IdIdentificacionFiscal { get; set; }
        public string NombreFiscal { get; set; }
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
        public bool? CrearRecibo { get; set; }
        public bool? AceptaFacturaElectronica { get; set; }
        public bool? NoVender { get; set; }
        public bool? NoImprimirEnListados { get; set; }
        public bool? CesionDatos { get; set; }
        public bool? EnviooComunicaciones { get; set; }
        public string CuentaContableTresDigitos { get; set; }
        public string IdentificacionFiscal { get; set; }

        public virtual TipoIdentificacionFiscal IdIdentificacionFiscalNavigation { get; set; }
        public virtual Paise IdPaisNavigation { get; set; }
        public virtual ICollection<ClienteCuenta> ClienteCuenta { get; set; }
        public virtual ICollection<ClienteDireccione> ClienteDirecciones { get; set; }
        public virtual ICollection<ClienteEmail> ClienteEmails { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
