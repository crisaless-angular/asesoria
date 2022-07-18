using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace BA002.Web.Models
{
    public class Usuarios
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Usuario de acceso")]
        [EmailAddress]
        public string UserName { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        public string IdRol { get; set; }
        public string Rol { get; set; }
    }
}
