using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using UUAC.WebApp.Libs;
using UUAC.Common;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vulcan.DataAccess;
using Vulcan.DataAccess.Context;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;

namespace UUAC.WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            this.Evn = env;

        }

        public IHostingEnvironment Evn { get; }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            // Add memory cache services
            services.AddMemoryCache();
            if(Evn.IsDevelopment())
            {
                // 分布式缓存本地实现，开发模式使用，
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddSingleton<IDistributedCache, RedisCache>();
            }
            

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = ".UUACAPP";
            });



            ConnectionStringManager.Configure(Configuration.GetSection("ConnectionStrings"));


            UUAC.DataAccess.Mysql.RepositoryRegistry.Registry(services);// 注册DB接口
            UUAC.Business.ServiceRegistry.Registry(services); //注册服务接口
            LocalRegistry.Registry(services);//注册本地服务

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IConnectionFactory, MySqlConnectionFactory>();

            //
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            loggerFactory.AddNLog(); //添加NLog支持

            //获取注入的当前运行时上下文，用于临时存放数据库连接
            AppRuntimeContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

            //设置默认的数据库连接
            ConnectionFactoryHelper.Configure(app.ApplicationServices.GetRequiredService<IConnectionFactory>());

            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseSession();

            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = Constans.AuthenticationScheme,
                LoginPath = new PathString("/Home/Login/"),
                AccessDeniedPath = new PathString("/Home/Forbidden/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            
            if (env.IsDevelopment())
            {
                //开发模式下使用模拟用户
                app.UseMockAppUser(
                new MockAppUserOption()
                {
                    MockUserId = Configuration["DebuggerUserId"]
                });

            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   "ajax",                                           // Route name
                   "!ajax",                            // URL with parameters
                   new { controller = "Home", action = "Index" }  // Parameter defaults
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
