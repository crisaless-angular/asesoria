using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class FormasPago
    {
        public FormasPago()
        {
            Clientes = new HashSet<Cliente>();
        }

        public int IdFomaPago { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
