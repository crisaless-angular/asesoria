using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class NotaArchivo
    {
        public int IdNotaArchivos { get; set; }
        public string RutaArchivo { get; set; }
        public int? IdNota { get; set; }

        public virtual TickectNota IdNotaNavigation { get; set; }
    }
}
