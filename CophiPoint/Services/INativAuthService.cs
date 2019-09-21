using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Services
{
    public interface INativAuthService
    {
        bool IsLogged { get; }
        Task Login();

        Task<(string accessToken, string idToken)> GetTokens();
    }

}
