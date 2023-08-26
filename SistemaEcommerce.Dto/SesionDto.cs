using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Dto
{
    public class SesionDto
    {
        public int IdUsuario { get; set; }

        public string? Nombres { get; set; }

        public string? Apellidos { get; set; }

        public string? Correo { get; set; }

        public string? RolDescripcion { get; set; }

        public string? RutaImagen { get; set; }

        public int? Reestablecer { get; set; }
    }
}
