using Web.Data;

namespace Web.Business.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Empresa> EmpresaRepository { get; }
        IGenericRepository<AspNetUser> UsuariosRepository { get; }
        IGenericRepository<Cliente> ClienteRepository { get; }
        IGenericRepository<ClienteMail> ClienteEmailRepository { get; }
        IGenericRepository<AspNetUserToken> UserTokenRepository { get; }
        IGenericRepository<Agente> AgenteRepository { get; }
        IGenericRepository<TipoCliente> TipoClienteRepository { get; }
        IGenericRepository<Auditorium> AuditoriaRepository { get; }
        IGenericRepository<Paise> PaisesRepository { get; }
        IGenericRepository<TipoIdentificacionFiscal> TipoIdentificacionFiscalRepository { get; }
        IGenericRepository<FormasPago> FormasPagoRepository { get; }
        IGenericRepository<Actividad> ActividadRepository { get; }
        IGenericRepository<Cuenta> CuentaRepository { get; }
        IGenericRepository<Configuracione> ConfiguracionRepository { get; }
        IGenericRepository<Email> EmailRepository { get; }
        IGenericRepository<ClienteCuenta> ClienteCuentaRepository { get; }
        IGenericRepository<PersonasContacto> PersonaContactoRepository { get; }
        IGenericRepository<Documento> DocumentoRepository { get; }
        IGenericRepository<TipoDocumento> TipoDocumentoRepository { get; }

        void Save();

    }
}