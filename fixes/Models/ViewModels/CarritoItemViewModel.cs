namespace fixes.Models.ViewModels // Corrección del espacio de nombres
{
    public class CarritoItemViewModel // Definición de la clase CarritoItemViewModel
    {
        internal int cantidad;

        public int ProductoId { get; set; } // Propiedad ProductoId

        public Producto Producto { get; set; } = null!; // Propiedad Producto con inicialización a null

        public string Nombre { get; set; } = null!; // Propiedad Nombre con inicialización a null

        public decimal Precio { get; set; } // Propiedad Precio

        public int Cantidad { get; set; } // Propiedad Cantidad

        public decimal Subtotal => Precio * Cantidad; // Propiedad de solo lectura Subtotal calculada
    }
}
