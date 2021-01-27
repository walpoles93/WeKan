using System.Threading;
using System.Threading.Tasks;

namespace WeKan.Application.Common.Interfaces
{
    public interface IAuthorizer<TRequest>
    {
        Task<bool> Authorise(TRequest request, CancellationToken cancellationToken = default);
    }
}
