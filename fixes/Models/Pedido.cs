using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fixes.Models
{
    public class Pedido
    {
        [Key]
        public int PedidoId { get; set; }

        [Required]
        public int UsuarioId { get; set; } // Corregido el nombre de la propiedad

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } = null!;

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string Estado { get; set; } = null!;

        public int DireccionIdSeleccionada { get; set; }

        public Direccion Direccion { get; set; } = null!;

        public decimal Total { get; set; }

        public ICollection<Detalle_Pedido> DetallesPedido { get; set; } = null!;
    }
}
