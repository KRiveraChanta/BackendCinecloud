using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCine.Entities.Models
{
    [Table("GenerosXVideos")]
    public class GeneroXVideo : Control
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Genero")]
        public int IdGenero { get; set; }
        public Genero Genero { get; set; }

        [ForeignKey("Video")]
        public int IdVideo { get; set; }
        public Video Video { get; set; }
    }
}