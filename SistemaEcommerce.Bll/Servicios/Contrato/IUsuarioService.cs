using SistemaEcommerce.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Bll.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDto>> Lista();
        Task<UsuarioDto> ObtenerById(int idUsuario);
        Task<SesionDto> ValidarCredenciales(string correo, string clave);
        Task<UsuarioDto> Crear(UsuarioDto usuario, Stream imagen = null, string nombreImagen = "");
        Task<bool> Editar(UsuarioDto usuario, Stream imagen = null, string nombreImagen = "");
        Task<bool> Eliminar(int id);
        Task<bool> CambiarClave(int idUsuario, string claveAntigua, string claveNueva);
        Task<bool> RestablecerClave(string correo);
    }
}
