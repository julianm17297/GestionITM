using Microsoft.AspNetCore.Mvc;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

[Route("api/[controller]")]
[ApiController]
public class CursosController : ControllerBase
{
    public readonly ICursoRepository _repository;

    public CursosController(ICursoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
    {
        var cursos = await _repository.ObtenerTodoAsync();
        return Ok(cursos);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Curso>> GetCurso(int id)
    {
        var curso = await _repository.ObtenerPorIdAsync(id);

        if (curso == null)
        {
            return NotFound();
        }

        return Ok(curso);
    }



    [HttpPost]
    public async Task<ActionResult<Curso>> PostCurso(Curso curso)
    {
        await _repository.AgregarAsync(curso);
        return CreatedAtAction(nameof(GetCursos), new { id = curso.Id }, curso);
    }
}


