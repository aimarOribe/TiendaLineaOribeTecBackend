using SistemaEcommerce.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Bll.Servicios.Contrato
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDto>> Lista();
        Task<List<CategoriaDto>> ListaActivo();
        Task<CategoriaDto> Crear(CategoriaDto categoriaDto);
        Task<bool> Editar(CategoriaDto categoriaDto);
        Task<bool> Eliminar(int id);
    }
}
