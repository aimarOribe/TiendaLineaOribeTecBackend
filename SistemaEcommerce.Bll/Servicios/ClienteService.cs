using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaEcommerce.Bll.Recursos;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Dal.Repositorios.Contrato;
using SistemaEcommerce.Dto;
using SistemaEcommerce.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SistemaEcommerce.Bll.Servicios
{
    public class ClienteService : IClienteService
    {
        private readonly IGenericRepository<Cliente> _clienteRepositorio;
        private readonly IMapper _mapper;

        public ClienteService(IGenericRepository<Cliente> clienteRepositorio, IMapper mapper)
        {
            _clienteRepositorio = clienteRepositorio;
            _mapper = mapper;
        }

        public async Task<List<ClienteDto>> Lista()
        {
            try
            {
                var queryUsuario = await _clienteRepositorio.Consultar();
                var listaUsuarios = queryUsuario.ToList();
                return _mapper.Map<List<ClienteDto>>(listaUsuarios);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SesionClienteDto> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                string claveEncriptada = RecursosService.ConvertirSha256(clave);

                var queryUsuario = await _clienteRepositorio.Consultar(u => u.Correo == correo && u.Clave == claveEncriptada);
                if (queryUsuario.FirstOrDefault() == null)
                {
                    throw new Exception("Credenciales Incorrectas");
                }
                else
                {
                    Cliente devolverCliente = queryUsuario.First();
                    return _mapper.Map<SesionClienteDto>(devolverCliente);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClienteDto> Crear(ClienteDto cliente)
        {
            try
            {
                var clienteModelo = _mapper.Map<Cliente>(cliente);
                var usuarioVerificacionCorreo = await _clienteRepositorio.Obtener(u => u.Correo == clienteModelo.Correo);
                if (usuarioVerificacionCorreo != null)
                {
                    throw new Exception("El correo ya se encuentra en uso");
                }
                else
                {
                    string clave = RecursosService.GenerarClave();
                    string asunto = "Creacion de Cuenta en OTienda";
                    string mensaje = "<h3>Se cuenta fue creada correctamente</h3></br><p>Su contraseña de acceso es !clave!</p>";
                    mensaje = mensaje.Replace("!clave!", clave);
                    bool respuesta = RecursosService.EnviarCorreo(cliente.Correo, asunto, mensaje);
                    if (respuesta)
                    {
                        cliente.Clave = RecursosService.ConvertirSha256(clave);
                        var clienteCreado = await _clienteRepositorio.Crear(_mapper.Map<Cliente>(cliente));
                        if (clienteCreado.IdCliente == 0)
                        {
                            throw new Exception("No se pudo crear");
                        }
                        var query = await _clienteRepositorio.Consultar(c => c.IdCliente == clienteCreado.IdCliente);
                        clienteCreado = query.First();
                        return _mapper.Map<ClienteDto>(clienteCreado);
                    }
                    else
                    {
                        throw new Exception("No se pudo enviar el correo, vuelva a intentarlo otra vez");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Editar(ClienteDto clienteDto)
        {
            try
            {
                var clienteModelo = _mapper.Map<Cliente>(clienteDto);
                var clienteEncontrado = await _clienteRepositorio.Obtener(c => c.IdCliente == clienteModelo.IdCliente);
                if (clienteEncontrado == null)
                {
                    throw new Exception("El cliente no existe");
                }

                var categoriaEncontradaVerificacion = await _clienteRepositorio.Obtener(c => c.Correo == clienteModelo.Correo && c.IdCliente != clienteModelo.IdCliente);
                if (categoriaEncontradaVerificacion != null)
                {
                    throw new Exception("El correo ya se encuentra en uso");
                }
                else
                {
                    clienteEncontrado.Nombres = clienteDto.Nombres;
                    clienteEncontrado.Apellidos = clienteDto.Apellidos;
                    clienteEncontrado.Correo = clienteDto.Correo;

                    bool respuesta = await _clienteRepositorio.Editar(clienteEncontrado);
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
                var clienteEncontrado = await _clienteRepositorio.Obtener(c => c.IdCliente == id);
                if (clienteEncontrado == null) { throw new Exception("El usuario no existe"); }


                bool respuesta = await _clienteRepositorio.Eliminar(clienteEncontrado);
                if (!respuesta)
                {
                    throw new Exception("No se pudo eliminar");
                }

                return respuesta;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CambiarClave(int idCliente, string claveAntigua, string claveNueva)
        {
            try
            {
                var clienteEncontrado = await _clienteRepositorio.Obtener(u => u.IdCliente == idCliente);
                if (clienteEncontrado == null) { throw new Exception("El cliente no existe"); }


                if (clienteEncontrado.Clave != RecursosService.ConvertirSha256(claveAntigua)) throw new Exception("Contraseña Antigua Incorrecta");

                clienteEncontrado.Clave = RecursosService.ConvertirSha256(claveNueva);

                clienteEncontrado.Reestablecer = false;

                bool respuesta = await _clienteRepositorio.Editar(clienteEncontrado);

                return respuesta;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RestablecerClave(string correo)
        {
            try
            {
                var clienteEncontrado = await _clienteRepositorio.Obtener(c => c.Correo == correo);
                if (clienteEncontrado == null) { throw new Exception("El correo no existe"); }

                string nuevaClave = RecursosService.GenerarClave();

                string asunto = "Cuenta Reestablecida en OTienda";
                string mensaje = "<h3>Se cuenta fue reestablecida correctamente</h3></br><p>Su nueva contraseña de acceso es !clave!</p>";
                mensaje = mensaje.Replace("!clave!", nuevaClave);
                bool respuesta = RecursosService.EnviarCorreo(clienteEncontrado.Correo, asunto, mensaje);

                if (respuesta)
                {
                    clienteEncontrado.Clave = RecursosService.ConvertirSha256(nuevaClave);
                    clienteEncontrado.Reestablecer = true;
                    var usuarioModificado = await _clienteRepositorio.Editar(clienteEncontrado);
                    if (usuarioModificado)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception("Ocurrio un error en el reestableciento de la contraseña");
                    }
                }
                else
                {
                    throw new Exception("Ocurrio un error en el envio del correo de reestablecimiento de su contraseña");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
