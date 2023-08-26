using SistemaEcommerce.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Bll.Servicios.Contrato
{
    public interface IProductoService
    {
        Task<List<ProductoDto>> Lista();
        Task<List<ProductoDto>> ListarTienda(int idCategoria, int idMarca);
        Task<ProductoDto> ObtenerPorId(int idProducto);
        Task<ProductoDto> Crear(ProductoDto producto, Stream imagen = null, string nombreImagen = "");
        Task<bool> Editar(ProductoDto producto, Stream imagen = null, string nombreImagen = "");
        Task<bool> Eliminar(int id);
    }
}
