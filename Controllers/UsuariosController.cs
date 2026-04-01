using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practica2Web.Data;
using practica2Web.Models;
using practica2Web.ViewModels;

namespace practica2Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            // Carga con .Include()
            var usuarios = await _context.Usuarios.Include(u => u.Perfil).ToListAsync();
            return View(usuarios);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioPerfilViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = new Usuario
                {
                    Nombre = model.Nombre,
                    Email = model.Email,
                    Perfil = new Perfil
                    {
                        Direccion = model.Direccion,
                        Telefono = model.Telefono,
                        FechaNacimiento = model.FechaNacimiento
                    }
                };
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.Include(u => u.Perfil).FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null) return NotFound();

            var model = new UsuarioPerfilViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Direccion = usuario.Perfil.Direccion,
                Telefono = usuario.Perfil.Telefono,
                FechaNacimiento = usuario.Perfil.FechaNacimiento
            };
            return View(model);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioPerfilViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = await _context.Usuarios.Include(u => u.Perfil).FirstOrDefaultAsync(u => u.Id == id);
                    if (usuario == null) return NotFound();

                    usuario.Nombre = model.Nombre;
                    usuario.Email = model.Email;
                    usuario.Perfil.Direccion = model.Direccion;
                    usuario.Perfil.Telefono = model.Telefono;
                    usuario.Perfil.FechaNacimiento = model.FechaNacimiento;

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Usuarios.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.Include(u => u.Perfil).FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.Include(u => u.Perfil).FirstOrDefaultAsync(u => u.Id == id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
