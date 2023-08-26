using System;
using System.Collections.Generic;

namespace SistemaEcommerce.Entity;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int? IdRol { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string? Correo { get; set; }

    public string? Clave { get; set; }

    public bool? Reestablecer { get; set; }

    public bool? Activo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public string? RutaImagen { get; set; }

    public string? NombreImagen { get; set; }


    public virtual Rol? IdRolNavigation { get; set; }
}
