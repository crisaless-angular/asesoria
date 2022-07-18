using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class TipoIdentificacionFiscal
    {
        public TipoIdentificacionFiscal()
        {
            Clientes = new HashSet<Cliente>();
        }

        public int IdTipoIdentificacionFiscal { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
