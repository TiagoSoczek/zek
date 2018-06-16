using System.Threading;
using System.Threading.Tasks;

namespace Zek.Shared.Tasks
{
    public interface IScheduledTask
    {
        string Description { get; }
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}