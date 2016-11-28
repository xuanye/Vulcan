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
using Vulcan.AspNetCoreMvc.Interfaces;
using UUAC.Common;

namespace UUAC.WebApp.Controllers
{
    public class OrgController : MyControllerBase
    {
        const string rootId = "000000";
        private readonly IOrgManageService _service;
        private readonly IAppContextService _contextService;

        public OrgController(IOrgManageService service, IAppContextService contextService)
        {
            this._service = service;
            this._contextService = contextService;
        }
        // GET: /<controller>/
        public IActionResult List()
        {
            
            return View();
        }
        public IActionResult Edit(string orgCode,string pcode,string pname)
        {
            IOrganization model;
            if (string.IsNullOrEmpty(orgCode))
            {
                model = new DtoOrganization();
                if (string.IsNullOrEmpty(pcode))
                {
                    throw new ArgumentNullException("pcode", "请选择上层组织");
                }
                else
                {
                    model.ParentCode = pcode;
                    model.ParentName = pname;
                }
            }
            else
            {
                model = this._service.GetOrgInfo(orgCode);
                if(model == null)
                {
                    throw new ArgumentOutOfRangeException("orgCode", "不存在对应的组织");
                }
                if (string.IsNullOrEmpty(model.ParentCode))
                {
                    model.ParentCode = rootId;
                    model.ParentName = "根组织";
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> QueryOrgList(SearchOrgModel search)
        {
            if(search.pcode == rootId)
            {
                search.pcode = "";
            }
            if(string.IsNullOrEmpty(search.pcode)) // 根组织
            {
                //判断用户是否为超级管理员，如果是超级管理员则 获取所有的组织结构，否则获取当前用户的可见组织结构
                bool admin = this._contextService.IsInRole(base.UserId, Constans.SUPPER_ADMIN_ROLE);

                if (!admin)
                {                 
                    if (!string.IsNullOrEmpty(base.ViewRootCode))
                        search.pcode = base.ViewRootCode;
                }

            }

            List<IOrganization> list = await this._service.QueryOrgListByParentCode(search.pcode);
            var ret= JsonQTable.ConvertFromList(list, search.colkey, search.colsArray);
            return Json(ret);
        }

        [HttpPost]
        public async Task<IActionResult> QueryOrgTree([FromForm]string id)
        {
            if (string.IsNullOrEmpty(id)) // 根组织
            {
                //判断用户是否为超级管理员，如果是超级管理员则 获取所有的组织结构，否则获取当前用户的可见组织结构
                bool admin = this._contextService.IsInRole(base.UserId, Constans.SUPPER_ADMIN_ROLE);
                if (!admin)
                {
                    if (!string.IsNullOrEmpty(base.ViewRootCode))
                        id = base.ViewRootCode;
                }

            }         
              
            List<IOrganization> list = await this._service.QueryOrgTreeByParentCode(id==rootId?"":id);
            List<JsonTreeNode> rlist = new List<JsonTreeNode>();
            if (string.IsNullOrEmpty(id) || id == rootId)
            {

                JsonTreeNode root = new JsonTreeNode
                {
                    text = "根组织",
                    id = rootId,
                    value = rootId
                };
                BuildChildNodes(root, list);
                root.hasChildren = root.ChildNodes.Count > 0;
                root.complete = true;
                root.isexpand = true;
                rlist.Add(root);
            }
            else
            {
                ConvertListToTree(list, rlist);
            }

            return Json(rlist);
        }
        [HttpPost]
        public async Task<IActionResult> CheckOrgCode(string id, [FromForm]string OrgCode)
        {
            string ret = "";
            try
            {
                if(OrgCode == rootId)
                {
                    ret ="不能使用内置根编码";
                    return Content(ret);
                }
                if (!string.IsNullOrEmpty(OrgCode))
                {
                    bool valid =  await this._service.CheckOrgCode(id, OrgCode);
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

        [HttpPost]
        public async Task<IActionResult> SaveOrg(int type, DtoOrganization entity)
        {
            JsonMsg msg = new JsonMsg();

            try
            {
                string errMsg;
                bool valid = ValidateOrg(entity, out errMsg);
                if (!valid)
                {
                    msg.status = -1;
                    msg.message = errMsg;
                    return Json(msg);
                }
                entity.LastModifyTime = DateTime.Now;
                entity.LastModifyUserId = base.UserId;
                entity.LastModifyUserName = base.UserId;
                if(entity.ParentCode == rootId)
                {
                    entity.ParentCode = null;
                }
                int ret = await this._service.SaveOrgInfo(entity, type);
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
        public async Task<IActionResult> Remove(string orgCode)
        {
            JsonMsg msg = new JsonMsg();

            try
            {
                int ret = await this._service.RemoveOrgInfo(orgCode);
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

        /// <summary>
        /// 选择组织的页面，只能选择当前用户可以选择的组织
        /// </summary>
        /// <returns></returns>
        public IActionResult Choose()
        {
            // 1. 获取当前用户的根组织
            return View("ChooseOrg");
        }
        private bool ValidateOrg(DtoOrganization entity, out string errMsg)
        {
            errMsg = "";
            if (entity == null)
            {
                errMsg = "组织信息为空，请刷新后重试";
                return false;
            }
            if (string.IsNullOrEmpty(entity.OrgCode))
            {
                errMsg += "组织代码不能为空";
            }
            if (string.IsNullOrEmpty(entity.OrgName))
            {
                errMsg += "组织名称不能为空";
            }

            return string.IsNullOrEmpty(errMsg);
        }


        private static void BuildChildNodes(JsonTreeNode pNode , List<IOrganization> list)
        {
            List<IOrganization> clist = pNode.id == rootId ?
                list.FindAll(x => string.IsNullOrEmpty(x.ParentCode))
                :
                list.FindAll(x => x.ParentCode == pNode.id);

            if (clist.Count > 0)
            {
                ConvertListToTree(list, pNode.ChildNodes);
            }
        }

        private static void ConvertListToTree(List<IOrganization> list , List<JsonTreeNode> rlist)
        {
            foreach (IOrganization org in list)
            {
                JsonTreeNode node = new JsonTreeNode
                {
                    text = org.OrgName,
                    id = org.OrgCode,
                    value = org.OrgCode
                };


                BuildChildNodes(node, list);

                node.hasChildren = org.HasChild;
                node.complete = !org.HasChild;
                node.isexpand = false;
                rlist.Add(node);
            }
        }


    }
}
