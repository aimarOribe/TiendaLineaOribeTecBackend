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
    public class MarcaService : IMarcaService
    {
        private readonly IGenericRepository<Marca> _marcaRepositorio;
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;

        public MarcaService(IGenericRepository<Marca> marcaRepositorio, IGenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _marcaRepositorio = marcaRepositorio;
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
        }

        public async Task<List<MarcaDto>> Lista()
        {
            try
            {
                var listaMarcas = await _marcaRepositorio.Consultar();
                return _mapper.Map<List<MarcaDto>>(listaMarcas.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MarcaDto>> ListarMarcasPorCategoria(int idCategoria)
        {
            try
            {
                var query = await _productoRepositorio.Consultar();
                query.Include(c => c.IdCategoriaNavigation!)
                    .Include(m => m.IdMarcaNavigation!)
                    .Where(p => p.IdCategoriaNavigation.Activo == true && p.IdMarcaNavigation.Activo == true);

                if (idCategoria != 0)
                {
                    query = query.Where(p => p.IdCategoria == idCategoria);
                }

                var marcas = await query
                    .Select(p => new Marca
                    {
                        IdMarca = p.IdMarcaNavigation.IdMarca,
                        Descripcion = p.IdMarcaNavigation.Descripcion
                    })
                    .Distinct() 
                    .ToListAsync();

                return _mapper.Map<List<MarcaDto>>(marcas);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MarcaDto> Crear(MarcaDto marcaDto)
        {
            try
            {
                var marcaModelo = _mapper.Map<Marca>(marcaDto);
                var marcaEncontrada = await _marcaRepositorio.Obtener(m => m.Descripcion == marcaModelo.Descripcion);
                if (marcaEncontrada != null)
                {
                    throw new Exception("La marca ya existe");
                }
                var marcaCreada = await _marcaRepositorio.Crear(_mapper.Map<Marca>(marcaDto));
                if (marcaCreada.IdMarca == 0)
                {
                    throw new Exception("No se pudo crear");
                }
                return _mapper.Map<MarcaDto>(marcaCreada);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Editar(MarcaDto marcaDto)
        {
            try
            {
                var marcaModelo = _mapper.Map<Marca>(marcaDto);

                var marcaEncontrada = await _marcaRepositorio.Obtener(m => m.IdMarca == marcaModelo.IdMarca);
                if (marcaEncontrada == null)
                {
                    throw new Exception("La marca no existe");
                }

                var marcaEncontradaVerificacion = await _marcaRepositorio.Obtener(m => m.Descripcion == marcaModelo.Descripcion && m.IdMarca != marcaModelo.IdMarca);
                if (marcaEncontradaVerificacion != null)
                {
                    throw new Exception("La marca ya existe");
                }
                else
                {
                    marcaEncontrada.Descripcion = marcaModelo.Descripcion;
                    marcaEncontrada.Activo = marcaModelo.Activo;
                    bool respuesta = await _marcaRepositorio.Editar(marcaEncontrada);
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
                var marcaEncontrada = await _marcaRepositorio.Obtener(m => m.IdMarca == id);
                if (marcaEncontrada == null) { throw new Exception("La marca no existe"); }

                var listaMarcaProducto = await _productoRepositorio.Consultar();
                if (listaMarcaProducto.Any(p => p.IdMarca == id))
                {
                    throw new Exception("No puede eliminar la marca " + marcaEncontrada.Descripcion + " porque esta enlazada a un producto");

                }
                else
                {
                    bool respuesta = await _marcaRepositorio.Eliminar(marcaEncontrada);
                    if (!respuesta)
                    {
                        throw new Exception("No se pudo eliminar");
                    }
                    return respuesta;
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
