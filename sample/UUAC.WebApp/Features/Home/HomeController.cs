using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
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
        public HomeController(IPrivilegeService pservice)
        {
            this._pservice = pservice;
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
                //TODO:这里要加数据校验+密码验证
                //
                msg.status = 0;

                const string Issuer = "https://contoso.com";
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserId, ClaimValueTypes.String, Issuer),                 
                };


                var userIdentity = new ClaimsIdentity("SuperSecureLogin");
                userIdentity.AddClaims(claims);
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.Authentication.SignInAsync(Constans.AuthenticationScheme, userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                        IsPersistent = false,
                        AllowRefresh = false
                    });
            }
            catch(Exception ex)
            {
                msg.status = -1;
                msg.message = "登录失败：" + ex.Message;
            }
            return Json(msg);
        }
        public IActionResult Logout()
        {
            this.SignOut(Constans.AuthenticationScheme);
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
