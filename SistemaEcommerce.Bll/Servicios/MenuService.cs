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
    public class MenuService: IMenuService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private readonly IGenericRepository<Menurol> _menuRolRepositorio;
        private readonly IGenericRepository<Menu> _menuRepositorio;
        private readonly IMapper _mapper;

        public MenuService(IGenericRepository<Usuario> usuarioRepositorio,
            IGenericRepository<Menurol> menuRolRepositorio,
            IGenericRepository<Menu> menuRepositorio,
            IMapper mapper)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _menuRolRepositorio = menuRolRepositorio;
            _menuRepositorio = menuRepositorio;
            _mapper = mapper;
        }

        public async Task<List<MenuDto>> Lista(int idUsuario)
        {
            IQueryable<Usuario> tablaUsuario = await _usuarioRepositorio.Consultar(u => u.IdUsuario == idUsuario);
            IQueryable<Menurol> tablaMenuRol = await _menuRolRepositorio.Consultar();
            IQueryable<Menu> tablaMenu = await _menuRepositorio.Consultar();

            try
            {
                IQueryable<Menu> tablaResultado = (from u in tablaUsuario join mr in tablaMenuRol on u.IdRol equals mr.IdRol join m in tablaMenu on mr.IdMenu equals m.IdMenu select m).AsQueryable();
                var listaMenus = tablaResultado.ToList();
                return _mapper.Map<List<MenuDto>>(listaMenus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
