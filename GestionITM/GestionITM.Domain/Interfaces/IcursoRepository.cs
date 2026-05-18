
using GestionITM.Domain.Entities;
using GestionITM.Domain.Models;
namespace GestionITM.Domain.Interfaces
{
    public interface ICursoRepository

    {

        Task<PagedResult<Curso>> GetPagedAsync(int pagina, int cantidad);
        Task<IEnumerable<Curso>> ObtenerTodoAsync();
        Task <Curso?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Curso curso);
        Task<Curso?> GetByIdAsync(int id);
        Task UpdateAsync(Curso curso);

    }
}
