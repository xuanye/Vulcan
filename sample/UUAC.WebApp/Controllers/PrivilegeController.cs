using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UUAC.Entity;
using UUAC.Entity.DTOEntities;
using UUAC.Interface.Service;
using UUAC.WebApp.Libs;
using UUAC.WebApp.ViewModels;
using Vulcan.Core.Enities;


namespace UUAC.WebApp.Controllers
{
    public class PrivilegeController : MyControllerBase
    {

        private readonly IPrivilegeService _service;
        private readonly IAppManageService _appService;
        public PrivilegeController(IPrivilegeService service,IAppManageService appService)
        {
            this._service = service;
            this._appService = appService;
        }

        public IActionResult List()
        {
            return View();
        }

       

        [HttpPost]
        public async Task<IActionResult> QueryPrivilegeTree([FromForm]string id,[FromForm]string value)
        {

            string appCode = value;
            string pCode = id == value ? "" : id;

            List<IPrivilege> list = await this._service.QueryPrivilegeByParentCode(appCode, pCode);

            List<JsonTreeNode> nodeList = new List<JsonTreeNode>();

            foreach(IPrivilege p in list)
            {
                JsonTreeNode node = new JsonTreeNode
                {
                    text = p.PrivilegeName,
                    id = p.PrivilegeCode,
                    value = value,
                    classes = p.PrivilegeType == 1 ? "menu" : "privilege",
                    hasChildren = p.HasChild,
                    complete = false,
                    isexpand = false
                };
                nodeList.Add(node);
            }
            return Json(nodeList);

        }

        [HttpPost]
        public async Task<IActionResult> QueryList(SearchPrivilegeModel search)
        {

            if(search == null)
            {
                return Json(new JsonQTable() { error ="参数为空，请检查后重试"});
            }
            if (string.IsNullOrEmpty(search.appCode))
            {
                return Json(new JsonQTable() { error = "请先选择系统" });
            }

            List<IPrivilege> list = await this._service.QueryPrivilegeByParentCode(search.appCode, search.pCode);

            var jsonData = JsonQTable.ConvertFromList(list, search.colkey, search.colsArray);

            return Json(jsonData);
            
        }
        [HttpPost]
        public async Task<IActionResult> CheckCode(string id,string appCode, [FromForm]string PrivilegeCode)
        {
            string ret = "";
            try
            {
                if (!string.IsNullOrEmpty(PrivilegeCode))
                {
                    bool valid = await this._service.CheckCode(id, appCode, PrivilegeCode);
                    if (!valid)
                    {
                        ret = "代码已存在";
                    }
                }
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }
            return Content(ret);
        }

        public async Task<IActionResult> Edit(string id,string appcode,string pcode,string pname)
        {
            IPrivilege model;
            if (string.IsNullOrEmpty(id))
            {
                if (string.IsNullOrEmpty(appcode))
                {
                    throw new ArgumentNullException("appcode","请先选择系统");
                }
                model = new DtoPrivilege
                {
                    AppCode = appcode,
                    ParentCode = pcode,
                    ParentName = pname
                };

            }
            else
            {
                model = await _service.GetPrivilege(id);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(int type, DtoPrivilege entity)
        {
            JsonMsg msg = new JsonMsg();

            try
            {
                string errMsg;
                bool valid = ValidatePrivilege(entity, out errMsg);
                if (!valid)
                {
                    msg.status = -1;
                    msg.message = errMsg;
                    return Json(msg);
                }
                entity.LastModifyTime = DateTime.Now;
                entity.LastModifyUserId = base.UserId;
                entity.LastModifyUserName = base.UserId;

                int ret = await this._service.SavePrivilege(entity, type);
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
                msg.message = "操作不成功：" + ex.Message;
            }
            return Json(msg);
        }

        private static bool ValidatePrivilege(DtoPrivilege entity, out string errMsg)
        {
            errMsg = "";
            if (entity == null)
            {
                errMsg = "用户数据为空，很检查后重试";
                return false;
            }

            if (string.IsNullOrEmpty(entity.PrivilegeCode))
            {
                errMsg += "权限标识不能为空；";
            }

            if (string.IsNullOrEmpty(entity.PrivilegeName))
            {
                errMsg += "权限姓名不能为空；";
            }

            if (string.IsNullOrEmpty(entity.AppCode))
            {
                errMsg += "系统标识不能为空；";
            }


            return string.IsNullOrEmpty(errMsg);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string id)
        {
            JsonMsg msg = new JsonMsg();

            try
            {
                int ret = await this._service.RemovePrivilege(id);
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
                msg.message = "操作不成功：" + ex.Message;
            }
            return Json(msg);
        }

    }
}
