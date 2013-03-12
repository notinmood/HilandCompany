using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XQYC.Web.Models
{
    public class EmployeeScoreStatisticalEntity
    {
        public Guid EmployeeGuid { get; set; }
        public string  EmployeeName { get; set; }
        public Guid DepartmentGuid { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentFullName { get; set; }

        /// <summary>
        /// 作为业务员劳务部员工张某招聘的劳务人员数量
        /// </summary>
        public int LaborCountOfLaborBusiness { get; set; }

        /// <summary>
        /// 张某作为企业的信息提供人，统计出的劳务人员数量
        /// </summary>
        public int LaborCountOfEnterpriseProvide { get; set; }

        /// <summary>
        /// 作为企业的开发人员统计出的劳务人员数量
        /// </summary>
        public int LaborCountOfEnterpriseBusiness { get; set; }

        /// <summary>
        /// 作为劳务人员的客服人员，统计出的劳务人员数量
        /// </summary>
        public int LaborCountOfLaborService { get; set; }
    }
}