using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class Cliente1
    {
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
        public bool? CrearRecibo { get; set; }
        public bool? AceptaFacturaElectronica { get; set; }
        public bool? NoVender { get; set; }
        public bool? NoImprimirEnListados { get; set; }
        public bool? CesionDatos { get; set; }
        public bool? EnviooComunicaciones { get; set; }
        public string CuentaContableTresDigitos { get; set; }
        public string IdentificacionFiscal { get; set; }
        public string PersonaContacto { get; set; }
        public int? IdFormaPago { get; set; }
        public int? IdTipoCliente { get; set; }
        public int? IdActividad { get; set; }
        public bool? Iva { get; set; }
        public bool? Recargo { get; set; }
        public int? Agente { get; set; }
    }
}
