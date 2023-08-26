using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaEcommerce.Dal.DBContext;
using SistemaEcommerce.Dal.Repositorios.Contrato;
using SistemaEcommerce.Dal.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaEcommerce.Utility;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Bll.Servicios;

namespace SistemaEcommerce.Ioc
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbcarritoContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("cadena"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository, VentaRepository>();

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IMarcaService, MarcaService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IFireBaseService, FireBaseService>();
            services.AddScoped<IDepartamentoService, DepartamentoService>();
            services.AddScoped<IProvinciaService, ProvinciaService>();
            services.AddScoped<IDistritoService, DistritoService>();
        }
    }
}
