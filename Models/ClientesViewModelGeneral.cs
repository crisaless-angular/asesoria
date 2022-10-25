using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ClientesViewModelGeneral
    {
        public int CODIGO_CLIENTE { get; set; }
       
        [Display(Name = "Nombre comercial")]
        public string NOMBRE_COMERCIAL { get; set; }

        [Display(Name = "Domicilio Fiscal")]
        public string DOMICILIO { get; set; }

        [RegularExpression("0[1-9][0-9]{3}|[1-4][0-9]{4}|5[0-2][0-9]{3}", ErrorMessage = "Indique un código postal correcto")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Longitud incorrecta")]
        [Range(0, int.MaxValue, ErrorMessage = "Solo se permiten numeros")]
        [Display(Name = "Código postal Fiscal")]
        public string CODIGO_POSTAL { get; set; }

        [Display(Name = "Población Fiscal")]
        public string POBLACION { get; set; }

        [Display(Name = "Provincia Fiscal")]
        public string PROVINCIA { get; set; }

        [Display(Name = "País Fiscal")]
        public string PAIS { get; set; }

        [Display(Name = "Domicilio Actividad")]
        public string DOMICILIO_ACTIVIDAD { get; set; }

        [RegularExpression("0[1-9][0-9]{3}|[1-4][0-9]{4}|5[0-2][0-9]{3}", ErrorMessage = "Indique un código postal correcto")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Longitud incorrecta")]
        [Range(0, int.MaxValue, ErrorMessage = "Solo se permiten numeros")]
        [Display(Name = "Código postal Actividad")]
        public string CODIGO_POSTAL_ACTIVIDAD { get; set; }

        [Display(Name = "Población Actividad")]
        public string POBLACION_ACTIVIDAD { get; set; }

        [Display(Name = "Provincia Actividad")]
        public string PROVINCIA_ACTIVIDAD { get; set; }

        [Display(Name = "País Actividad")]
        public string PAIS_ACTIVIDAD { get; set; }

        [Display(Name = "Teléfono")]
        public string TELEFONO { get; set; }

        [Display(Name = "Móvil")]
        public string MOVIL { get; set; }

        [Required(ErrorMessage = "El Email es obligatorio")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El formato de email no es correcto")]
        [EmailAddress]
        public string EMAILPRINCIPAL { get; set; }

        [Display(Name = "Agente")]
        public string AGENTE { get; set; }

        [Display(Name = "Tipo de cliente")]
        public string TIPO_CLIENTE { get; set; }

        [Display(Name = "Nuevo Tipo de cliente")]
        public string NUEVO_TIPO_CLIENTE { get; set; }

        public string IBAN { get; set; }
        public string BIC { get; set; }
        public string BANCO { get; set; }
 
        [Display(Name = "Nombre")]
        public string NOMBRE_COMPLETO { get; set; }

        [Display(Name = "Apellido uno")]
        public string APELLIDO_UNO { get; set; }

        [Display(Name = "Apellido dos")]
        public string APELLIDO_DOS { get; set; }

    }
}
