using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class Empresa
    {
        public int IdEmpresa { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string TelefonoMovil { get; set; }
        public string TelefonoFijo { get; set; }
        public string Cif { get; set; }
        public string Logo { get; set; }
        public string Logotipo { get; set; }
    }
}
