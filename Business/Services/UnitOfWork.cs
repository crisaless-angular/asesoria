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

        public UnitOfWork(AsesoriaContext context)
        {
            this._context = context;
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
