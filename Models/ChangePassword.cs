using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ChangePassword
    {
        [Required]
        [StringLength(50, ErrorMessage = "La contraseña debe tener al menos 9 carácteres y un máximo de 100 carácteres", MinimumLength = 8)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "La contraseña debe tener, una letra en mayúscula, un número y un carácter especial. ")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repetir nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
        public string UserId { get; set; }
    }
}
