using System.Threading.Tasks;

namespace TechStart.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}