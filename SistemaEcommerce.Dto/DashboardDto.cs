using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Dto
{
    public class DashboardDto
    {
        public int TotalVentas { get; set; }
        public string? TotalIngresos { get; set; }
        public int totalProductos { get; set; }
        public int totalClientes { get; set; }
        public List<VentaSemanaDto> VentasUltimaSemana { get; set; }
        public List<VentaSemanaProductosDto> VentasUltimaSemanaProductos { get; set; }
    }
}
