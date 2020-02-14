using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CophiPoint.Services
{
    public interface INativeAuthService
    {
        bool IsLogged { get; }
        Task<(bool IsSucessful, string Error)> Login(Api.Urls.OIDCUrls urls);
        Task<(string accessToken, string idToken)> GetTokens();
        Task LogOut();
    }

}
