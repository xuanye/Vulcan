using System;
using System.Collections.Generic;
using Vulcan.DapperExtensions.ORMapping;


namespace Vulcan.DapperExtensionsUnitTests.MySQL
{

    [TableName("test_item_table")]
    public partial class MySQLTestItem : MySQLEntity
    {

        private int _Id;
        /// <summary>
        /// primary key		
        /// </summary>	
        [MapField("id"), Identity, PrimaryKey()]
        public int Id
        { get { return _Id; } set { _Id = value; OnPropertyChanged("id"); } }

        private string _Name;
        /// <summary>
        /// name		
        /// </summary>	
        [MapField("name"), Nullable]
        public string Name
        { get { return _Name; } set { _Name = value; OnPropertyChanged("name"); } }

        private string _MobilePhone;
        /// <summary>
        /// mobile phone		
        /// </summary>	
        [MapField("mobile_phone"), Nullable]
        public string MobilePhone
        { get { return _MobilePhone; } set { _MobilePhone = value; OnPropertyChanged("mobile_phone"); } }

        private string _Address;
        /// <summary>
        /// link address		
        /// </summary>	
        [MapField("address"), Nullable]
        public string Address
        { get { return _Address; } set { _Address = value; OnPropertyChanged("address"); } }

        private bool _IsDefault;
        /// <summary>
        /// test fo bool		
        /// </summary>	
        [MapField("is_default")]
        public bool IsDefault
        { get { return _IsDefault; } set { _IsDefault = value; OnPropertyChanged("is_default"); } }

        private sbyte _Status;
        /// <summary>
        /// test for enum 0= initial status 99=deleted		
        /// </summary>	
        [MapField("status")]
        public sbyte Status
        { get { return _Status; } set { _Status = value; OnPropertyChanged("status"); } }

        private string _CreatorId;
        /// <summary>
        /// nothing~		
        /// </summary>	
        [MapField("creator_id")]
        public string CreatorId
        { get { return _CreatorId; } set { _CreatorId = value; OnPropertyChanged("creator_id"); } }

        private DateTime _CreateTime;
        /// <summary>
        /// nothing~		
        /// </summary>	
        [MapField("create_time")]
        public DateTime CreateTime
        { get { return _CreateTime; } set { _CreateTime = value; OnPropertyChanged("create_time"); } }

        private string _ModifierId;
        /// <summary>
        /// nothing~		
        /// </summary>	
        [MapField("modifier_id")]
        public string ModifierId
        { get { return _ModifierId; } set { _ModifierId = value; OnPropertyChanged("modifier_id"); } }

        private DateTime _ModifyTime;
        /// <summary>
        /// nothing~		
        /// </summary>	
        [MapField("modify_time")]
        public DateTime ModifyTime
        { get { return _ModifyTime; } set { _ModifyTime = value; OnPropertyChanged("modify_time"); } }

    }

}