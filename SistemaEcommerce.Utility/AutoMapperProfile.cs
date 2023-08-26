using AutoMapper;
using SistemaEcommerce.Dto;
using SistemaEcommerce.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Utility
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() 
        {
            #region Rol
            CreateMap<Rol, RolDto>().ReverseMap();
            #endregion Rol

            #region Menu
            CreateMap<Menu, MenuDto>().ReverseMap();
            #endregion Menu

            #region Usuario
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(destino =>
                    destino.RolDescripcion,
                    opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre)
                )
                .ForMember(destino =>
                    destino.Activo,
                    opt => opt.MapFrom(origen => origen.Activo == true ? 1 : 0)
                );
            
            CreateMap<Usuario, SesionDto>()
                .ForMember(destino =>
                    destino.RolDescripcion,
                    opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre)
                );

            CreateMap<UsuarioDto, Usuario>()
                .ForMember(destino =>
                    destino.IdRolNavigation,
                    opt => opt.Ignore()
                )
                .ForMember(destino =>
                    destino.Activo,
                    opt => opt.MapFrom(origen => origen.Activo == 1 ? true : false)
                );

            CreateMap<SesionDto, Usuario>()
                .ForMember(destino =>
                    destino.IdRolNavigation,
                    opt => opt.Ignore()
                );
            #endregion Usuario

            #region Cliente
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Cliente, SesionClienteDto>().ReverseMap();
            #endregion

            #region Departamento
            CreateMap<Departamento, DepartamentoDto>().ReverseMap();
            #endregion

            #region Provincia
            CreateMap<Provincia, ProvinciaDto>().ReverseMap();
            #endregion

            #region Distrito
            CreateMap<Distrito, DistritoDto>().ReverseMap();
            #endregion

            #region Marca
            CreateMap<Marca, MarcaDto>().ReverseMap();
            #endregion Marca

            #region Categoria
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            #endregion Categoria

            #region Producto
            CreateMap<Producto, ProductoDto>()
                .ForMember(destino =>
                    destino.DescripcionMarca,
                    opt => opt.MapFrom(origen => origen.IdMarcaNavigation.Descripcion)
                )
                .ForMember(destino =>
                    destino.DescripcionCategoria,
                    opt => opt.MapFrom(origen => origen.IdCategoriaNavigation.Descripcion)
                )
                .ForMember(destino =>
                    destino.Precio,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-PE")))
                )
                .ForMember(destino =>
                    destino.Stock,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Stock.Value, new CultureInfo("es-PE")))
                )
                .ForMember(destino =>
                    destino.Activo,
                    opt => opt.MapFrom(origen => origen.Activo == true ? 1 : 0)
                );

            CreateMap<ProductoDto, Producto>()
                .ForMember(destino =>
                    destino.IdMarcaNavigation,
                    opt => opt.Ignore()
                )
                .ForMember(destino =>
                    destino.IdCategoriaNavigation,
                    opt => opt.Ignore()
                )
                .ForMember(destino =>
                    destino.Precio,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-PE")))
                )
                .ForMember(destino =>
                    destino.Stock,
                    opt => opt.MapFrom(origen => Convert.ToInt32(origen.Stock, new CultureInfo("es-PE")))
                )
                .ForMember(destino =>
                    destino.Activo,
                    opt => opt.MapFrom(origen => origen.Activo == 1 ? true : false)
                );
            #endregion Producto

            #region Venta
            CreateMap<Venta, VentaDto>()
                .ForMember(destino =>
                    destino.DescripcionCliente,
                    opt => opt.MapFrom(origen => origen.IdClienteNavigation.Nombres + " " + origen.IdClienteNavigation.Apellidos)
                )
                .ForMember(destino =>
                    destino.MontoTotalTexto,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.MontoTotal.Value, new CultureInfo("es-PE")))
                )
                .ForMember(destino =>
                    destino.FechaVenta,
                    opt => opt.MapFrom(origen => origen.FechaVenta.Value.ToString("dd/MM/yyyy"))
                );

            CreateMap<VentaDto, Venta>()
                .ForMember(destino =>
                    destino.IdClienteNavigation,
                    opt => opt.Ignore()
                )
                .ForMember(destino =>
                    destino.MontoTotal,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.MontoTotalTexto, new CultureInfo("es-PE")))
                );
            #endregion Venta

            #region DetalleVenta
            CreateMap<Detalleventa, DetalleVentaDto>()
                .ForMember(destino =>
                    destino.RutaImagen,
                    opt => opt.MapFrom(origen => origen.IdProductoNavigation.RutaImagen)
                )
                .ForMember(destino =>
                    destino.DescripcionProducto,
                    opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre)
                )
                .ForMember(destino =>
                    destino.PrecioTexto,
                    opt => opt.MapFrom(origen => origen.IdProductoNavigation.Precio)
                )
                .ForMember(destino =>
                    destino.TotalTexto,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE")))
                );

            CreateMap<DetalleVentaDto, Detalleventa>()
                .ForMember(destino =>
                    destino.Precio,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioTexto, new CultureInfo("es-PE")))
                )
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-PE")))
                );
            #endregion DetalleVenta

            #region Reporte
            CreateMap<Detalleventa, ReporteDto>()
                .ForMember(destino =>
                    destino.FechaVenta,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.FechaVenta.Value.ToString("dd/MM/yyyy"))
                )
                .ForMember(destino =>
                    destino.IdTransaccion,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.IdTransaccion)
                )
                .ForMember(destino =>
                    destino.TotalVenta,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.IdVentaNavigation.MontoTotal.Value, new CultureInfo("es-PE")))
                )
                .ForMember(destino =>
                    destino.Producto,
                    opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre)
                )
                .ForMember(destino =>
                    destino.Cantidad,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Cantidad.Value, new CultureInfo("es-PE")))
                )
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE")))
                );
            #endregion Reporte
        }

    }
}
