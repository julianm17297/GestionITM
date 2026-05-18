using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionITM.Domain.Entities
{
    public class Matricula
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EstudianteId { get; set; }

        [Required]
        public int CursoId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Periodo { get; set; } = string.Empty; // Ej: 2026-1

        [MaxLength(20)]
        public string Estado { get; set; } = "Activa";

        public DateTime FechaMatricula { get; set; } = DateTime.Now;

        // Propiedades de navegación (Clave para que EF reconozca las relaciones)
        [ForeignKey("EstudianteId")]
        public virtual Estudiante Estudiante { get; set; } = null!;

        [ForeignKey("CursoId")]
        public virtual Curso Curso { get; set; } = null!;
    }
}