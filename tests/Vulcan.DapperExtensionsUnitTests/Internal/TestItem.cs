using System;
using Vulcan.DapperExtensions.ORMapping;

namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    [TableName("test_item")]
    public class TestItem : BaseEntity
    {
        private string _Address;

        private DateTime _CreateTime;

        private string _CreatorId;
        private int _Id;

        private bool _IsDefault;

        private string _MobilePhone;

        private string _ModifierId;

        private DateTime _ModifyTime;

        private string _Name;

#if MySqlDebug
        private sbyte _Status;
#else
        private byte _Status;
#endif

        /// <summary>
        ///     primary key
        /// </summary>
        [MapField("id")]
        [Identity]
        [PrimaryKey]
        public int Id
        {
            get => _Id;
            set
            {
                _Id = value;
                OnPropertyChanged("id");
            }
        }

        /// <summary>
        ///     name
        /// </summary>
        [MapField("name")]
        [Nullable]
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged("name");
            }
        }

        /// <summary>
        ///     mobile phone
        /// </summary>
        [MapField("mobile_phone")]
        [Nullable]
        public string MobilePhone
        {
            get => _MobilePhone;
            set
            {
                _MobilePhone = value;
                OnPropertyChanged("mobile_phone");
            }
        }

        /// <summary>
        ///     link address
        /// </summary>
        [MapField("address")]
        [Nullable]
        public string Address
        {
            get => _Address;
            set
            {
                _Address = value;
                OnPropertyChanged("address");
            }
        }

        /// <summary>
        ///     test fo bool
        /// </summary>
        [MapField("is_default")]
        public bool IsDefault
        {
            get => _IsDefault;
            set
            {
                _IsDefault = value;
                OnPropertyChanged("is_default");
            }
        }

        /// <summary>
        ///     test for enum 0= initial status 99=deleted
        /// </summary>
        [MapField("status")]
#if MySqlDebug
        public sbyte Status
#else
        public byte Status
#endif
        {
            get => _Status;
            set
            {
                _Status = value;
                OnPropertyChanged("status");
            }
        }

        /// <summary>
        ///     nothing~
        /// </summary>
        [MapField("creator_id")]
        public string CreatorId
        {
            get => _CreatorId;
            set
            {
                _CreatorId = value;
                OnPropertyChanged("creator_id");
            }
        }

        /// <summary>
        ///     nothing~
        /// </summary>
        [MapField("create_time")]
        public DateTime CreateTime
        {
            get => _CreateTime;
            set
            {
                _CreateTime = value;
                OnPropertyChanged("create_time");
            }
        }

        /// <summary>
        ///     nothing~
        /// </summary>
        [MapField("modifier_id")]
        public string ModifierId
        {
            get => _ModifierId;
            set
            {
                _ModifierId = value;
                OnPropertyChanged("modifier_id");
            }
        }

        /// <summary>
        ///     nothing~
        /// </summary>
        [MapField("modify_time")]
        public DateTime ModifyTime
        {
            get => _ModifyTime;
            set
            {
                _ModifyTime = value;
                OnPropertyChanged("modify_time");
            }
        }
    }
}
