using System;

namespace Web.Models
{
    public class DocumentosClientesViewModel
    {
        public int IdDocumento { get; set; }
        public string NombreDocumento { get; set; }
        public string UrlDocumento { get; set; }
        public int IdTipoDocumento { get; set; }
        public bool Borrado { get; set; }
        public int IdClienteDocumento { get; set; }
        public string NombreTipoDocumento { get; set; }
        public DateTime FechaSubida { get; set; }
    }
}
