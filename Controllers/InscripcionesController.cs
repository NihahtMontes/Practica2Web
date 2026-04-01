using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            ViewBag.EstudianteId = new SelectList(_context.Estudiantes, "Id", "Nombre", estudianteId);
            ViewBag.CursoId = new SelectList(_context.Cursos, "Id", "Nombre");

            return View();
        }

        // POST: Inscripciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inscripcion inscripcion)
        {
            // 🔥 LIMPIAR TODO (evita errores automáticos de ASP.NET)
            ModelState.Clear();

            // =========================
            // ✅ VALIDACIONES MANUALES
            // =========================

            // Estudiante
            if (inscripcion.EstudianteId <= 0)
            {
                ModelState.AddModelError("EstudianteId", "Debes seleccionar un estudiante");
            }

            // Curso
            if (inscripcion.CursoId <= 0)
            {
                ModelState.AddModelError("CursoId", "Debes seleccionar un curso");
            }

            // Fecha
            if (inscripcion.FechaInscripcion == default)
            {
                ModelState.AddModelError("FechaInscripcion", "La fecha es obligatoria");
            }
            else if (inscripcion.FechaInscripcion > DateTime.Now)
            {
                ModelState.AddModelError("FechaInscripcion", "La fecha no puede ser futura");
            }

            // Estado
            if (string.IsNullOrWhiteSpace(inscripcion.Estado))
            {
                ModelState.AddModelError("Estado", "Debes seleccionar un estado");
            }
            else
            {
                var estadosValidos = new[] { "Activo", "Completado", "Pendiente" };

                if (!estadosValidos.Contains(inscripcion.Estado))
                {
                    ModelState.AddModelError("Estado", "Estado inválido");
                }
            }

            // Duplicados
            if (_context.Inscripciones.Any(i =>
                i.EstudianteId == inscripcion.EstudianteId &&
                i.CursoId == inscripcion.CursoId))
            {
                ModelState.AddModelError("", "Este estudiante ya está inscrito en ese curso");
            }

            // =========================

            if (ModelState.IsValid)
            {
                _context.Add(inscripcion);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Estudiantes", new { id = inscripcion.EstudianteId });
            }

            // 🔁 Recargar combos si falla
            ViewBag.EstudianteId = new SelectList(_context.Estudiantes, "Id", "Nombre", inscripcion.EstudianteId);
            ViewBag.CursoId = new SelectList(_context.Cursos, "Id", "Nombre", inscripcion.CursoId);

            return View(inscripcion);
        }
    }
}