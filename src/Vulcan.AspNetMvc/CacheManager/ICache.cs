using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulcan.AspNetMvc.CacheManager
{
    public interface ICache
    {
        /// <summary>
        /// cache中的数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// cache的所有keys
        /// </summary>
        IList<string> Keys { get; }

        /// <summary>
        /// 根据key从缓存中获取数据，不存在返回 null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// 删除单个
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);


        /// <summary>
        /// 清除所有
        /// </summary>
        void Clear();

        /// <summary>
        /// 插入数据 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Insert(string key, object value);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeToLive"></param>
        void Insert(string key, object value, TimeSpan timeToLive);
    }
}
