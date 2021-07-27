using System.Threading.Tasks;
using TechStart.Core;
using TechStart.DbContexts;

namespace TechStart.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Initializer
        private readonly TechDbContext context;
        public UnitOfWork(TechDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region public async Task CompleteAsync()
        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
        #endregion
    }
}