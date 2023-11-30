using fixes.Models;
// Asumiendo que el espacio de nombres es Ecommerce en lugar de Ecomerce

namespace Ecommerce.Services
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> GetCategorias(); // Faltaba cerrar los corchetes y añadir los corchetes angulares para especificar el tipo de retorno.
    }
}
