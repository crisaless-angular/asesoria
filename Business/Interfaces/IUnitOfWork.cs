using Web.Data;
using System;
using BA002.Web.Models;

namespace Web.Business.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Cliente> ClienteRepository { get; }
        IGenericRepository<AspNetUser> UsuariosRepository { get; }
        void Save();

    }
}