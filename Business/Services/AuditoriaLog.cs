using Web.Business.Interfaces;
using Web.Data;
using Web.Models;

namespace Web.Business.Services
{
    public class AuditoriaLog : IAuditoria
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuditoriaLog(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public bool GuardarAuditoria(AuditoriaModel model)
        {
            Auditorium Auditoria = new Auditorium()
            {
                Fecha = model.Fecha,
                Usuario = model.Usuario,
                Accion = model.Accion
            };
            
            _unitOfWork.AuditoriaRepository.Add(Auditoria);
            _unitOfWork.Save();
            
            return true;
        }
        
    }
}
