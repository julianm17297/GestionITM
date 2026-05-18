using GestionITM.Domain.Dtos;
using GestionITM.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace GestionITM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Solo usuarios con Token válido entran aquí
    public class MatriculasController : ControllerBase
    {
        private readonly IMatriculaService _matriculaService;

        public MatriculasController(IMatriculaService matriculaService)
        {
            _matriculaService = matriculaService;
        }

        /// <summary>
        /// Endpoint para que un estudiante se matricule en un curso.
        /// El ID del estudiante se extrae automáticamente del Token JWT.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Estudiante")] // Solo el rol Estudiante puede matricularse
        public async Task<IActionResult> Post([FromBody] MatriculaCreateDto dto)
        {
            // EXTRAER EL ID DEL ESTUDIANTE DEL TOKEN
            // Buscamos el claim "NameIdentifier" o el que hayan configurado al crear el token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("No se pudo identificar al estudiante desde el token.");

            int estudianteId = int.Parse(userIdClaim.Value);

            try
            {
                // Llamamos al servicio (La Regla del Chef ocurre adentro)
                var resultado = await _matriculaService.MatricularEstudianteAsync(estudianteId, dto);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                // Si el servicio lanza el error de "Sin cupos" o "Ya matriculado", 
                // devolvemos un 400 Bad Request con el mensaje de error.
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}