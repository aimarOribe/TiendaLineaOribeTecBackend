﻿using SistemaEcommerce.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Bll.Servicios.Contrato
{
    public interface IMenuService
    {
        Task<List<MenuDto>> Lista(int idUsuario);
    }
}
