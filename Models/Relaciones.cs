using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace practica2Web.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$",
            ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }

    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.DateTime)]
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El total es obligatorio")]
        [Range(0.01, 1000000, ErrorMessage = "El total debe ser mayor a 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }
    }

    public class Estudiante
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$",
            ErrorMessage = "El nombre solo puede contener letras")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La carrera es obligatoria")]
        [StringLength(100)]
        public string Carrera { get; set; }

        public virtual ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }

    public class Curso
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del curso es obligatorio")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$",
            ErrorMessage = "El curso solo puede contener letras")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los créditos son obligatorios")]
        [Range(1, 10, ErrorMessage = "Los créditos deben estar entre 1 y 10")]
        public int Creditos { get; set; }

        public virtual ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }

    public class Inscripcion
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estudiante")]
        public int EstudianteId { get; set; }

        public virtual Estudiante Estudiante { get; set; } // ❌ SIN Required

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un curso")]
        public int CursoId { get; set; }

        public virtual Curso Curso { get; set; } // ❌ SIN Required

        [Required]
        public DateTime FechaInscripcion { get; set; }

        [Required]
        public string Estado { get; set; }
    }
}