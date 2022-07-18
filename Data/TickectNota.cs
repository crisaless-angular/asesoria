using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class TickectNota
    {
        public TickectNota()
        {
            NotaArchivos = new HashSet<NotaArchivo>();
        }

        public int IdNota { get; set; }
        public string Usuario { get; set; }
        public DateTime? Fecha { get; set; }
        public string Descripcion { get; set; }
        public int? IdTicket { get; set; }

        public virtual Ticket IdTicketNavigation { get; set; }
        public virtual AspNetUser UsuarioNavigation { get; set; }
        public virtual ICollection<NotaArchivo> NotaArchivos { get; set; }
    }
}
