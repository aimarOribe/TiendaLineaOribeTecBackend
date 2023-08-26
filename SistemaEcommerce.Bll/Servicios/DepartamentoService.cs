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
    public class DepartamentoService : IDepartamentoService
    {
        private readonly IGenericRepository<Departamento> _departamentoRepositorio;
        private readonly IMapper _mapper;

        public DepartamentoService(IGenericRepository<Departamento> departamentoRepositorio, IMapper mapper)
        {
            _departamentoRepositorio = departamentoRepositorio;
            _mapper = mapper;
        }
        public async Task<List<DepartamentoDto>> Lista()
        {
            try
            {
                var listaCategorias = await _departamentoRepositorio.Consultar();
                return _mapper.Map<List<DepartamentoDto>>(listaCategorias.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
