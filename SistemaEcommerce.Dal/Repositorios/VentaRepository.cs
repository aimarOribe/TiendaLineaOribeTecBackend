using SistemaEcommerce.Dal.DBContext;
using SistemaEcommerce.Dal.Repositorios.Contrato;
using SistemaEcommerce.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Dal.Repositorios
{
    public class VentaRepository: GenericRepository<Venta>, IVentaRepository
    {
        private readonly DbcarritoContext _dbcontext;

        public VentaRepository(DbcarritoContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Venta> Registrar(Venta venta)
        {
            Venta ventaGenerada = new Venta();
            using (var trasaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    foreach (Detalleventa dv in venta.Detalleventa)
                    {
                        Producto productoEncontrado = _dbcontext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();
                        productoEncontrado.Stock = productoEncontrado.Stock - dv.Cantidad;
                        _dbcontext.Productos.Update(productoEncontrado);
                    }
                    await _dbcontext.SaveChangesAsync();
                    await _dbcontext.Venta.AddAsync(venta);
                    await _dbcontext.SaveChangesAsync();

                    ventaGenerada = venta;

                    trasaction.Commit();
                }
                catch (Exception)
                {
                    trasaction.Rollback();
                    throw;
                }

                return ventaGenerada;
            }
        }
    }
}
