namespace fixes.Models.ViewModels
{
    public class ProductosPaginadosViewModel
    {
        public List<Producto> Productos { get; set; } = new List<Producto>(); // Corregido para inicializar la lista de productos

        public int PaginaActual { get; set; }

        public int TotalPaginas { get; set; }

        public int? CategoriaIdSeleccionada { get; set; }

        public string? Busqueda { get; set; }

        public bool MostrarMensajeSinResultados { get; set; } // Corregido paréntesis de cierre

        public string? NombreCategoriaSeleccionada { get; set; }
    }
}
