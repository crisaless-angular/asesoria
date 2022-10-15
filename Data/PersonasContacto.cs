using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class PersonasContacto
    {
        public PersonasContacto()
        {
            Clientes = new HashSet<Cliente>();
        }

        public int IdPersonaContacto { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
