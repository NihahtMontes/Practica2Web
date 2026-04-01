using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using practica2Web.Data;
using practica2Web.Models;

namespace practica2Web.Controllers
{
    public class InscripcionesController : Controller
    {
        private readonly AppDbContext _context;

        public InscripcionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Inscripciones/Create
        public IActionResult Create(int? estudianteId)
        {
            ViewData["EstudianteId"] = new SelectList(_context.Estudiantes, "Id", "Nombre", estudianteId);
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nombre");
            return View();
        }

        // POST: Inscripciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EstudianteId,CursoId,FechaInscripcion,Estado")] Inscripcion inscripcion)
        {
            if (ModelState.IsValid)
            {
                // Check if already enrolled
                var exists = await _context.Inscripciones.AnyAsync(i => i.EstudianteId == inscripcion.EstudianteId && i.CursoId == inscripcion.CursoId);
                if (!exists)
                {
                    _context.Add(inscripcion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Estudiantes", new { id = inscripcion.EstudianteId });
                }
                ModelState.AddModelError("", "El estudiante ya está inscrito en este curso.");
            }
            ViewData["EstudianteId"] = new SelectList(_context.Estudiantes, "Id", "Nombre", inscripcion.EstudianteId);
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nombre", inscripcion.CursoId);
            return View(inscripcion);
        }

        // POST: Inscripciones/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int estudianteId, int cursoId)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(estudianteId, cursoId);
            if (inscripcion != null)
            {
                _context.Inscripciones.Remove(inscripcion);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Estudiantes", new { id = estudianteId });
        }
    }
}
