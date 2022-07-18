using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class EstadosTicket
    {
        public EstadosTicket()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int IdEstadoTicket { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
