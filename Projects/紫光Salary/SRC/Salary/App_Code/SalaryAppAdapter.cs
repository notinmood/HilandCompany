using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary.Core.Data;
using Salary.Biz;

/// <summary>
///SalaryAppParameter 的摘要说明
/// </summary>
public class SalaryAppAdapter
{
    public SalaryAppAdapter()
        {
            _DataHelper = new DataHelper(_DBName);
        }
        private DataHelper _DataHelper = null;
        private String _DBName = SalaryConst.SalaryDBServer;
        private static SalaryAppAdapter _Instance = new SalaryAppAdapter();
        public static SalaryAppAdapter Instance
        {
            get
            {
                return _Instance;
            }
        }

    public String AddSpaceInFrontOfFeeName(String feeCode)
    {
        feeCode = CommonTools.Instance.MatchNumber(feeCode)[0];
        String addStr = "";
        Int32 len = feeCode.Length;
        addStr = len > 3 ? len > 5 ? len > 7 ? "　　　" : "　　" : "　" : "";
        addStr += addStr.Length>0?"-":"";
        return addStr;
    }
    public String AddSpaceInFrontOfDeptName(String deptCode)
    {
        deptCode = CommonTools.Instance.MatchNumber(deptCode)[0];
        String addStr = "";
        Int32 len = deptCode.Length;
        addStr = len > 2 ? len > 4 ? len > 6 ? "　　　" : "　　" : "　" : "";
        addStr += addStr.Length > 0 ? "-" : "";
        return addStr;
    }
    public String AddSpaceInFrontOfProjectName(String projectCode)
    {
        projectCode = CommonTools.Instance.MatchNumber(projectCode)[0];
        String addStr = "";
        Int32 len = projectCode.Length;
        addStr = len > 2 ? len > 4 ? len > 6 ? "　　　" : "　　" : "　" : "";
        addStr += addStr.Length > 0 ? "-" : "";
        return addStr;
    }
    public String AddSpaceInFrontOfParameterName(String parameterCode)
    {
        parameterCode = CommonTools.Instance.MatchNumber(parameterCode)[0];
        String addStr = "";
        Int32 len = parameterCode.Length;
        addStr = len > 3 ? len > 5 ? len > 7 ? "　　　" : "　　" : "　" : "";
        addStr += addStr.Length > 0 ? "-" : "";
        return addStr;
    }
}