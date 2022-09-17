using BA002.Web.Models;
using System;
using Web.Business.Genericrepository;
using Web.Business.Interfaces;
using Web.Data;

namespace Web.Business.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AsesoriaContext _context;
        public IGenericRepository<AspNetUser> _Usuarios;
        public IGenericRepository<Empresa> _Empresa;
        public IGenericRepository<Cliente> _Cliente;
        public IGenericRepository<ClienteEmail> _ClienteEmail;
        public IGenericRepository<AspNetUserToken> _UserToken;
        public IGenericRepository<Agente> _Agente;
        public IGenericRepository<TipoCliente> _TipoCliente;
        public IGenericRepository<Auditorium> _Auditoria;
        public IGenericRepository<Paise> _Paises;
        public IGenericRepository<TipoIdentificacionFiscal> _TipoIdentificacionFiscal;
        public IGenericRepository<FormasPago> _FormasPago;
        public IGenericRepository<Actividad> _Actividad;
        public IGenericRepository<ClienteCuenta> _ClienteCuenta;
        public IGenericRepository<Configuracione> _Configuracion;

        public UnitOfWork(AsesoriaContext context)
        {
            this._context = context;
        }

        public IGenericRepository<Configuracione> ConfiguracionRepository
        {
            get
            {
                if (this._Configuracion == null)
                    this._Configuracion = new GenericRepository<Configuracione>(_context);

                return _Configuracion;
            }

        }

        public IGenericRepository<ClienteCuenta> ClienteCuentaRepository
        {
            get
            {
                if (this._ClienteCuenta == null)
                    this._ClienteCuenta = new GenericRepository<ClienteCuenta>(_context);

                return _ClienteCuenta;
            }

        }

        public IGenericRepository<Actividad> ActividadRepository
        {
            get
            {
                if (this._Actividad == null)
                    this._Actividad = new GenericRepository<Actividad>(_context);

                return _Actividad;
            }

        }

        public IGenericRepository<FormasPago> FormasPagoRepository
        {
            get
            {
                if (this._FormasPago == null)
                    this._FormasPago = new GenericRepository<FormasPago>(_context);

                return _FormasPago;
            }

        }

        public IGenericRepository<TipoIdentificacionFiscal> TipoIdentificacionFiscalRepository
        {
            get
            {
                if (this._TipoIdentificacionFiscal == null)
                    this._TipoIdentificacionFiscal = new GenericRepository<TipoIdentificacionFiscal>(_context);

                return _TipoIdentificacionFiscal;
            }

        }

        public IGenericRepository<Paise> PaisesRepository
        {
            get
            {
                if (this._Paises == null)
                    this._Paises = new GenericRepository<Paise>(_context);

                return _Paises;
            }

        }

        public IGenericRepository<Auditorium> AuditoriaRepository
        {
            get
            {
                if (this._Auditoria == null)
                    this._Auditoria = new GenericRepository<Auditorium>(_context);

                return _Auditoria;
            }

        }

        public IGenericRepository<TipoCliente> TipoClienteRepository
        {
            get
            {
                if (this._TipoCliente == null)
                    this._TipoCliente = new GenericRepository<TipoCliente>(_context);

                return _TipoCliente;
            }

        }

        public IGenericRepository<Agente> AgenteRepository
        {
            get
            {
                if (this._Agente == null)
                    this._Agente = new GenericRepository<Agente>(_context);

                return _Agente;
            }

        }

        public IGenericRepository<AspNetUserToken> UserTokenRepository
        {
            get
            {
                if (this._UserToken == null)
                    this._UserToken = new GenericRepository<AspNetUserToken>(_context);

                return _UserToken;
            }

        }

        public IGenericRepository<ClienteEmail> ClienteEmailRepository
        {
            get
            {
                if (this._ClienteEmail == null)
                    this._ClienteEmail = new GenericRepository<ClienteEmail>(_context);

                return _ClienteEmail;
            }

        }

        public IGenericRepository<AspNetUser> UsuariosRepository
        {
            get
            {
                if (this._Usuarios == null)
                    this._Usuarios = new GenericRepository<AspNetUser>(_context);

                return _Usuarios;
            }

        }

        public IGenericRepository<Empresa> EmpresaRepository
        {
            get
            {
                if (this._Empresa == null)
                    this._Empresa = new GenericRepository<Empresa>(_context);

                return _Empresa;
            }

        }

        public IGenericRepository<Cliente> ClienteRepository
        {
            get
            {
                if (this._Cliente == null)
                    this._Cliente = new GenericRepository<Cliente>(_context);

                return _Cliente;
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
