using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class TipoDocumento
    {
        public TipoDocumento()
        {
            Documentos = new HashSet<Documento>();
        }

        public int IdTipoDocumento { get; set; }
        public string NombreTipoDocumento { get; set; }

        public virtual ICollection<Documento> Documentos { get; set; }
    }
}
