using Web.Data;

namespace Web.Business.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Empresa> EmpresaRepository { get; }
        IGenericRepository<AspNetUser> UsuariosRepository { get; }
        IGenericRepository<Cliente> ClienteRepository { get; }
        IGenericRepository<ClienteEmail> ClienteEmailRepository { get; }
        IGenericRepository<AspNetUserToken> UserTokenRepository { get; }
        IGenericRepository<Agente> AgenteRepository { get; }
        IGenericRepository<TipoCliente> TipoClienteRepository { get; }
        IGenericRepository<Auditorium> AuditoriaRepository { get; }
        IGenericRepository<Paise> PaisesRepository { get; }
        IGenericRepository<TipoIdentificacionFiscal> TipoIdentificacionFiscalRepository { get; }
        IGenericRepository<FormasPago> FormasPagoRepository { get; }
        IGenericRepository<Actividad> ActividadRepository { get; }
        IGenericRepository<ClienteCuenta> ClienteCuentaRepository { get; }
        IGenericRepository<Configuracione> ConfiguracionRepository { get; }
        void Save();

    }
}