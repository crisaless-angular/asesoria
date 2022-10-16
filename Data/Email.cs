using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class Email
    {
        public Email()
        {
            ClienteMails = new HashSet<ClienteMail>();
        }

        public int IdEmailCliente { get; set; }
        public string Email1 { get; set; }
        public bool? Activo { get; set; }

        public virtual ICollection<ClienteMail> ClienteMails { get; set; }
    }
}
