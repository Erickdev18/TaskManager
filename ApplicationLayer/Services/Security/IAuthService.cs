using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services.Security
{
    public interface IAuthService
    {
        Task<string> GetToken(string correo, int edad);
    }
}
