using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Dto
{
    public class NewPasswordDto
    {
        public int idUsuario { get; set; }

        public string claveAntigua { get; set; }

        public string claveNueva { get; set; }
    }
}
