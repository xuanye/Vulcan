using System.Threading.Tasks;
using Vulcan.DapperExtensions.ORMapping;

namespace Vulcan.DapperExtensions.Contract
{
    public interface IRepository
    {
        /// <summary>
        /// create unit of work
        /// </summary>
        /// <param name="isTrans">is db transaction</param>
        /// <returns></returns>
        IUnitOfWork CreateUnitOfWork(bool isTrans = false);

        Task<TEntity> GetAsync<TEntity, TKeyType>(TKeyType id) where TEntity : BaseEntity;

        Task AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;

        Task<int> UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;

        Task<int> RemoveByIdAsync<TEntity, TKeyType>(TKeyType id) where TEntity : BaseEntity;
    }
}
