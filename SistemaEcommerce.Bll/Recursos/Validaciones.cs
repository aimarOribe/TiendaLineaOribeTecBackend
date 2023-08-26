using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Bll.Recursos
{
    public class Validaciones
    {
        public static bool TryConvertToDecimal(string input, out decimal result)
        {
            return decimal.TryParse(input, out result);
        }

        public static bool TryConvertToInt(string input, out int result)
        {
            return int.TryParse(input, out result);
        }
    }
}
