using System;

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
    }
}
