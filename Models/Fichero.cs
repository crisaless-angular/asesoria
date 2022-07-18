using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BA002.Web.Models
{
    public class Fichero
    {
        
        public int IdProyectoUsuario { get; set; }
        public int IdProyecto { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        [Required]
        public string Descripcion{ get; set; }
        [Required]
        public string Comentario { get; set; }
        [Required]
        public IFormFile Archivo { get; set; }
    }
}