using System.Threading.Tasks;

namespace WeKan.Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        string NameIdentifier { get; }

        Task<int> GetId();
    }
}
