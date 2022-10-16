using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class TipoCliente
    {
        public TipoCliente()
        {
            Clientes = new HashSet<Cliente>();
        }

        public int IdTipoCliente { get; set; }
        public string TipoCliente1 { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
