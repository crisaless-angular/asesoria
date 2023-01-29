using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BA002.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NToastNotify;
using Web.Business;
using Web.Business.Interfaces;
using Web.Data;

namespace BA002.Web.Views
{
    [Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        private readonly AsesoriaContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private IConfigurationRoot _ConfigRoot;
        private readonly IToastNotification _toastNotification;
        private IUnitOfWork _unitofwork;

        public UsuariosController(UserManager<IdentityUser> userManager, IConfiguration configRoot, IToastNotification toastNotification, IUnitOfWork unitofwork,
            AsesoriaContext context)
        {
            _context = context;
            _userManager = userManager;
            this._ConfigRoot = (IConfigurationRoot)configRoot;
            this._toastNotification = toastNotification;
            this._unitofwork = unitofwork;
        }

        public IActionResult Index(string mensaje = null)
        {
            ViewData["Title"] = "Listado de usuarios";

            if (mensaje != null)
                _toastNotification.AddSuccessToastMessage(mensaje);
            return View();
        }

        public IActionResult Crear()
        {
            ViewBag.IdRol = new SelectList(_context.AspNetRoles, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([Bind("UserName,Nombre,Password,IdRol")] Usuarios model)
        {
            ViewBag.IdRol = new SelectList(_context.AspNetRoles, "Id", "Name");
            if (String.IsNullOrEmpty(model.Nombre))
            {
                ModelState.AddModelError(string.Empty, "El nombre no puede estar vacío.");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.UserName))
            {
                ModelState.AddModelError(string.Empty, "El usuario no puede estar vacío.");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.IdRol))
            {
                ModelState.AddModelError(string.Empty, "El rol no puede estar vacío.");
                return View(model);
            }
            else
            {
                var resultado = _context.AspNetUsers.Where(x => x.UserName == model.UserName && x.Id != model.Id);
                if (resultado.Count() > 0)
                {
                    ModelState.AddModelError(string.Empty, "Usuario ya existente.");
                    return View(model);
                }
            }

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName, Email = model.UserName };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    AspNetUser AspNetUsers = _context.AspNetUsers.Find(user.Id);
                    AspNetUsers.Nombre = model.Nombre;
                    _context.Entry(AspNetUsers).State = EntityState.Modified;
                    _context.SaveChanges();

                    AspNetUserRole role = new AspNetUserRole
                    {
                        UserId = AspNetUsers.Id,
                        RoleId = model.IdRol
                    };
                    _context.AspNetUserRoles.Add(role);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Editar(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var AspNetUsers = await _context.AspNetUsers.FindAsync(id);
            if (AspNetUsers == null)
            {
                return NotFound();
            }

            Usuarios usuario = new Usuarios();
            usuario.Id = AspNetUsers.Id;
            usuario.UserName = AspNetUsers.UserName;
            usuario.Nombre = AspNetUsers.Nombre;
            usuario.IdRol = _context.AspNetUserRoles.Where(x => x.UserId == usuario.Id).SingleOrDefault().RoleId;

            ViewBag.IdRol = new SelectList(_context.AspNetRoles, "Id", "Name", usuario.IdRol);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(string id, [Bind("Id,UserName,Nombre,IdRol")] Usuarios model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (String.IsNullOrEmpty(model.Nombre))
            {
                ModelState.AddModelError(string.Empty, "El nombre no puede estar vacío.");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.UserName))
            {
                ModelState.AddModelError(string.Empty, "El usuario no puede estar vacío.");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.IdRol))
            {
                ModelState.AddModelError(string.Empty, "El rol no puede estar vacío.");
                return View(model);
            }
            else
            {
                var resultado = _context.AspNetUsers.Where(x => x.UserName == model.UserName && x.Id != model.Id);
                if (resultado.Count() > 0)
                {
                    ModelState.AddModelError(string.Empty, "Usuario ya existente.");
                    return View(model);
                }
            }

            try
            {
                AspNetUser usuario = _context.AspNetUsers.Find(model.Id);

                usuario.UserName = model.UserName;
                usuario.Email = model.UserName;
                usuario.NormalizedEmail = _userManager.NormalizeEmail(model.UserName);
                usuario.NormalizedUserName = _userManager.NormalizeName(model.UserName);
                usuario.Nombre = model.Nombre;

                _context.Update(usuario);
                await _context.SaveChangesAsync();

                string oldRole = _context.AspNetUserRoles.Where(x => x.UserId == usuario.Id).SingleOrDefault().RoleId;
                if (model.Rol != oldRole)
                {
                    string newRolName = _context.AspNetRoles.Where(x => x.Id == model.IdRol).SingleOrDefault().NormalizedName;
                    string oldRolName = _context.AspNetRoles.Where(x => x.Id == oldRole).SingleOrDefault().NormalizedName;

                    IdentityUser usuariocambio = await _userManager.FindByIdAsync(model.Id);

                    var deleteRol = await _userManager.RemoveFromRoleAsync(usuariocambio, oldRolName);
                    if (deleteRol.Succeeded)
                    {
                        var addRol = await _userManager.AddToRoleAsync(usuariocambio, newRolName);
                        if (addRol.Succeeded)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ModelState.AddModelError(string.Empty, "Rellene correctamente los campos.");
            }

            ViewBag.IdRol = new SelectList(_context.AspNetRoles, "Id", "Name", model.IdRol);
            return View(model);
        }

        public async Task<IActionResult> Eliminar(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var AspNetUsers = await _context.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (AspNetUsers == null)
            {
                return NotFound();
            }

            return View(AspNetUsers);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var AspNetUsers = await _context.AspNetUsers.FindAsync(id);




            _context.AspNetUsers.Remove(AspNetUsers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetUsersExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }

        public List<Usuarios> QueryObtenerUsuariosIndex()
        {
            List<Usuarios> usuarios = new List<Usuarios>();
            foreach (AspNetUser usuario in _unitofwork.UsuariosRepository.GetAll())
            {

                Usuarios Usuario = new Usuarios()
                {
                    Id = usuario.Id,
                    UserName = usuario.UserName,
                    Email = usuario.Email,
                    Nombre = usuario.Nombre
                };

                usuarios.Add(Usuario);

            }
            return usuarios;
        }

    }
}
