using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Dto
{
    public class VentaDto
    {
        public int IdVenta { get; set; }

        public int IdCliente { get; set; }

        public string? DescripcionCliente { get; set; }

        public int TotalProducto { get; set; }

        public string? MontoTotalTexto { get; set; }

        public string? Contacto { get; set; }

        public string? IdDistrito { get; set; }

        public string? Telefono { get; set; }

        public string? Direccion { get; set; }

        public string? IdTransaccion { get; set; }

        public string? FechaVenta { get; set; }

        public virtual ICollection<DetalleVentaDto> DetalleVenta { get; set; }
    }
}
