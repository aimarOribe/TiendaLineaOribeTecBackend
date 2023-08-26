using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SistemaEcommerce.Dto
{
    public class ProductoDto
    {
        public int IdProducto { get; set; }

        public string? Nombre { get; set; }

        public string? Decripcion { get; set; }

        public int? IdMarca { get; set; }

        public string? DescripcionMarca { get; set; }

        public int? IdCategoria { get; set; }

        public string? DescripcionCategoria { get; set; }

        public string? Precio { get; set; }

        public string? Stock { get; set; }

        public string? RutaImagen { get; set; }

        public string? NombreImagen { get; set; }

        public int? Activo { get; set; }
    }
}
