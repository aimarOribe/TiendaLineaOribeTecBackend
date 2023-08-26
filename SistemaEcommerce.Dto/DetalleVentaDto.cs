using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Dto
{
    public class DetalleVentaDto
    {
        public int? IdProducto { get; set; }

        public string? RutaImagen { get; set; }

        public string? DescripcionProducto { get; set; }

        public int? Cantidad { get; set; }

        public string? PrecioTexto { get; set; }

        public string? TotalTexto { get; set; }
    }
}
