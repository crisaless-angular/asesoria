using System;

namespace Web.Models
{
    public class ClientesViewModel
    {
        public int CODIGO_CLIENTE { get; set; }
        public string CODIGO_CONTABILIDAD { get; set; }
        public int TIPO_IDENTIFICACION_FISCAL { get; set; }
        public string IDENTIFICACION_FISCAL { get; set; }
        public string NOMBRE_FISCAL { get; set; }
        public string NOMBRE_COMERCIAL { get; set; }
        public string DOMICILIO { get; set; }
        public string CODIGO_POSTAL { get; set; }
        public string POBLACION { get; set; }
        public string PROVINCIA { get; set; }
        public string PAIS { get; set; }
        public string TELEFONO { get; set; }
        public string FAX { get; set; }
        public string MOVIL { get; set; }
        public string OBSERVACIONES { get; set; }
        public DateTime FECHA_ALTA { get; set; }
        public DateTime MODIFICADO { get; set; }
        public string DIRECCION_WEB { get; set; }
        public string MENSAJE_EMERGENTE { get; set; }
        public string CODIGO_PROVEEDOR { get; set; }
        public bool NO_FACTURAS { get; set; }
        public bool CREAR_RECIBO { get; set; }
        public bool ACEPTA_FACTURA_ELECTRONICA { get; set; }
        public bool NO_VENDER { get; set; }
        public bool NO_IMPRIMIR_EN_LISTADOS { get; set; }
        public bool CESION_DATOS { get; set; }
        public bool ENVIOO_COMUNICACIONES { get; set; }
        public string CUENTA_CONTABLE_TRES_DIGITOS { get; set; }
        public string EMAILPRINCIPAL { get; set; }

    }
}
