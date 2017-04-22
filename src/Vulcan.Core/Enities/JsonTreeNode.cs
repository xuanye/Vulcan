using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulcan.Core.Enities
{
    public class JsonTreeNode
    {
        #region properties

        /// <summary>
        /// treenode的主键必须唯一
        /// </summary>
        /// <value>The id.</value>
        public string id { get; set; }

        /// <summary>
        /// treenode的显示文本
        /// </summary>
        /// <value>The text.</value>
        public string text { get; set; }

        /// <summary>
        /// 节点的值
        /// </summary>
        /// <value>The value.</value>
        public string value { get; set; }

        /// <summary>
        /// 是否显示checkbox，如果前端设置了配置 showcheck: true，这里设置为true也有用，否则始终不会显示checkbox
        /// 相反就算你前端设置了showcheck: true ，这个属性设置false那么前端这个节点也不会显示checkbox。
        /// </summary>
        /// <value><c>true</c> if showcheck; otherwise, <c>false</c>.</value>
        public bool showcheck { get; set; }

        /// <summary>
        /// 是否展开，一般用于父节点展开，同时再获取一下节点的数据。
        /// 当设置为true时应该保证他的下级数据已经加载。
        /// </summary>
        /// <value><c>true</c> if isexpand; otherwise, <c>false</c>.</value>
        public bool isexpand { get; set; }

        /// <summary>
        /// 选中的状态0,1,2 0为没有选中，1为选中，2为半选
        /// </summary>
        /// <value>The checkstate.</value>
        public byte checkstate { get; set; }

        /// <summary>
        /// 是否有子节点
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has children; otherwise, <c>false</c>.
        /// </value>
        public bool hasChildren { get; set; }

        /// <summary>
        /// 是否已经完成加载，如果这个节点设置为true则，展开节点时不会发起ajax请求。
        /// </summary>
        /// <value><c>true</c> if complete; otherwise, <c>false</c>.</value>
        public bool complete { get; set; }

        /// <summary>
        /// 节点的自定义样式，可以为节点设置特定的样式，如修改节点的图标
        /// </summary>
        /// <value>The classes.</value>
        public string classes { get; set; }

        /// <summary>
        /// 额外的数据
        /// </summary>
        /// <value>The data.</value>
        public Dictionary<string, string> data { get; set; }

        private List<JsonTreeNode> _ChildNodes;
        public List<JsonTreeNode> ChildNodes
        {
            get
            {
                if (_ChildNodes == null)
                {
                    _ChildNodes = new List<JsonTreeNode>();
                }
                return _ChildNodes;
            }
        }

        #endregion
    }
}
