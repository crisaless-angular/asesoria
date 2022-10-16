using NPOI.SS.UserModel;

namespace Web.Business.Interfaces
{
    public interface ILeerExcel
    {
        void Leerexcel(IWorkbook MiExcel);
    }
}