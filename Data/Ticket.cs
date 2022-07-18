using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class Ticket
    {
        public Ticket()
        {
            TickectNota = new HashSet<TickectNota>();
        }

        public int IdTicket { get; set; }
        public string IdSeguimiento { get; set; }
        public int? IdEstado { get; set; }
        public int? IdCliente { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdPrioridad { get; set; }
        public string AsignadoUsuario { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaActualizado { get; set; }
        public int? Respuestas { get; set; }
        public string UltimaRespuesta { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public byte[] TiempoDedicado { get; set; }
        public string Descripcion { get; set; }

        public virtual AspNetUser AsignadoUsuarioNavigation { get; set; }
        public virtual CategoriaTicket IdCategoriaNavigation { get; set; }
        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual EstadosTicket IdEstadoNavigation { get; set; }
        public virtual PrioridadTicket IdPrioridadNavigation { get; set; }
        public virtual ICollection<TickectNota> TickectNota { get; set; }
    }
}
