using System.Threading;
using System.Threading.Tasks;

namespace WeKan.Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        string NameIdentifier { get; }

        Task<int> GetId(CancellationToken cancellationToken = default);
    }
}
