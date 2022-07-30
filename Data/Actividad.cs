using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class Actividad
    {
        public Actividad()
        {
            Clientes = new HashSet<Cliente>();
        }

        public int IdActividad { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
