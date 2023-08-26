using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Dto
{
    public class MarcaDto
    {
        public int IdMarca { get; set; }

        public string? Descripcion { get; set; }

        public int? Activo { get; set; }
    }
}
