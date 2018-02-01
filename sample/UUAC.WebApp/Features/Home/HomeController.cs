using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using UUAC.Common;
using UUAC.Interface.Service;
using UUAC.WebApp.Libs;
using UUAC.WebApp.ViewModels;
using Vulcan.Core.Enities;

namespace UUAC.WebApp.Features.Home
{
    public class HomeController : MyControllerBase
    {
        private readonly IPrivilegeService _pservice;
        private readonly IDistributedCache _cache;
        private readonly IUserManageService _uservice;

        public HomeController(IPrivilegeService pservice, IUserManageService uservice, IDistributedCache cache)
        {
            this._pservice = pservice;
            this._cache = cache;
            this._uservice = uservice;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeModel
            {
                Menus = await _pservice.QueryUserPrivilegeList(Constans.APP_CODE, base.UserId, 1)
            };

            ViewBag.UserId = base.UserId;

            var user = await base.GetSignedUser();

            ViewBag.FullName = user.FullName;
            //只获取菜单
            return View(model);
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (string.IsNullOrEmpty(ReturnUrl))
                {
                    ReturnUrl = "/";
                }
                return Redirect(ReturnUrl);
            }

            return View("Login");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            JsonMsg msg = new JsonMsg();
            try
            {
                int state = await this._uservice.CheckLogin(model.UserId, model.Password);
                if(state == 0)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, model.UserId) };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    //重新登录后，重置角色和权限的缓存
                    var pCacheKey = Constans.APP_CODE + "_" + model.UserId + "_P";
                    var rCacheKey = Constans.APP_CODE + "_" + model.UserId + "_R";
                    await _cache.RemoveAsync(pCacheKey);
                    await _cache.RemoveAsync(rCacheKey);
                }
                else
                {
                    msg.status = -1;
                    msg.message = "用户名或密码错误";
                }

               
            }
            catch (Exception ex)
            {
                msg.status = -1;
                msg.message = "登录失败：" + ex.Message;
            }
            return Json(msg);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult ThrowError()
        {
            throw new Exception("尝试故意抛出一个异常");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
