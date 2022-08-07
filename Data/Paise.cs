using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class Paise
    {
        public Paise()
        {
            Clientes = new HashSet<Cliente>();
        }

        public int IdPais { get; set; }
        public string Descripcion { get; set; }
        public string Claim { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
