using Web.Models;

namespace Web.Business.Interfaces
{
    public interface IAuditoria
    {
        bool GuardarAuditoria(AuditoriaModel model);
    }
}