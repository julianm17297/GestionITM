using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using GestionITM.Domain.Models;


namespace GestionITM.Infrastructure.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly ApplicationDbContext _context;

        public CursoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Curso>> ObtenerTodoAsync()
        {
            return await _context.Cursos.ToListAsync();
        }

        public async Task AgregarAsync(Curso curso)
        {
            await _context.Cursos.AddAsync(curso);
            await _context.SaveChangesAsync();
        }
        public async Task<Curso?> ObtenerPorIdAsync(int id)
        {
            return await _context.Cursos.FindAsync(id);
     }
        public async Task<Curso?> GetByIdAsync(int id)
        {
            return await _context.Cursos.FindAsync(id);
        }

        public async Task UpdateAsync(Curso curso)
        {
            _context.Cursos.Update(curso);
            await _context.SaveChangesAsync();
    
        }

        public async Task<PagedResult<Curso>> GetPagedAsync(int pagina, int cantidad)
        {
            var query = _context.Cursos.AsQueryable();

            // 1. Contamos el total antes de cortar la lista
            var totalRegistros = await query.CountAsync();

            // 2. Calculamos el total de páginas
            var totalPaginas = (int)Math.Ceiling((double)totalRegistros / cantidad);

            // 3. Traemos solo los registros de la página actual
            var items = await query
                .Skip((pagina - 1) * cantidad)
                .Take(cantidad)
                .ToListAsync();

            // 4. Armamos la respuesta con los nombres del profe
            return new PagedResult<Curso>
            {
                Items = items,
                PaginaActual = pagina,
                TotalPaginas = totalPaginas,
                TotalRegistros = totalRegistros,
                RegistrosPorPagina = cantidad
            };
        }


    }


}
