﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Dto
{
    public class MenuDto
    {
        public int IdMenu { get; set; }

        public string? Nombre { get; set; }

        public string? Icono { get; set; }

        public string? Urls { get; set; }
    }
}
