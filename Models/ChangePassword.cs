using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ChangePassword
    {
        [Required]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {1} carácteres y un máximo de {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repetir nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
