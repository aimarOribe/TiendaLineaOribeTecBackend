using SistemaEcommerce.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Bll.Servicios.Contrato
{
    public interface IProvinciaService
    {
        Task<List<ProvinciaDto>> Lista(string idDepartamento);
    }
}
