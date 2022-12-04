using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class Documento
    {
        public int IdDocumento { get; set; }
        public string NombreDocumento { get; set; }
        public string UrlDocumento { get; set; }
        public int? IdTipoDocumento { get; set; }
        public bool? Borrado { get; set; }
        public int? IdCliente { get; set; }
        public DateTime? FechaSubida { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual TipoDocumento IdTipoDocumentoNavigation { get; set; }
    }
}
