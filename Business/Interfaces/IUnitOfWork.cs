using Web.Data;

namespace Web.Business.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Empresa> EmpresaRepository { get; }
        IGenericRepository<AspNetUser> UsuariosRepository { get; }
        IGenericRepository<Cliente> ClienteRepository { get; }
        void Save();

    }
}