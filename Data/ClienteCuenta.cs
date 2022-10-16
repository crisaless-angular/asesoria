using System;
using System.Collections.Generic;

#nullable disable

namespace Web.Data
{
    public partial class ClienteCuenta
    {
        public int IdCliente { get; set; }
        public int IdCuenta { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Cuenta IdCuentaNavigation { get; set; }
    }
}
