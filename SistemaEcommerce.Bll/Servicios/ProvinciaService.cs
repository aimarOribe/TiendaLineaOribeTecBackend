using AutoMapper;
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
    public class ProvinciaService : IProvinciaService
    {
        private readonly IGenericRepository<Provincia> _provinciaRepositorio;
        private readonly IMapper _mapper;

        public ProvinciaService(IGenericRepository<Provincia> provinciaRepositorio, IMapper mapper)
        {
            _provinciaRepositorio = provinciaRepositorio;
            _mapper = mapper;
        }
        public async Task<List<ProvinciaDto>> Lista(string idDepartamento)
        {
            try
            {
                var listaProvincias = await _provinciaRepositorio.Consultar(p => p.IdDepartamento == idDepartamento);
                return _mapper.Map<List<ProvinciaDto>>(listaProvincias.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
