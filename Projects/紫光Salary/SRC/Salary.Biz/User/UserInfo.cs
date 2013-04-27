using System;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using Salary.Biz.Eunm;

namespace Salary.Biz
{
    /// <summary>
    /// UserInfo 的摘要说明
    /// </summary>
    [Serializable()]
    [ORTableMapping("[USER]")]
    public class UserInfo
    {
        #region 基本信息属性

        /// <summary>
        /// 用户ID
        /// </summary>
        [ORFieldMapping("USER_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String UserId { set; get; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        [ORFieldMapping("USER_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String UserName { set; get; }
        /// <summary>
        /// 登录名
        /// </summary>
        [ORFieldMapping("LOGON_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String LogonName { set; get; }
        /// <summary>
        /// 密码
        /// </summary>
        [ORFieldMapping("PASSWORD")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String Password { set; get; }
        /// 注销
        /// </summary>
        [ORFieldMapping("LOGOUT")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public Int32 Logout { set; get; }
        /// <summary>
        /// 用户类型:/0全权用户，1基础数据维护
        /// </summary>
        [ORFieldMapping("USER_TYPE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public UserType UserType { set; get; }
        ///// <summary>
        ///// 电话
        ///// </summary>
        //[ORFieldMapping("PHONE")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String Phone { set; get; }
        ///// <summary>
        ///// 手机
        ///// </summary>
        //[ORFieldMapping("MOBILE")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String Mobile { set; get; }
        ///// <summary>
        ///// 部门ID
        ///// </summary>
        //[ORFieldMapping("DEPT_ID")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String DeptId { set; get; }
        ///// <summary>
        ///// 部门名称
        ///// </summary>
        //[ORFieldMapping("DEPT_NAME")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String DeptName { set; get; }
        ///// <summary>
        ///// 职务
        ///// </summary>
        //[ORFieldMapping("DUTIES")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String Duties { set; get; }
        ///// <summary>
        ///// 最后维护人ID
        ///// </summary>
        //[ORFieldMapping("EDIT_USER")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String EditUser { set; get; }
        ///// <summary>
        ///// 最后维护人姓名
        ///// </summary>
        //[ORFieldMapping("EDIT_USER_NAME")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String EditUserName { set; get; }
        ///// <summary>
        ///// 最后维护时间
        ///// </summary>
        //[ORFieldMapping("EDIT_DATE")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public DateTime EditDate { set; get; }
        ///// <summary>
        ///// 备注
        ///// </summary>
        //[ORFieldMapping("REMARK")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String Remark { set; get; }

        #endregion

        #region Other
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            UserInfo info = obj as UserInfo;

            if (info == null)
            {
                return false;
            }
            return UserId == info.UserId;
        }
        ///// <summary>
        ///// 获得HashCode
        ///// </summary>
        ///// <returns></returns>
        //public override int GetHashCode()
        //{
        //    if (DeptId == null)
        //    {
        //        return Int32.MinValue;
        //    }
        //    return UserId.GetHashCode();
        //}

        #endregion
    }
    /// <summary>
    /// User 的摘要说明
    /// </summary>
    [Serializable()]
    public class UserInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "[USER]";
        /// <summary>
        /// 用户ID
        /// </summary>
        public const String UserId = "USER_ID";
        /// <summary>
        /// 用户姓名
        /// </summary>
        public const String UserName = "USER_NAME";
        /// <summary>
        /// 登录名
        /// </summary>
        public const String LogonName = "LOGON_NAME";
        /// <summary>
        /// 密码
        /// </summary>
        public const String Password = "PASSWORD";
        /// <summary>
        /// 注销
        /// </summary>
        public const String Logout = "LOGOUT";
        /// <summary>
        /// 用户类型:/0全权用户，1基础数据维护
        /// </summary>
        public const String UserType = "USER_TYPE";
        ///// <summary>
        ///// 电话
        ///// </summary>
        //public const String Phone = "PHONE";
        ///// <summary>
        ///// 手机
        ///// </summary>
        //public const String Mobile = "MOBILE";
        ///// <summary>
        ///// 部门ID
        ///// </summary>
        //public const String DeptId = "DEPT_ID";
        ///// <summary>
        ///// 部门名称
        ///// </summary>
        //public const String DeptName = "DEPT_NAME";
        ///// <summary>
        ///// 职务
        ///// </summary>
        //public const String Duties = "DUTIES";
        ///// <summary>
        ///// 最后维护人ID
        ///// </summary>
        //public const String EditUser = "EDIT_USER";
        ///// <summary>
        ///// 最后维护人姓名
        ///// </summary>
        //public const String EditUserName = "EDIT_USER_NAME";
        ///// <summary>
        ///// 最后维护时间
        ///// </summary>
        //public const String EditDate = "EDIT_DATE";
        ///// <summary>
        ///// 备注
        ///// </summary>
        //public const String Remark = "REMARK";
    }

    [Serializable()]
    public class UserInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String ClassName = "UserInfo";
        /// <summary>
        /// 用户ID
        /// </summary>
        public const String UserId = "UserId";
        /// <summary>
        /// 用户姓名
        /// </summary>
        public const String UserName = "UserName";
        /// <summary>
        /// 登录名
        /// </summary>
        public const String LogonName = "LogonName";
        /// <summary>
        /// 密码
        /// </summary>
        public const String Password = "Password";
        /// <summary>
        /// 注销
        /// </summary>
        public const String Logout = "Logout";
        /// <summary>
        /// 用户类型:/0全权用户，1基础数据维护
        /// </summary>
        public const String UserType = "UserType";
        ///// <summary>
        ///// 电话
        ///// </summary>
        //public const String Phone = "Phone";
        ///// <summary>
        ///// 手机
        ///// </summary>
        //public const String Mobile = "Mobile";
        ///// <summary>
        ///// 部门ID
        ///// </summary>
        //public const String DeptId = "DeptId";
        ///// <summary>
        ///// 部门名称
        ///// </summary>
        //public const String DeptName = "DeptName";
        ///// <summary>
        ///// 职务
        ///// </summary>
        //public const String Duties = "Duties";
        ///// <summary>
        ///// 最后维护人ID
        ///// </summary>
        //public const String EditUser = "EditUser";
        ///// <summary>
        ///// 最后维护人姓名
        ///// </summary>
        //public const String EditUserName = "EditUserName";
        ///// <summary>
        ///// 最后维护时间
        ///// </summary>
        //public const String EditDate = "EditDate";
        ///// <summary>
        ///// 备注
        ///// </summary>
        //public const String Remark = "Remark";
    }

    /// <summary>
    /// UserInfo 的摘要说明
    /// </summary>
    [Serializable()]
    [ORTableMapping("[USER_LOG]")]
    public class UserLogInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [ORFieldMapping("USER_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String UserId { set; get; }
        /// <summary>
        /// 登录名
        /// </summary>
        [ORFieldMapping("LOGON_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String LogonName { set; get; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        [ORFieldMapping("INPUT_DATE", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public DateTime InputDate { set; get; }
    }
}