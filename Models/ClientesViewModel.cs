using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.Data;

namespace Web.Models
{
    public class ClientesViewModel
    {
        public int CODIGO_CLIENTE { get; set; }

        [Required(ErrorMessage = "El Código contabilidad es obligatorio")]
        [Display(Name = "Código contabilidad")]
        public string CODIGO_CONTABILIDAD { get; set; }

        [Display(Name = "Tipo identificación fiscal")]
        public string TIPO_IDENTIFICACION_FISCAL { get; set; }

        [Required(ErrorMessage = "La Identificación fiscal es obligatoria")]
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

        [Required(ErrorMessage = "La fecha de alta es obligatoria")]
        [Display(Name = "Fecha de alta")]
        public DateTime FECHA_ALTA { get; set; } = DateTime.Now;

        [Display(Name = "Modificado")]
        public DateTime MODIFICADO { get; set; } = DateTime.Now;

        [Display(Name = "Dirección web")]
        public string DIRECCION_WEB { get; set; }

        [Display(Name = "Mensaje emergente")]
        public string MENSAJE_EMERGENTE { get; set; }

        [Display(Name = "Código de proveedor")]
        public string CODIGO_PROVEEDOR { get; set; }


        [Display(Name = "No realizar facturas a este cliente")]
        public bool NO_FACTURAS { get; set; }

        [Display(Name = "Crear recibo al remitir factura")]
        public bool CREAR_RECIBO { get; set; }

        [Display(Name = "Acepta factura electrónica")]
        public bool ACEPTA_FACTURA_ELECTRONICA { get; set; }

        [Display(Name = "No vender a este cliente")]
        public bool NO_VENDER { get; set; }

        [Display(Name = "No imprimir en los listados")]
        public bool NO_IMPRIMIR_EN_LISTADOS { get; set; }

        [Display(Name = "Acepta la cesión de sus datos")]
        public bool CESION_DATOS { get; set; }

        [Display(Name = "Acepta el envío de comunicaciones")]
        public bool ENVIOO_COMUNICACIONES { get; set; }

        [Display(Name = "Cuenta contable a 3 dígitos")]
        public string CUENTA_CONTABLE_TRES_DIGITOS { get; set; }

        [Required(ErrorMessage = "El Email es obligatorio")]
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

        [Display(Name = "Actividad")]
        public string ACTIVIDAD { get; set; }

        [Display(Name = "Iva")]
        public bool IVA { get; set; }

        [Display(Name = "Forma de pago")]
        public int FORMA_PAGO { get; set; }

        [Display(Name = "Recargo")]
        public bool RECARGO { get; set; }

        [Display(Name = "Persona de contacto")]
        public string PERSONA_CONTACTO { get; set; }
    }
}
