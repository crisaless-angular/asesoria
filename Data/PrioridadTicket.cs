using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class PrioridadTicket
    {
        public PrioridadTicket()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int IdPrioridad { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
