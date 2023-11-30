using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fixes.Models
{
    public class Producto
    {
        [Key]
        public int ProductoId { get; set; }

        [Required]
        [StringLength(50)]
        public string Codigo { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string Modelo { get; set; } = null!;

        [Required]
        [StringLength(1000)]
        public string Descripcion { get; set; } = null!;

        [Required]
        public decimal Precio { get; set; }

        [StringLength(255)] // Ajustar según la longitud máxima de la ruta de la imagen
        public string Imagen { get; set; } = null!;

        [ForeignKey("CategoriaId")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;

        public int Stock { get; set; } // ¿Es realmente requerido?

        [Required]
        [StringLength(100)]
        public string Marca { get; set; } = null!;

        [Required]
        public bool Activo { get; set; }

        public ICollection<Detalle_Pedido> DetallesPedido { get; set; } = new List<Detalle_Pedido>();
    }
}
