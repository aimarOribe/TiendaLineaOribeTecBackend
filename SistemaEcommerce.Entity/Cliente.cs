using System;
using System.Collections.Generic;

namespace SistemaEcommerce.Entity;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string? Correo { get; set; }

    public string? Clave { get; set; }

    public bool? Reestablecer { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
