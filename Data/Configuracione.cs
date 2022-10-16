using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class Configuracione
    {
        public int IdConfiguracion { get; set; }
        public string NombreConfiguracion { get; set; }
        public bool? Activa { get; set; }
        public string Descripcion { get; set; }
    }
}
