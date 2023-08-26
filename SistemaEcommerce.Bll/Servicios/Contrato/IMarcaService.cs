using SistemaEcommerce.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Bll.Servicios.Contrato
{
    public interface IMarcaService
    {
        Task<List<MarcaDto>> Lista();
        Task<List<MarcaDto>> ListarMarcasPorCategoria(int idCategoria);
        Task<MarcaDto> Crear(MarcaDto marcaDto);
        Task<bool> Editar(MarcaDto marcaDto);
        Task<bool> Eliminar(int id);
    }
}
