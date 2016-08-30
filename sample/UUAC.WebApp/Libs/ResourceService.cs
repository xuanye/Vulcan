using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Vulcan.AspNetCoreMvc.Interfaces;

namespace UUAC.WebApp.Libs
{
    public class ResourceService:IResourceService
    {
        private static readonly TimeSpan _expirationTime = new TimeSpan(0, 30, 0); //过期时间

        private readonly IMemoryCache _cache;
        public ResourceService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        public List<IResource> GetResourceByCode(string code)
        {
            return GetResourceByCode(code, false);
        }

        public List<IResource> GetResourceByCode(string code, bool hasAllOption)
        {
            return GetResourceByCode(code, "", hasAllOption);
        }

        public List<IResource> GetResourceByCode(string code, string parentCode, bool hasAllOption)
        {
            string key = "RESOURCE_" + code + "_" + parentCode + (hasAllOption ? "1" : "0");
            List<IResource> cachedObject;
            bool cached= _cache.TryGetValue<List<IResource>>(key, out cachedObject);
            if (cached)
            {
                return cachedObject;
            }
            List<IResource> list =null;
            switch (code)
            {
                case "ORG_TYPE":
                    list = new List<IResource>
                    {
                        new DefaultResource() {Code = "1", Name = "组级", Value = "1"},
                        new DefaultResource() {Code = "2", Name = "部门级", Value = "2"},
                        new DefaultResource() {Code = "3", Name = "中心级", Value = "3"},
                        new DefaultResource() {Code = "4", Name = "事业部级", Value = "4"},
                        new DefaultResource() {Code = "5", Name = "公司级", Value = "5"}
                    };
                    break;
                case "ACCOUNT_TYPE":
                    list = new List<IResource>
                    {
                        new DefaultResource() {Code = "0", Name = "自建/外部", Value = "0"},
                        new DefaultResource() {Code = "1", Name = "内部", Value = "1"},
                        new DefaultResource() {Code = "9", Name = "系统", Value = "9"}
                    };
                    break;
                case "PRIVILEGE_TYPE":
                    list = new List<IResource>
                    {
                        new DefaultResource() {Code = "1", Name = "菜单权限", Value = "1"},
                        new DefaultResource() {Code = "2", Name = "标识权限", Value = "2"}
                    };
                    break;
                case "GENDER":
                    list = new List<IResource>
                    {
                        new DefaultResource() {Code = "1", Name = "男", Value = "1"},
                        new DefaultResource() {Code = "0", Name = "女", Value = "0"},
                        new DefaultResource() {Code = "9", Name = "未知", Value = "9"},
                    
                    };
                    break;

            }
            if (list != null && hasAllOption)
            {
                list.Insert(0, new DefaultResource() { Code = null, Name = "请选择..", Value = null });
            }

            if (list != null)
            {
                _cache.Set(key, list, _expirationTime);
            }
        
            return list;
        }
    }

    public class DefaultResource :IResource
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
