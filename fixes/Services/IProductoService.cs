using System.Collections.Generic;
using System.Threading.Tasks;
using fixes.Models;
using fixes.Models.ViewModels;

namespace fixes.Services
{
    public interface IProductoService
    {
        Producto GetProducto(int id);
        Task<List<Producto>> GetProductosDestacados();
        Task<ProductosPaginadosViewModel> GetProductosPaginados(int? categoriaId, string? busqueda, int pagina, int productosPorPagina);
    }
}
