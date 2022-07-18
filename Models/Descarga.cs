using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BA002.Web.Models
{
    public class Descarga
    {
        [Required] 
        public int IdFichero { get; set; }
        [Required]
        [Range(100000, 999999, ErrorMessage = "Clave incorrecta")]
        public int OTP { get; set; }
    }
}