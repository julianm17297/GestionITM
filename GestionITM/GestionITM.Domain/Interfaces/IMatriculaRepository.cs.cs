using GestionITM.Domain.Entities;
namespace GestionITM.Domain.Interfaces
{
    public interface IMatriculaRepository
    {
        Task<bool> ExisteMatriculaAsync(int estudianteId, int cursoId);
        Task AddAsync(Matricula entity); // Para poder guardar la matrícula
    }
}