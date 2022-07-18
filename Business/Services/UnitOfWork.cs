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
        public IGenericRepository<Cliente> _clienterepository;
        public IGenericRepository<AspNetUser> _Usuarios;

        public UnitOfWork(AsesoriaContext context)
        {
            this._context = context;
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

        public IGenericRepository<Cliente> ClienteRepository
        {
            get
            {
                if (this._clienterepository == null)
                    this._clienterepository = new GenericRepository<Cliente>(_context);

                return _clienterepository;
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
