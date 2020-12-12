using System.Threading;

namespace ChatApp.Identity.Core.Assets
{
    public interface ICancellationTokenWrapper
    {
        CancellationToken CancellationToken { get; }
    }
}
