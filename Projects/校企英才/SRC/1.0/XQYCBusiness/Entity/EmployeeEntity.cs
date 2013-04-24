using System;
using HiLand.Framework.FoundationLayer;
using HiLand.Utility.Attributes;
using HiLand.Utility.Data;

namespace XQYC.Business.Entity
{
    /// <summary>
    /// 内部员工实体
    /// </summary>
    public class EmployeeEntity : BusinessUserEx<EmployeeEntity>
    {
        /*
            字段UserGuid跟[CoreUser]表中的字段UserGuid是对应的
         */
        #region 属性信息

        private int employeeID;
        public int EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        private string foo = String.Empty;
        public string Foo
        {
            get { return foo; }
            set { foo = value; }
        }
        #endregion

        #region 扩展属性
        /// <summary>
        /// 每个员工可以管理企业的最大数量
        /// </summary>
        [NonCopyMember]
        public int MaxEnterpriseCountOfManager
        {
            get
            {
                return Converter.ChangeType(((IModelExtensible)this).ExtensiableRepository.GetExtentibleProperty("MaxEnterpriseCountOfManager"), 0);
            }
            set
            {
                ((IModelExtensible)this).ExtensiableRepository.SetExtentibleProperty("MaxEnterpriseCountOfManager", value.ToString());
            }
        }

        #endregion
    }
}
