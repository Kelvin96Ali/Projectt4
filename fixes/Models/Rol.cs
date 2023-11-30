using System.ComponentModel.DataAnnotations;

namespace fixes.Models
{
    public class Rol{
        [Key]
        public int RolId{ get ; set ;}
        [Required(ErrorMessage ="El campo nombre es obligatorio")]
        [StringLength(50)]
        public string Nombre { get ; set;}=null!;

    }

}