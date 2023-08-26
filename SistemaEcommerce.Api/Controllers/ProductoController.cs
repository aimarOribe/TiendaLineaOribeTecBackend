using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Dto;
using SistemaEcommerce.Api.Utilidades;
using Newtonsoft.Json;
using SistemaEcommerce.Entity;

namespace SistemaEcommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoServicio;

        public ProductoController(IProductoService productoServicio)
        {
            _productoServicio = productoServicio;
        }

        [HttpGet]
        [Route("lista")]
        public async Task<IActionResult> lista()
        {
            var response = new Response<List<ProductoDto>>();

            try
            {
                response.status = true;
                response.data = await _productoServicio.Lista();
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
        [Route("{idProducto}")]
        public async Task<IActionResult> ObtenerPorId(int idProducto)
        {
            var response = new Response<ProductoDto>();

            try
            {
                response.status = true;
                response.data = await _productoServicio.ObtenerPorId(idProducto);
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
        [Route("ListarTienda/{idCategoria:int}/{idMarca:int}")]
        public async Task<IActionResult> ListarTienda(int idCategoria, int idMarca)
        {
            var response = new Response<List<ProductoDto>>();

            try
            {
                response.status = true;
                response.data = await _productoServicio.ListarTienda(idCategoria, idMarca);
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
        public async Task<IActionResult> Guardar([FromForm] IFormFile? imagen, [FromForm] string producto)
        {
            var response = new Response<ProductoDto>();

            try
            {
                ProductoDto productoDto = JsonConvert.DeserializeObject<ProductoDto>(producto);
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
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "productoPorDefecto.png");
                    nombreImagen = string.Concat(nombreEnCodigo, ".png");
                    imagenStream = System.IO.File.OpenRead(imagePath);
                }

                response.status = true;
                response.data = await _productoServicio.Crear(productoDto, imagenStream, nombreImagen);
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
        public async Task<IActionResult> Editar([FromForm] IFormFile? imagen, [FromForm] string producto)
        {
            var response = new Response<bool>();

            try
            {
                ProductoDto productoDto = JsonConvert.DeserializeObject<ProductoDto>(producto);
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
                response.data = await _productoServicio.Editar(productoDto, imagenStream, nombreImagen);
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
                response.data = await _productoServicio.Eliminar(id);
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
