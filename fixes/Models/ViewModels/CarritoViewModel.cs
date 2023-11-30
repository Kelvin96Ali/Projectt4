namespace fixes.Models.ViewModels // Corrección del espacio de nombres
{
    public class CarritoViewModel // Definición de la clase CarritoViewModel
    {
        public List<CarritoItemViewModel> Items { get; set; } = new List<CarritoItemViewModel>(); // Propiedad Items inicializada como una nueva lista de CarritoItemViewModel

        public decimal Total { get; set; } // Propiedad Total
    }
}
