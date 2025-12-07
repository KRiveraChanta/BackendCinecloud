using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.TestProject1
{
    [Table("Roles")]
    public class Rol
    {
        [Key]
        [Column("IdRol")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRol { get; set; }

        [Column("NombreRol")]
        public required string NombreRol { get; set; }
        
        // Relación inversa: un rol puede tener muchos usuarios
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    }
}
