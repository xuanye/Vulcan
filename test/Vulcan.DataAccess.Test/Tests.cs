using System;
using Microsoft.AspNetCore.Http;
using Vulcan.DataAccess.Context;
using Xunit;

namespace Vulcan.DataAccess.Test
{
    public class Tests
    {
        [Fact]
        public void TestAppRuntimeContext() 
        {
            // 配置模拟的上下文组件
            AppRuntimeContext.Configure(new MockHttpContextAccessor(new DefaultHttpContext()));

            const string key = "TEST_FOR_KEY";
            bool checkContains= AppRuntimeContext.Contains(key);

            Assert.False(checkContains);

            AppRuntimeContext.SetItem(key, "1");
            checkContains = AppRuntimeContext.Contains(key);
            Assert.True(checkContains);

            var item = AppRuntimeContext.GetItem(key);

            Assert.NotNull(item);

            Assert.Equal("1", item.ToString());
          
        }
    }

    public class MockHttpContextAccessor : IHttpContextAccessor
    {
        public MockHttpContextAccessor(HttpContext httpContext)
        {
            this.HttpContext = httpContext;
        }
        public HttpContext HttpContext { get; set; }
    }
}
