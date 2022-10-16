using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class Agente
    {
        public Agente()
        {
            Clientes = new HashSet<Cliente>();
        }

        public int IdAgente { get; set; }
        public string Agente1 { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
