using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionITM.Domain.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new (); // La lista de elementos que se mostraran en la pagina actual
        public int PaginaActual { get; set; } // El numero de la pagina actual
        public int TotalPaginas { get; set; } // El numero total de paginas disponibles
        public int TotalRegistros { get; set; } // El numero total de registros en la base de datos (Sin paginar)
        public int RegistrosPorPagina { get; set; } // El numero de registros que se muestran por pagina 


    }
}
