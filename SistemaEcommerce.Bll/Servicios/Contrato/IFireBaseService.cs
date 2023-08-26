using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Bll.Servicios.Contrato
{
    public interface IFireBaseService
    {
        Task<string> subirStorage(Stream streamArchivo, string CarpetaDestino, string NombreArchivo);
        Task<bool> eliminarStorage(string CarpetaDestino, string NombreArchivo);
    }
}
