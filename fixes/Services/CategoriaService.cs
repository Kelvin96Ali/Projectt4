using fixes.Data;
using fixes.Models;
using Ecommerce.Services;
using Microsoft.EntityFrameworkCore; // Asegúrate de tener este using

namespace fixes.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ApplicationDbContext _context;

        public CategoriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Categoria>> GetCategorias()
        {

            return await _context.Categorias.ToListAsync();
        }
    }
}
