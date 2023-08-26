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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Bll.Servicios
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private readonly IFireBaseService _fireBaseServicio;
        private readonly IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario> usuarioRepositorio, IFireBaseService fireBaseServicio, IMapper mapper)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _fireBaseServicio = fireBaseServicio;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDto>> Lista()
        {
            try
            {
                var queryUsuario = await _usuarioRepositorio.Consultar();
                var listaUsuarios = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();
                return _mapper.Map<List<UsuarioDto>>(listaUsuarios);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<UsuarioDto> ObtenerById(int idUsuario)
        {
            try
            {
                var queryUsuario = await _usuarioRepositorio.Consultar(u => u.IdUsuario == idUsuario);
                var usuario = queryUsuario.Include(rol => rol.IdRolNavigation).First();
                return _mapper.Map<UsuarioDto>(usuario);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SesionDto?> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                string claveEncriptada = RecursosService.ConvertirSha256(clave);

                var queryUsuario = await _usuarioRepositorio.Consultar(u => u.Correo == correo && u.Clave == claveEncriptada);
                if (queryUsuario.FirstOrDefault() == null)
                {
                    throw new Exception("Credenciales Incorrectas");
                }
                else
                {
                    Usuario devolverUsuario = queryUsuario.Include(u => u.IdRolNavigation).First();
                    return _mapper.Map<SesionDto>(devolverUsuario);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UsuarioDto> Crear(UsuarioDto usuario, Stream imagen = null, string nombreImagen = "")
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(usuario);
                var usuarioVerificacionCorreo = await _usuarioRepositorio.Obtener(u => u.Correo == usuarioModelo.Correo);
                if (usuarioVerificacionCorreo != null)
                {
                    throw new Exception("El correo ya existe");
                }
                else
                {
                    usuario.NombreImagen = nombreImagen;
                    if (imagen != null)
                    {
                        string urlImagen = await _fireBaseServicio.subirStorage(imagen, "carpeta_usuario", nombreImagen);
                        usuario.RutaImagen = urlImagen;
                    }

                    string clave = RecursosService.GenerarClave();
                    string asunto = "Creacion de Cuenta en OTienda";
                    string mensaje = "<h3>Se cuenta fue creada correctamente</h3></br><p>Su contraseña de acceso es !clave!</p>";
                    mensaje = mensaje.Replace("!clave!", clave);
                    bool respuesta = RecursosService.EnviarCorreo(usuario.Correo, asunto, mensaje);
                    if (respuesta)
                    {
                        usuario.Clave = RecursosService.ConvertirSha256(clave);
                        var usuarioCreado = await _usuarioRepositorio.Crear(_mapper.Map<Usuario>(usuario));
                        if (usuarioCreado.IdUsuario == 0)
                        {
                            throw new Exception("No se pudo crear");
                        }
                        var query = await _usuarioRepositorio.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);
                        usuarioCreado = query.Include(rol => rol.IdRolNavigation).First();
                        return _mapper.Map<UsuarioDto>(usuarioCreado);
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

        public async Task<bool> Editar(UsuarioDto usuario, Stream imagen = null, string nombreImagen = "")
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(usuario);
                var usuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.IdUsuario == usuarioModelo.IdUsuario);
                if (usuarioEncontrado == null)
                {
                    throw new Exception("El usuario no existe");
                }

                var categoriaEncontradaVerificacion = await _usuarioRepositorio.Obtener(u => u.Correo == usuarioModelo.Correo && u.IdUsuario != usuarioModelo.IdUsuario);
                if (categoriaEncontradaVerificacion != null)
                {
                    throw new Exception("El correo ya existe");
                }
                else
                {
                    usuarioEncontrado.Nombres = usuarioModelo.Nombres;
                    usuarioEncontrado.Apellidos = usuarioModelo.Apellidos;
                    usuarioEncontrado.Correo = usuarioModelo.Correo;
                    usuarioEncontrado.IdRol = usuarioModelo.IdRol;
                    usuarioEncontrado.Activo = usuarioModelo.Activo;

                    if (usuarioEncontrado.NombreImagen == "")
                    {
                        usuarioEncontrado.NombreImagen = nombreImagen;
                    }

                    if (imagen != null)
                    {
                        string urlImagen = await _fireBaseServicio.subirStorage(imagen, "carpeta_usuario", usuarioEncontrado.NombreImagen);
                        usuarioEncontrado.RutaImagen = urlImagen;
                    }

                    bool respuesta = await _usuarioRepositorio.Editar(usuarioEncontrado);
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
                var usuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.IdUsuario == id);
                if (usuarioEncontrado == null) { throw new Exception("El usuario no existe"); }

                string nombreImagen = usuarioEncontrado.NombreImagen;

                bool respuesta = await _usuarioRepositorio.Eliminar(usuarioEncontrado);
                if (!respuesta)
                {
                    throw new Exception("No se pudo eliminar");
                }
                else
                {
                    if (nombreImagen != "")
                    {
                        await _fireBaseServicio.eliminarStorage("carpeta_usuario", nombreImagen);

                    }
                }
                return respuesta;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CambiarClave(int idUsuario, string claveAntigua, string claveNueva)
        {
            try
            {
                var usuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.IdUsuario == idUsuario);
                if (usuarioEncontrado == null) { throw new Exception("El usuario no existe"); }


                if (usuarioEncontrado.Clave != RecursosService.ConvertirSha256(claveAntigua)) throw new Exception("Contraseña Antigua Incorrecta");

                usuarioEncontrado.Clave = RecursosService.ConvertirSha256(claveNueva);

                usuarioEncontrado.Reestablecer = false;

                bool respuesta = await _usuarioRepositorio.Editar(usuarioEncontrado);

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
                var usuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.Correo == correo);
                if (usuarioEncontrado == null) { throw new Exception("El correo no existe"); }

                string nuevaClave = RecursosService.GenerarClave();
                
                string asunto = "Cuenta Reestablecida en OTienda";
                string mensaje = "<h3>Se cuenta fue reestablecida correctamente</h3></br><p>Su nueva contraseña de acceso es !clave!</p>";
                mensaje = mensaje.Replace("!clave!", nuevaClave);
                bool respuesta = RecursosService.EnviarCorreo(usuarioEncontrado.Correo, asunto, mensaje);
               
                if (respuesta)
                {
                    usuarioEncontrado.Clave = RecursosService.ConvertirSha256(nuevaClave);
                    usuarioEncontrado.Reestablecer = true;
                    var usuarioModificado = await _usuarioRepositorio.Editar(usuarioEncontrado);
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
