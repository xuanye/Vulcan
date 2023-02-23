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

        Task<TEntity> GetAsync<TEntity, TKeyType>(TKeyType id) where TEntity : BaseEntity;

        Task<TKeyType> AddAsync<TEntity, TKeyType>(TEntity entity) where TEntity : BaseEntity;

        Task<TKeyType> UpdateAsync<TEntity, TKeyType>(TEntity entity) where TEntity : BaseEntity;

        Task<int> RemoveByIdAsync<TEntity, TKeyType>(TKeyType id) where TEntity : BaseEntity;
    }
}
