using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using GestionITM.Infrastructure.Repositories;
using GestionITM.Infrastructure.Services;

namespace GestionITM.Infrastructure.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IMatriculaRepository _matriculaRepo;
        private readonly ICursoRepository _cursoRepo;
        private readonly IEstudianteRepository _estudianteRepo;

        public MatriculaService(IMatriculaRepository matriculaRepo, ICursoRepository cursoRepo, IEstudianteRepository estudianteRepo)
        {
            _matriculaRepo = matriculaRepo;
            _cursoRepo = cursoRepo;
            _estudianteRepo = estudianteRepo;
        }

        public async Task<MatriculaDto> MatricularEstudianteAsync(int estudianteId, MatriculaCreateDto dto)
        {
            var curso = await _cursoRepo.GetByIdAsync(dto.CursoId);

            // REGLA DEL CHEF: Validar cupos
            if (curso == null || curso.Cupos <= 0)
            {
                // Esta excepción la atrapará su Middleware y la mandará a Serilog
                throw new Exception("Lo sentimos, no hay cupos disponibles para este curso.");
            }

            // Validar si ya está matriculado (opcional pero recomendado)
            var existe = await _matriculaRepo.ExisteMatriculaAsync(estudianteId, dto.CursoId);
            if (existe) throw new Exception("Ya te encuentras matriculado en este curso.");

            // Crear matrícula
            var matricula = new Matricula
            {
                EstudianteId = estudianteId,
                CursoId = dto.CursoId,
                Periodo = dto.Periodo,
                Estado = "Activa"
            };

            // Restar cupo
            curso.Cupos--;
            await _cursoRepo.UpdateAsync(curso);
            await _matriculaRepo.AddAsync(matricula);

            return new MatriculaDto { Id = matricula.Id, Fecha = matricula.FechaMatricula };
        }
    }
}