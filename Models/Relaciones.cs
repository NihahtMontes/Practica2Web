using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace practica2Web.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; }
    }

    public class Pedido
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }
    }

    public class Estudiante
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Carrera { get; set; }

        public virtual ICollection<Inscripcion> Inscripciones { get; set; }
    }

    public class Curso
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public int Creditos { get; set; }

        public virtual ICollection<Inscripcion> Inscripciones { get; set; }
    }

    public class Inscripcion
    {
        public int EstudianteId { get; set; }
        public virtual Estudiante Estudiante { get; set; }

        public int CursoId { get; set; }
        public virtual Curso Curso { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        [Required]
        public string Estado { get; set; } // E.g., Activo, Completado, Pendiente
    }
}
