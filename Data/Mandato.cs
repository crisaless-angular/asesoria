using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class Mandato
    {
        public Mandato()
        {
            ClienteCuenta = new HashSet<ClienteCuenta>();
        }

        public int IdMandato { get; set; }
        public string ReferenciaUnica { get; set; }
        public DateTime? FechaFirma { get; set; }
        public bool? Activo { get; set; }

        public virtual ICollection<ClienteCuenta> ClienteCuenta { get; set; }
    }
}
