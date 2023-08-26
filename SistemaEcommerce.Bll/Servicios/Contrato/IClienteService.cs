using SistemaEcommerce.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Bll.Servicios.Contrato
{
    public interface IClienteService
    {
        Task<List<ClienteDto>> Lista();
        Task<SesionClienteDto> ValidarCredenciales(string correo, string clave);
        Task<ClienteDto> Crear(ClienteDto cliente);
        Task<bool> Editar(ClienteDto clienteDto);
        Task<bool> Eliminar(int id);
        Task<bool> CambiarClave(int idCliente, string claveAntigua, string claveNueva);
        Task<bool> RestablecerClave(string correo);
        
    }
}
