using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class ClienteDireccione
    {
        public int IdDireccion { get; set; }
        public string Descripcion { get; set; }
        public int? IdCliente { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}
