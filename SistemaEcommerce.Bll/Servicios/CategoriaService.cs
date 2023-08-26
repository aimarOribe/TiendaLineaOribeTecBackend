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

namespace SistemaEcommerce.Bll.Servicios
{
    public class CategoriaService : ICategoriaService
    {

        private readonly IGenericRepository<Categoria> _categoriaRepositorio;
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<Categoria> categoriaRepositorio, IGenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDto>> Lista()
        {
            try
            {
                var listaCategorias = await _categoriaRepositorio.Consultar();
                return _mapper.Map<List<CategoriaDto>>(listaCategorias.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CategoriaDto>> ListaActivo()
        {
            try
            {
                var listaCategoriasActivas = await _categoriaRepositorio
                    .Consultar(c => c.Activo == true);

                return _mapper.Map<List<CategoriaDto>>(listaCategoriasActivas.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CategoriaDto> Crear(CategoriaDto categoriaDto)
        {
            try
            {
                var categoriaModelo = _mapper.Map<Categoria>(categoriaDto);
                var categoriaEncontrada = await _categoriaRepositorio.Obtener(c => c.Descripcion == categoriaModelo.Descripcion);
                if (categoriaEncontrada != null)
                {
                    throw new Exception("La categoria ya existe");
                }

                var categoriaCreada = await _categoriaRepositorio.Crear(_mapper.Map<Categoria>(categoriaDto));
                if (categoriaCreada.IdCategoria == 0)
                {
                    throw new Exception("No se pudo crear");
                }
                return _mapper.Map<CategoriaDto>(categoriaCreada);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Editar(CategoriaDto categoriaDto)
        {
            try
            {
                var categoriaModelo = _mapper.Map<Categoria>(categoriaDto);

                var categoriaEncontrada = await _categoriaRepositorio.Obtener(c => c.IdCategoria == categoriaModelo.IdCategoria);
                if (categoriaEncontrada == null) { 
                    throw new Exception("La categoria no existe"); 
                }

                var categoriaEncontradaVerificacion = await _categoriaRepositorio.Obtener(c => c.Descripcion == categoriaModelo.Descripcion && c.IdCategoria != categoriaModelo.IdCategoria);
                if (categoriaEncontradaVerificacion != null)
                {
                    throw new Exception("La categoria ya existe");
                }
                else
                {
                    categoriaEncontrada.Descripcion = categoriaModelo.Descripcion;
                    categoriaEncontrada.Activo = categoriaModelo.Activo;
                    bool respuesta = await _categoriaRepositorio.Editar(categoriaEncontrada);
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
                var categoriaEncontrada = await _categoriaRepositorio.Obtener(c => c.IdCategoria == id);
                if (categoriaEncontrada == null) { throw new Exception("La categoria no existe"); }

                var listaCategoriaProducto = await _productoRepositorio.Consultar();
                if(listaCategoriaProducto.Any(p => p.IdCategoria == id))
                {
                    throw new Exception("No puede eliminar la categoria " + categoriaEncontrada.Descripcion + " porque esta enlazada a un producto");

                }
                else
                {
                    bool respuesta = await _categoriaRepositorio.Eliminar(categoriaEncontrada);
                    if (!respuesta)
                    {
                        throw new Exception("No se pudo eliminar");
                    }
                    return respuesta;
                };   
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
