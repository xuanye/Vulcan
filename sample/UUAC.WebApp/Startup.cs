using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using UUAC.Common;
using UUAC.WebApp.Libs;
using Vulcan.AspNetCoreMvc;
using Vulcan.DataAccess;

namespace UUAC.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment;

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

                config.Conventions.Add(new FeatureConvention());
            })
            .AddRazorOptions(options =>
            {
                // 支持默认的方式
                // {0} - Action Name
                // {1} - Controller Name
                // {2} - Area Name
                // {3} - Feature Name
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Areas/{2}/Features/{3}/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/{2}/Features/{3}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/{2}/Features/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/Shared/{0}.cshtml");

                // 支持Features文件夹组织代码的方式
                // replace normal view location entirely
                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("/Features/{3}/{1}/{0}.cshtml");
                options.ViewLocationFormats.Add("/Features/{3}/{0}.cshtml");
                options.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");

                // add support for features side-by-side with /Views
                // (do NOT clear ViewLocationFormats)
                //options.ViewLocationFormats.Insert(0, "/Features/Shared/{0}.cshtml");
                //options.ViewLocationFormats.Insert(0, "/Features/{3}/{0}.cshtml");
                //options.ViewLocationFormats.Insert(0, "/Features/{3}/{1}/{0}.cshtml");
                //
                // (do NOT clear AreaViewLocationFormats)
                //options.AreaViewLocationFormats.Insert(0, "/Areas/{2}/Features/Shared/{0}.cshtml");
                //options.AreaViewLocationFormats.Insert(0, "/Areas/{2}/Features/{3}/{0}.cshtml");
                //options.AreaViewLocationFormats.Insert(0, "/Areas/{2}/Features/{3}/{1}/{0}.cshtml");

                options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
            });

            // Add memory cache services
            services.AddMemoryCache();
            if (HostingEnvironment.IsDevelopment())
            {
                // 分布式缓存本地实现，开发模式使用，
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddDistributedRedisCache(option =>
                {
                    option.Configuration = "127.0.0.1";
                    option.InstanceName = "UUAC:";
                });
            }
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => {             
                options.Cookie.Name = ".UUACAPP";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Home/Login";
            });


            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);            
            });

            //获取数据库连接
            services.Configure<DBOption>(Configuration.GetSection("connectionStrings"));

            UUAC.DataAccess.Mysql.RepositoryRegistry.Registry(services);// 注册DB接口
            UUAC.Business.ServiceRegistry.Registry(services); //注册服务接口
            LocalRegistry.Registry(services);//注册本地服务

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRuntimeContextStorage, AspNetCoreContext>();
            services.AddSingleton<IConnectionFactory, MySqlConnectionFactory>();


            //链接管理器
            services.AddSingleton<IConnectionManagerFactory, ConnectionManagerFactory>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
         
            app.UseSession();

            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                //开发模式下使用模拟用户
                app.UseMockAppUser(
                new MockAppUserOption()
                {
                    MockUserId = Configuration["DebuggerUserId"]
                });
            }
            app.UseAuthentication();
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
