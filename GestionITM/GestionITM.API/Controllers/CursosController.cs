using Microsoft.AspNetCore.Mvc;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Models; // Para PagedResult

[Route("api/[controller]")]
[ApiController]
public class CursosController : ControllerBase
{
    public readonly ICursoRepository _repository;

    public CursosController(ICursoRepository repository)
    {
        _repository = repository;
    }

    // GET: api/Cursos?pagina=1&cantidad=10
    [HttpGet]
    public async Task<ActionResult<PagedResult<Curso>>> GetCursos([FromQuery] int pagina = 1, [FromQuery] int cantidad = 10)
    {
        // Llamamos al nuevo método paginado
        var resultado = await _repository.GetPagedAsync(pagina, cantidad);
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Curso>> GetCurso(int id)
    {
        var curso = await _repository.ObtenerPorIdAsync(id);
        if (curso == null) return NotFound();
        return Ok(curso);
    }

    [HttpPost]
    public async Task<ActionResult<Curso>> PostCurso(Curso curso)
    {
        await _repository.AgregarAsync(curso);
        return CreatedAtAction(nameof(GetCurso), new { id = curso.Id }, curso);
    }
}