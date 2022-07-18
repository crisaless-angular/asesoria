using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class ClienteEmail
    {
        public int IdEmailCliente { get; set; }
        public int? IdCliente { get; set; }
        public string Email { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
    }
}
