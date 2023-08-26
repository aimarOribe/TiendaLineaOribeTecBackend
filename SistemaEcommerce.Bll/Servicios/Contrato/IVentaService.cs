using SistemaEcommerce.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Bll.Servicios.Contrato
{
    public interface IVentaService
    {
        Task<VentaDto> Registrar(VentaDto ventaDTO);
        Task<List<VentaDto>> Historial(string fechaInicio, string fechaFin, string? idTransaccion);
        Task<List<ReporteDto>> Reporte(string fechaInicio, string fechaFin);
        Task<UbicacionDto> Ubicacion(string idDistrito);
        Task<List<VentaDto>> HistorialCliente(string fechaInicio, string fechaFin, int idCliente, string? idTransaccion);
    }
}
