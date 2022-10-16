using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class ClienteMail
    {
        public int IdCliente { get; set; }
        public int IdMail { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Email IdMailNavigation { get; set; }
    }
}
