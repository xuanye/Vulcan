using System;
using System.Collections.Generic;
using System.Text;
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


        Task<TEntity> FindAsync<TEntity>(int id) where TEntity : BaseEntity;

        Task<List<TEntity>> FindAllAsync<TEntity>() where TEntity : BaseEntity;

        Task<List<TEntity>> FindByConditionAsync<TEntity>(IDictionary<string, string> conditions) where TEntity : BaseEntity;


        Task<int> InsertAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;

        Task<int> UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;


        Task RemoveByConditionAsync<TEntity>(Dictionary<string, string> conditions);

        Task RemoveByIdAsync<TEntity>(int id);

    }
}
