using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class CategoriaTicket
    {
        public CategoriaTicket()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int IdCategoriaTicket { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
