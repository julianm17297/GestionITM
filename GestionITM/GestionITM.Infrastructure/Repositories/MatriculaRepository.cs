using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using GestionITM.Infrastructure;

namespace GestionITM.Infrastructure.Repositories
{
    // Quitamos la herencia de "Repository" porque no existe ese archivo base
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly ApplicationDbContext _context;

        public MatriculaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExisteMatriculaAsync(int estudianteId, int cursoId)
        {
            return await _context.Matriculas
                .AnyAsync(m => m.EstudianteId == estudianteId &&
                               m.CursoId == cursoId &&
                               m.Estado == "Activa");
        }

        // Si tu interfaz IRepository (que hereda IMatriculaRepository) 
        // pide métodos como Add, Update, etc., hay que implementarlos aquí:

        public async Task AddAsync(Matricula entity)
        {
            await _context.Matriculas.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Si te salen errores de "no implementa miembro de interfaz", 
        // dímelo para darte los métodos faltantes rápido.
    }
}