using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Services
{
    public interface INativAuthService
    {
        bool IsLogged { get; }
        Task<(bool IsSucessful, string Error)> Login();
        Task<(string accessToken, string idToken)> GetTokens();
        Task LogOut();
    }

}
