using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class ClienteCuenta
    {
        public int IdCuenta { get; set; }
        public string Ccc { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string Banco { get; set; }
        public int? IdCliente { get; set; }
        public int? IdMandato { get; set; }
        public bool? Activa { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Mandato IdMandatoNavigation { get; set; }
    }
}
