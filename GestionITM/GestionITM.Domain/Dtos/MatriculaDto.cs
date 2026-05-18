namespace GestionITM.Domain.Dtos
{
  
    public class MatriculaDto
    {
        public int Id { get; set; }
        public string EstudianteNombre { get; set; } = string.Empty;
        public string CursoNombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }
}