using Microsoft.AspNetCore.Mvc;
using Web.Business;
using Web.Business.Interfaces;

namespace Web.Controllers
{
    public class Public : Controller
    {
        private IUnitOfWork _unitofwork;

        public Public(IUnitOfWork unitofwork)
        {
            this._unitofwork = unitofwork;
        }
        public string GetLogotipo()
        {
            return "";
            //return _unitofwork.EmpresaRepository.GetEntity(variables.IdEmpresa).Logotipo;
        }
    }
}
