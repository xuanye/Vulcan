using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UUAC.Common;
using UUAC.Entity;
using UUAC.Entity.DTOEntities;
using UUAC.Interface.Service;
using UUAC.WebApp.Libs;
using UUAC.WebApp.ViewModels;
using Vulcan.Core.Enities;

namespace UUAC.WebApp.Controllers
{
    public class AppController : MyControllerBase
    {
        private readonly IAppManageService _service;
        public AppController(IAppManageService service)
        {
            this._service = service;
        }
        // GET: /<controller>/
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Edit(string appCode)
        {
            IAppInfo model;
            if (string.IsNullOrEmpty(appCode))
            {
                model = new DtoAppInfo();
                
            }
            else
            {
                model = this._service.GetAppInfo(appCode);
            }
          
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveAppInfo(int type,DtoAppInfo entity)
        {
            JsonMsg msg = new JsonMsg();

            try
            {
                string errMsg;
                bool valid = ValidateAppInfo(entity, out errMsg);
                if (!valid)
                {
                    msg.status = -1;
                    msg.message = errMsg;
                    return Json(msg);
                }
                entity.LastModifyTime = DateTime.Now;
                entity.LastModifyUserId = base.UserId;
                entity.LastModifyUserName = base.UserId;

                int ret = await this._service.SaveAppInfo(entity, type);
                if (ret > 0)
                {
                    msg.status = 0;
                }
                else
                {
                    msg.status = -1;
                    msg.message = "操作不成功，请稍后重试";
                }
            }
            catch (Exception ex)
            {
                msg.status = -1;
                msg.message = "操作不成功："+ ex.Message;
            }
            return Json(msg);
        }

        private static bool ValidateAppInfo(DtoAppInfo entity, out string errMsg)
        {
            errMsg = "";
            if(entity == null)
            {
                errMsg = "应用信息为空，请刷新后重试";
                return false;
            }
            if(string.IsNullOrEmpty(entity.AppCode))
            {
                errMsg += "应用编码不能为空";
            }
            if (string.IsNullOrEmpty(entity.AppName))
            {
                errMsg += "应用名称不能为空";
            }
            return string.IsNullOrEmpty(errMsg);
        }

        [HttpPost]
        public async Task<IActionResult> QueryAppList(SearchAppInfoModel search)
        {
            JsonQTable ret = null;

            try
            {
                var list = await this._service.QueryAppInfoList(search.AppCode, search.AppName);

                ret =JsonQTable.ConvertFromList(list, search.colkey, search.colsArray);
            }
            catch(Exception ex)
            {
                ret = new JsonQTable {error = "操作失败:" + ex.Message};
            }
            return Json(ret);
        }


        [HttpPost]
        public async Task<IActionResult> Remove(string appCode)
        {
            JsonMsg msg = new JsonMsg();

            try
            {
                if(appCode ==Constans.APP_CODE)
                {
                    msg.status = -1;
                    msg.message = "内置系统不能为删除";
                }
                else
                {
                    int ret = await this._service.RemoveAppInfo(appCode);
                    if (ret > 0)
                    {
                        msg.status = 0;
                    }
                    else
                    {
                        msg.status = -1;
                        msg.message = "操作不成功，请稍后重试";
                    }
                }
               
            }
            catch (Exception ex)
            {
                msg.status = -1;
                msg.message = "操作不成功：" + ex.Message;
            }
            return Json(msg);
        }

        [HttpPost]
        public async Task<IActionResult> GetApps()
        {
            JsonMsg msg = new JsonMsg();

            try
            {
                var list = await this._service.QueryAppInfoList("", "");

                msg.status = 0;
                msg.data = list;

            }
            catch (Exception ex)
            {
                msg.status = -1;
                msg.message = "操作不成功：" + ex.Message;
            }
            return Json(msg);
        }

        [HttpPost]
        public async Task<IActionResult> GetMyApps()
        {
            JsonMsg msg = new JsonMsg();

            try
            {
                List<IAppInfo> list = await this._service.GetUserViewApp(base.UserId);

                msg.status = 0;
                msg.data = list;

            }
            catch (Exception ex)
            {
                msg.status = -1;
                msg.message = "操作不成功：" + ex.Message;
            }
            return Json(msg);
        }
    }
}
