using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class Auditorium
    {
        public int IdAuditoria { get; set; }
        public DateTime? Fecha { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
    }
}
