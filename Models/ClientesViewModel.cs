using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.Data;

namespace Web.Models
{
    public class ClientesViewModel
    {
        public int CODIGO_CLIENTE { get; set; }
        
        [Display(Name = "Código contabilidad")]
        public string CODIGO_CONTABILIDAD { get; set; }

        [Display(Name = "Tipo identificación fiscal")]
        public string TIPO_IDENTIFICACION_FISCAL { get; set; }

        [Display(Name = "Identificación fiscal")]
        public string IDENTIFICACION_FISCAL { get; set; }

        [Display(Name = "Nombre fiscal")]
        public string NOMBRE_FISCAL { get; set; }

        [Display(Name = "Nombre comercial")]
        public string NOMBRE_COMERCIAL { get; set; }

        [Display(Name = "Domicilio")]
        public string DOMICILIO { get; set; }

        [RegularExpression("0[1-9][0-9]{3}|[1-4][0-9]{4}|5[0-2][0-9]{3}", ErrorMessage = "Indique un código postal correcto")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Longitud incorrecta")]
        [Range(0, int.MaxValue, ErrorMessage = "Solo se permiten numeros")]
        [Display(Name = "Código postal")]
        public string CODIGO_POSTAL { get; set; }

        [Display(Name = "Población")]
        public string POBLACION { get; set; }

        [Display(Name = "Provincia")]
        public string PROVINCIA { get; set; }

        [Display(Name = "País")]
        public string PAIS { get; set; }

        [Display(Name = "Teléfono")]
        public string TELEFONO { get; set; }

        [Display(Name = "Móvil")]
        public string MOVIL { get; set; }

        [Display(Name = "Observaciones")]
        public string OBSERVACIONES { get; set; }

        [Display(Name = "Fecha de alta")]
        public DateTime FECHA_ALTA { get; set; }

        [Display(Name = "Modificado")]
        public DateTime MODIFICADO { get; set; }

        [Display(Name = "Dirección web")]
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

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EMAILPRINCIPAL { get; set; }

        [Display(Name = "Agente")]
        public string AGENTE { get; set; }

        [Display(Name = "Tipo de cliente")]
        public string TIPO_CLIENTE { get; set; }

        public string IBAN { get; set; }
        public string BIC { get; set; }
        public string BANCO { get; set; }


    }
}
