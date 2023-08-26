using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Dal.Repositorios.Contrato;
using SistemaEcommerce.Dto;
using SistemaEcommerce.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaEcommerce.Bll.Recursos;

namespace SistemaEcommerce.Bll.Servicios
{
    public class ProductoService : IProductoService
    {

        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IFireBaseService _fireBaseServicio;
        private readonly IMapper _mapper;

        public ProductoService(IGenericRepository<Producto> productoRepositorio, IFireBaseService fireBaseServicio, IMapper mapper)
        {
            _productoRepositorio = productoRepositorio;
            _fireBaseServicio = fireBaseServicio;
            _mapper = mapper;
        }

        public async Task<List<ProductoDto>> Lista()
        {
            try
            {
                var queryProducto = await _productoRepositorio.Consultar();
                var listaProductos = queryProducto.Include(cat => cat.IdCategoriaNavigation).Include(mar => mar.IdMarcaNavigation).ToList();
                return _mapper.Map<List<ProductoDto>>(listaProductos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductoDto> ObtenerPorId(int idProducto)
        {
            try
            {
                var producto = await _productoRepositorio.Consultar(p => p.IdProducto == idProducto);
                var productoCompleto = producto.Include(cat => cat.IdCategoriaNavigation).Include(mar => mar.IdMarcaNavigation).First();
                return _mapper.Map<ProductoDto>(productoCompleto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductoDto>> ListarTienda(int idCategoria, int idMarca)
        {
            try
            {
                var queryProducto = await _productoRepositorio
                    .Consultar(p => 
                        p.IdCategoriaNavigation.IdCategoria == (idCategoria == 0 ? p.IdCategoriaNavigation.IdCategoria : idCategoria) &&
                        p.IdMarcaNavigation.IdMarca == (idMarca == 0 ? p.IdMarcaNavigation.IdMarca : idMarca) &&
                        p.Stock > 0 &&
                        p.Activo == true
                    );

                var listaProductos = queryProducto.Include(cat => cat.IdCategoriaNavigation).Include(mar => mar.IdMarcaNavigation).ToList();
                return _mapper.Map<List<ProductoDto>>(listaProductos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductoDto> Crear(ProductoDto producto, Stream imagen = null, string nombreImagen = "")
        {
            try
            {
                //Validaciones
                if (!Validaciones.TryConvertToDecimal(producto.Precio, out decimal precioDecimalValue))
                {
                    throw new Exception("Ingresa el precio en el formato ##.##");
                }

                if (!Validaciones.TryConvertToInt(producto.Stock, out int stockIntValue))
                {
                    throw new Exception("Ingresa el stock en el formato ##");
                }

                var productoModelo = _mapper.Map<Producto>(producto);

                var productoEncontrado = await _productoRepositorio.Obtener(p => p.Nombre == productoModelo.Nombre);
                if (productoEncontrado != null)
                {
                    throw new Exception("El producto ya existe");
                }

                producto.NombreImagen = nombreImagen;
                if (imagen != null)
                {
                    string urlImagen = await _fireBaseServicio.subirStorage(imagen, "carpeta_producto", nombreImagen);
                    producto.RutaImagen = urlImagen;
                }

                var productoCreado = await _productoRepositorio.Crear(_mapper.Map<Producto>(producto));
                if (productoCreado.IdProducto == 0)
                {
                    throw new Exception("No se pudo crear");
                }
                return _mapper.Map<ProductoDto>(productoCreado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Editar(ProductoDto producto, Stream imagen = null, string nombreImagen = "")
        {
            try
            {
                //Validaciones
                if (!Validaciones.TryConvertToDecimal(producto.Precio, out decimal precioDecimalValue))
                {
                    throw new Exception("Ingresa el precio en el formato ##.##");
                }

                if (!Validaciones.TryConvertToInt(producto.Stock, out int stockIntValue))
                {
                    throw new Exception("Ingresa el stock en el formato ##");
                }

                var productoModelo = _mapper.Map<Producto>(producto);

                var productoEncontrado = await _productoRepositorio.Obtener(u =>u.IdProducto == productoModelo.IdProducto);
                if (productoEncontrado == null)
                {
                    throw new Exception("El producto no existe");
                }

                var productoEncontradaVerificacion = await _productoRepositorio.Obtener(p => p.Nombre == productoModelo.Nombre && p.IdProducto != productoModelo.IdProducto);
                if (productoEncontradaVerificacion != null)
                {
                    throw new Exception("La producto ya existe");
                }
                else
                {
                    productoEncontrado.Nombre = productoModelo.Nombre;
                    productoEncontrado.Decripcion = productoModelo.Decripcion;
                    productoEncontrado.IdMarca = productoModelo.IdMarca;
                    productoEncontrado.IdCategoria = productoModelo.IdCategoria;
                    productoEncontrado.Precio = productoModelo.Precio;
                    productoEncontrado.Stock = productoModelo.Stock;
                    productoEncontrado.Activo = productoModelo.Activo;

                    if (productoEncontrado.NombreImagen == "")
                    {
                        productoEncontrado.NombreImagen = nombreImagen;
                    }

                    if (imagen != null)
                    {
                        string urlImagen = await _fireBaseServicio.subirStorage(imagen, "carpeta_producto", productoEncontrado.NombreImagen);
                        productoEncontrado.RutaImagen = urlImagen;
                    }

                    bool respuesta = await _productoRepositorio.Editar(productoEncontrado);

                    if (!respuesta)
                    {
                        throw new Exception("No se pudo editar");
                    }

                    return respuesta;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var productoEncontrado = await _productoRepositorio.Obtener(p => p.IdProducto == id);
                if (productoEncontrado == null) { throw new Exception("El producto no existe"); }

                string nombreImagen = productoEncontrado.NombreImagen;

                bool respuesta = await _productoRepositorio.Eliminar(productoEncontrado);
                if (!respuesta)
                {
                    throw new Exception("No se pudo eliminar");
                }
                else
                {
                    if(nombreImagen != "")
                    {
                        await _fireBaseServicio.eliminarStorage("carpeta_producto", nombreImagen);

                    }
                }
                return respuesta;   
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
