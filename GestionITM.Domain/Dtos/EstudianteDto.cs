using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionITM.Domain.Dtos
{
    internal class EstudianteDto

    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string correo { get; set; } = string.Empty;
    }
}
