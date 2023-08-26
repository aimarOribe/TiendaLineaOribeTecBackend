using System;
using System.Collections.Generic;

namespace SistemaEcommerce.Entity;

public partial class Menurol
{
    public int IdMenuRol { get; set; }

    public int? IdMenu { get; set; }

    public int? IdRol { get; set; }

    public virtual Menu? IdMenuNavigation { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }
}
