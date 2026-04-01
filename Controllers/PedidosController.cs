using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using practica2Web.Data;
using practica2Web.Models;

namespace practica2Web.Controllers
{
    public class PedidosController : Controller
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        // 🔥 ESTO TE FALTABA
        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .ToListAsync();

            return View(pedidos);
        }

        // GET: Pedidos/Create
        public IActionResult Create(int? clienteId)
        {
            ViewBag.ClienteId = new SelectList(_context.Clientes, "Id", "Nombre", clienteId);
            return View();
        }

        // POST: Pedidos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pedido pedido)
        {
            // 🔥 LIMPIAR VALIDACIONES AUTOMÁTICAS
            ModelState.Clear();

            // =========================
            // ✅ VALIDACIONES MANUALES
            // =========================

            if (pedido.ClienteId <= 0)
                ModelState.AddModelError("ClienteId", "Debes seleccionar un cliente");

            if (pedido.FechaPedido == default)
                ModelState.AddModelError("FechaPedido", "La fecha es obligatoria");
            else if (pedido.FechaPedido > DateTime.Now)
                ModelState.AddModelError("FechaPedido", "La fecha no puede ser futura");

            if (pedido.Total <= 0)
                ModelState.AddModelError("Total", "El total debe ser mayor a 0");

            // =========================

            if (ModelState.IsValid)
            {
                _context.Add(pedido);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Clientes", new { id = pedido.ClienteId });
            }

            ViewBag.ClienteId = new SelectList(_context.Clientes, "Id", "Nombre", pedido.ClienteId);
            return View(pedido);
        }
    }
}