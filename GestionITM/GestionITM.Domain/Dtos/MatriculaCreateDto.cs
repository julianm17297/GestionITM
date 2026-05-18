namespace GestionITM.Domain.Dtos

{
    public class MatriculaCreateDto
    {
        // El ID del curso al que se quiere meter el estudiante
        public int CursoId { get; set; }

        // El periodo actual (pueden dejarlo por defecto)
        public string Periodo { get; set; } = "2026-1";
    }
}