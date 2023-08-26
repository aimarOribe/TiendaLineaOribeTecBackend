using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Api.Utilidades;
using SistemaEcommerce.Dto;
using Newtonsoft.Json;
using SistemaEcommerce.Entity;

namespace SistemaEcommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioServicio;

        public UsuarioController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        [HttpGet]
        [Route("lista")]
        public async Task<IActionResult> lista()
        {
            var response = new Response<List<UsuarioDto>>();

            try
            {
                response.status = true;
                response.data = await _usuarioServicio.Lista();
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{idUsuario:int}")]
        public async Task<IActionResult> ById(int idUsuario)
        {
            var response = new Response<UsuarioDto>();

            try
            {
                response.status = true;
                response.data = await _usuarioServicio.ObtenerById(idUsuario);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDto login)
        {
            var response = new Response<SesionDto>();

            try
            {
                response.status = true;
                response.data = await _usuarioServicio.ValidarCredenciales(login.Correo, login.Clave);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromForm] IFormFile? imagen, [FromForm] string usuario)
        {
            var response = new Response<UsuarioDto>();

            try
            {
                UsuarioDto usuarioDto = JsonConvert.DeserializeObject<UsuarioDto>(usuario);
                string nombreImagen = "";
                Stream imagenStream = null;

                string nombreEnCodigo = Guid.NewGuid().ToString("N");

                if (imagen != null)
                {
                    string extension = Path.GetExtension(imagen.FileName);
                    nombreImagen = string.Concat(nombreEnCodigo, extension);
                    imagenStream = imagen.OpenReadStream();
                }
                else
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "usuarioPorDefecto.png");
                    nombreImagen = string.Concat(nombreEnCodigo, ".png");
                    imagenStream = System.IO.File.OpenRead(imagePath);
                }

                response.status = true;
                response.data = await _usuarioServicio.Crear(usuarioDto, imagenStream, nombreImagen);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromForm] IFormFile? imagen, [FromForm] string usuario)
        {
            var response = new Response<bool>();

            try
            {
                UsuarioDto usuarioDto = JsonConvert.DeserializeObject<UsuarioDto>(usuario);
                string nombreImagen = "";
                Stream imagenStream = null;

                if (imagen != null)
                {
                    string nombreEnCodigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(imagen.FileName);
                    nombreImagen = string.Concat(nombreEnCodigo, extension);
                    imagenStream = imagen.OpenReadStream();
                }

                response.status = true;
                response.data = await _usuarioServicio.Editar(usuarioDto, imagenStream, nombreImagen);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.data = await _usuarioServicio.Eliminar(id);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);
        }
    }
}
