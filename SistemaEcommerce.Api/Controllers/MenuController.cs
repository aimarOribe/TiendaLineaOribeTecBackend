using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Dto;
using SistemaEcommerce.Api.Utilidades;

namespace SistemaEcommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuServicio;

        public MenuController(IMenuService menuServicio)
        {
            _menuServicio = menuServicio;
        }

        [HttpGet]
        [Route("lista/{idUsuario:int}")]
        public async Task<IActionResult> lista(int idUsuario)
        {
            var response = new Response<List<MenuDto>>();

            try
            {
                response.status = true;
                response.data = await _menuServicio.Lista(idUsuario);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                throw ex;
            }

            return Ok(response);
        }
    }
}
