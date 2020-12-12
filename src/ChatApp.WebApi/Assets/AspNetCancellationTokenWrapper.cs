using ChatApp.Identity.Core.Assets;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace ChatApp.WebApi.Assets
{
    public class AspNetCancellationTokenWrapper : ICancellationTokenWrapper
    {
        public CancellationToken CancellationToken { get; }
        public AspNetCancellationTokenWrapper(IHttpContextAccessor httpContextAccessor)
        {
            CancellationToken = httpContextAccessor?.HttpContext?.RequestAborted ?? CancellationToken.None;
        }
    }
}
