using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UUAC.Entity;
using UUAC.Entity.DTOEntities;
using UUAC.Interface.Service;
using UUAC.WebApp.Libs;
using UUAC.WebApp.ViewModels;
using Vulcan.Core.Enities;
using Vulcan.DataAccess;

namespace UUAC.WebApp.Features.Role
{
    public class RoleController : MyControllerBase
    {
        private readonly IAppManageService _appService;
        private readonly IRoleService _service;
        private readonly IPrivilegeService _pservice;
        public RoleController(IRoleService service,IPrivilegeService pservice, IAppManageService appService)
        {
            this._service = service;
            this._appService = appService;
            this._pservice = pservice;
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleUsers([FromForm]string roleCode, [FromForm]string userIds)
        {
            JsonMsg msg = new JsonMsg();

            try
            {
                string errMsg = "";

                if (string.IsNullOrEmpty(roleCode))
                {
                    errMsg += "权限标识不能为空；";
                }
                if (string.IsNullOrEmpty(userIds))
                {
                    errMsg += "用户不能为空；";
                }

                if (!string.IsNullOrEmpty(errMsg))
                {
                    msg.status = -1;
                    msg.message = errMsg;
                    return Json(msg);
                }

                int ret = await this._service.AddRoleUserBatch(roleCode, userIds);
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

        [HttpPost]
        public async Task<IActionResult> CheckCode(string id, string appCode, [FromForm]string PrivilegeCode)
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

        public async Task<IActionResult> Edit(string id, string appcode, string pcode, string pname)
        {
            IRoleInfo model;
            if (string.IsNullOrEmpty(id))
            {
                if (string.IsNullOrEmpty(appcode))
                {
                    throw new ArgumentNullException("appcode", "请先选择系统");
                }
                model = new DtoRole
                {
                    AppCode = appcode,
                    ParentCode = pcode,
                    ParentName = pname
                };
            }
            else
            {
                model = await _service.GetRole(id);
            }
            if (string.IsNullOrEmpty(model.ParentCode))
            {
                model.ParentName = "根角色";
            }
            return View(model);
        }

        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(SearchPrivilegeModel search)
        {
            if (search == null)
            {
                return Json(new JsonQTable() { error = "参数为空，请检查后重试" });
            }
            if (string.IsNullOrEmpty(search.appCode))
            {
                return Json(new JsonQTable() { error = "请先选择系统" });
            }
            List<IRoleInfo> list;
            if (string.IsNullOrEmpty(search.pCode))
            {
                list = await this._service.QueryUserTopRole(search.appCode, base.UserId);
            }
            else
            {
                list = await this._service.QueryRoleByParentCode(search.appCode, search.pCode);
            }

            var jsonData = JsonQTable.ConvertFromList(list, search.colkey, search.colsArray);

            return Json(jsonData);
        }

        [HttpPost]
        public async Task<IActionResult> QueryRoleUsers([FromForm] SearchRoleModel search)
        {
            JsonQTable ret;
            if (search == null || string.IsNullOrEmpty(search.roleCode))
            {
                ret = new JsonQTable();
                ret.error = "角色代码不能为空";
                return Json(ret);
            }
            PageView page = new PageView();
            page.PageIndex = search.page - 1;
            page.PageSize = search.rp;
            page.SortName = search.sortname;
            page.SortOrder = search.sortorder;
            PagedList<IUserInfo> list = await _service.QueryRoleUsers(search.roleCode, search.queryText, page);
            ret = JsonQTable.ConvertFromPagedList(list, search.colkey, search.colsArray);
            return Json(ret);
        }

        [HttpPost]
        public async Task<IActionResult> QueryTree([FromForm]string id, [FromForm]string value)
        {
            string appCode = value;
            string pCode = id == value ? "" : id;
            List<IRoleInfo> list;
            if (string.IsNullOrEmpty(pCode))
            {
                list = await this._service.QueryUserTopRole(appCode, base.UserId);
            }
            else
            {
                list = await this._service.QueryRoleByParentCode(appCode, pCode);
            }

            List<JsonTreeNode> nodeList = new List<JsonTreeNode>();

            foreach (IRoleInfo p in list)
            {
                JsonTreeNode node = new JsonTreeNode
                {
                    text = p.RoleName,
                    id = p.RoleCode,
                    value = value,
                    hasChildren = p.HasChild,
                    complete = false,
                    isexpand = false
                };
                nodeList.Add(node);
            }
            return Json(nodeList);
        }
        [HttpPost]
        public async Task<IActionResult> Remove(string id)
        {
            JsonMsg msg = new JsonMsg();

            try
            {
                int ret = await this._service.RemoveRole(id);
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

        [HttpPost]
        public async Task<IActionResult> RemoveRoleUser(string id, string roleCode)
        {
            JsonMsg msg = new JsonMsg();

            try
            {
                string userId = id;
                int ret = await this._service.RomveRoleUser(roleCode, userId);
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

        public IActionResult RoleUsers(string roleCode)
        {
            if (string.IsNullOrEmpty(roleCode))
            {
                throw new ArgumentNullException("角色代码不能为空");
            }
            ViewBag.RoleCode = roleCode;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(int type, DtoRole entity)
        {
            JsonMsg msg = new JsonMsg();

            try
            {
                string errMsg;
                bool valid = ValidateRole(entity, out errMsg);
                if (!valid)
                {
                    msg.status = -1;
                    msg.message = errMsg;
                    return Json(msg);
                }
                entity.LastModifyTime = DateTime.Now;
                entity.LastModifyUserId = base.UserId;
                entity.LastModifyUserName = base.UserId;

                int ret = await this._service.SaveRole(entity, type);
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


        public IActionResult Authorization(string id)
        {
            TempData["RoleCode"] = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryPrivilegeTree([FromForm]string appCode, [FromForm]string roleCode)
        {
            if (string.IsNullOrEmpty(appCode))
            {
                throw new ArgumentNullException("appCode 不能为空");
            }

            if (string.IsNullOrEmpty(roleCode))
            {
                throw new ArgumentNullException("roleCode 不能为空");
            }

            List<IPrivilege> list = await this._pservice.QueryPrivilegeList(appCode);
            List<string> rplist = await this._service.QueryRolePrivilegeList(appCode,roleCode);

           

            List<JsonTreeNode> nodeList = new List<JsonTreeNode>();

            if(list !=null && list.Count > 0)
            {
                buildTreeNode(list, "", rplist, nodeList);
                if(nodeList.Count == 1)
                {
                    nodeList[0].isexpand = true;
                }
            }

            return Json(nodeList);

        }

        [HttpPost]
        public async Task<IActionResult> SaveRolePrivileges([FromForm]string roleCode, [FromForm]string privilegeCodes)
        {
            JsonMsg msg = new JsonMsg();

            try
            {
                

                string errMsg =null;
                if (string.IsNullOrEmpty(roleCode))
                {
                    errMsg = "roleCode 不能为空";
                }

                if (!string.IsNullOrEmpty(errMsg))
                {
                    msg.status = -1;
                    msg.message = errMsg;
                    return Json(msg);
                }
                privilegeCodes = privilegeCodes ?? "";

                string[] arrCode = privilegeCodes.Split(",");

                List<IRolePrivilege> plist = new List<IRolePrivilege>();
                foreach(string code in arrCode)
                {
                    plist.Add(new DtoRolePrivilege() { RoleCode = roleCode, PrivilegeCode = code });
                }

                int ret = await this._service.SaveRolePrivileges(roleCode, plist);

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

        private static void buildTreeNode(List<IPrivilege> list,string pCode,List<string> pvlist, List<JsonTreeNode> pNode)
        {
            List<IPrivilege> clist = null;
            if (string.IsNullOrEmpty(pCode))
            {
                clist = list.FindAll(x => string.IsNullOrEmpty(x.ParentCode));
            }
            else
            {
                clist = list.FindAll(x => x.ParentCode == pCode);
            }

            foreach (IPrivilege p in clist)
            {
                JsonTreeNode node = new JsonTreeNode
                {
                    text = p.PrivilegeName,
                    id = p.PrivilegeCode,
                    value = p.PrivilegeCode,
                    classes = p.PrivilegeType == 1 ? "menu" : "privilege",
                    showcheck = true,
                    complete = true,
                    isexpand = false
                };
                bool check = pvlist.Exists(x => x == p.PrivilegeCode);
                node.checkstate = check ? (byte)1 : (byte)0;
                buildTreeNode(list, p.PrivilegeCode, pvlist, node.ChildNodes);
                node.hasChildren = node.ChildNodes.Count > 0;
                if(node.hasChildren && node.checkstate == 1)
                {
                    bool hasNoCheck= node.ChildNodes.Exists(x => x.checkstate == 0);
                    if (hasNoCheck)
                    {
                        node.checkstate = 2;
                    }
                }
                pNode.Add(node);
            }
        }
        private static bool ValidateRole(DtoRole entity, out string errMsg)
        {
            errMsg = "";
            if (entity == null)
            {
                errMsg = "用户数据为空，很检查后重试";
                return false;
            }

            if (string.IsNullOrEmpty(entity.RoleCode))
            {
                errMsg += "权限标识不能为空；";
            }

            if (string.IsNullOrEmpty(entity.RoleName))
            {
                errMsg += "权限姓名不能为空；";
            }

            if (string.IsNullOrEmpty(entity.AppCode))
            {
                errMsg += "系统标识不能为空；";
            }

            return string.IsNullOrEmpty(errMsg);
        }
    }
}
