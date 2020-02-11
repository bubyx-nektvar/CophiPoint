using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CophiPoint.Services
{
    public interface INativeHttpService
    {
        HttpMessageHandler GetNativeHandler();
    }
}
