using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fixes.Data;
using fixes.Models;

namespace fixes.Controllers
{
    public class DirrecionessController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DirrecionessController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dirrecioness
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Direcciones.Include(d => d.Usuario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Dirrecioness/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.DireccionId == id);
            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion);
        }

        // GET: Dirrecioness/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsarioId", "Ciudad");
            return View();
        }

        // POST: Dirrecioness/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DireccionId,Address,Ciudad,Estado,CodigoPostal,UsuarioId")] Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(direccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsarioId", "Ciudad", direccion.UsuarioId);
            return View(direccion);
        }

        // GET: Dirrecioness/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones.FindAsync(id);
            if (direccion == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsarioId", "Ciudad", direccion.UsuarioId);
            return View(direccion);
        }

        // POST: Dirrecioness/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DireccionId,Address,Ciudad,Estado,CodigoPostal,UsuarioId")] Direccion direccion)
        {
            if (id != direccion.DireccionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(direccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DireccionExists(direccion.DireccionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsarioId", "Ciudad", direccion.UsuarioId);
            return View(direccion);
        }

        // GET: Dirrecioness/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.DireccionId == id);
            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion);
        }

        // POST: Dirrecioness/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var direccion = await _context.Direcciones.FindAsync(id);
            if (direccion != null)
            {
                _context.Direcciones.Remove(direccion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DireccionExists(int id)
        {
            return _context.Direcciones.Any(e => e.DireccionId == id);
        }
    }
}
