using Microsoft.Extensions.Configuration;
using Web.Business.Interfaces;
using Web.Data;
using Web.Models;

namespace Web.Business.Services
{
    public class AuditoriaLog : IAuditoria
    {
        private readonly IUnitOfWork _unitOfWork;
        private IConfigurationRoot _ConfigRoot;

        public AuditoriaLog(IUnitOfWork unitOfWork, IConfiguration configRoot)
        {
            this._unitOfWork = unitOfWork;
            this._ConfigRoot = (IConfigurationRoot)configRoot;
        }

        public bool GuardarAuditoria(AuditoriaModel model)
        {

            string Guardar = _ConfigRoot.GetSection("Auditoria:Auditar").Value.ToString();

            if (Guardar.ToLower() == "true")
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

            return false;

        }

    }
}
