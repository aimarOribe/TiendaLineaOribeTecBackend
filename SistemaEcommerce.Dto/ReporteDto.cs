using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Dto
{
    public class ReporteDto
    {
        public string? FechaVenta { get; set; }
        public string? IdTransaccion { get; set; }
        public string? TotalVenta { get; set; }
        public string? Producto { get; set; }
        public string? Cantidad { get; set; }
        public string? Precio { get; set; }
        public string? Total { get; set; }
    }
}
