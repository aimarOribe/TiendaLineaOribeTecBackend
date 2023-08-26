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
    public class DistritoService : IDistritoService
    {
        private readonly IGenericRepository<Distrito> _distritoRepositorio;
        private readonly IMapper _mapper;

        public DistritoService(IGenericRepository<Distrito> distritoRepositorio, IMapper mapper)
        {
            _distritoRepositorio = distritoRepositorio;
            _mapper = mapper;
        }
        public async Task<List<DistritoDto>> Lista(string idProvincia, string idDepartamento)
        {
            try
            {
                var listaProvincias = await _distritoRepositorio.Consultar(d => d.IdProvincia == idProvincia && d.IdDepartamento == idDepartamento);
                return _mapper.Map<List<DistritoDto>>(listaProvincias.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
