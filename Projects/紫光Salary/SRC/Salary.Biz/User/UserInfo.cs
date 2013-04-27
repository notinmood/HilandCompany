using System;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using Salary.Biz.Eunm;

namespace Salary.Biz
{
    /// <summary>
    /// UserInfo ��ժҪ˵��
    /// </summary>
    [Serializable()]
    [ORTableMapping("[USER]")]
    public class UserInfo
    {
        #region ������Ϣ����

        /// <summary>
        /// �û�ID
        /// </summary>
        [ORFieldMapping("USER_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String UserId { set; get; }
        /// <summary>
        /// �û�����
        /// </summary>
        [ORFieldMapping("USER_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String UserName { set; get; }
        /// <summary>
        /// ��¼��
        /// </summary>
        [ORFieldMapping("LOGON_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String LogonName { set; get; }
        /// <summary>
        /// ����
        /// </summary>
        [ORFieldMapping("PASSWORD")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String Password { set; get; }
        /// ע��
        /// </summary>
        [ORFieldMapping("LOGOUT")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public Int32 Logout { set; get; }
        /// <summary>
        /// �û�����:/0ȫȨ�û���1��������ά��
        /// </summary>
        [ORFieldMapping("USER_TYPE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public UserType UserType { set; get; }
        ///// <summary>
        ///// �绰
        ///// </summary>
        //[ORFieldMapping("PHONE")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String Phone { set; get; }
        ///// <summary>
        ///// �ֻ�
        ///// </summary>
        //[ORFieldMapping("MOBILE")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String Mobile { set; get; }
        ///// <summary>
        ///// ����ID
        ///// </summary>
        //[ORFieldMapping("DEPT_ID")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String DeptId { set; get; }
        ///// <summary>
        ///// ��������
        ///// </summary>
        //[ORFieldMapping("DEPT_NAME")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String DeptName { set; get; }
        ///// <summary>
        ///// ְ��
        ///// </summary>
        //[ORFieldMapping("DUTIES")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String Duties { set; get; }
        ///// <summary>
        ///// ���ά����ID
        ///// </summary>
        //[ORFieldMapping("EDIT_USER")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String EditUser { set; get; }
        ///// <summary>
        ///// ���ά��������
        ///// </summary>
        //[ORFieldMapping("EDIT_USER_NAME")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String EditUserName { set; get; }
        ///// <summary>
        ///// ���ά��ʱ��
        ///// </summary>
        //[ORFieldMapping("EDIT_DATE")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public DateTime EditDate { set; get; }
        ///// <summary>
        ///// ��ע
        ///// </summary>
        //[ORFieldMapping("REMARK")]
        //[SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        //public String Remark { set; get; }

        #endregion

        #region Other
        /// <summary>
        /// �Ƿ����
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
        ///// ���HashCode
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
    /// User ��ժҪ˵��
    /// </summary>
    [Serializable()]
    public class UserInfoDBConst
    {
        /// <summary>
        /// ����
        /// </summary>
        public const String TableName = "[USER]";
        /// <summary>
        /// �û�ID
        /// </summary>
        public const String UserId = "USER_ID";
        /// <summary>
        /// �û�����
        /// </summary>
        public const String UserName = "USER_NAME";
        /// <summary>
        /// ��¼��
        /// </summary>
        public const String LogonName = "LOGON_NAME";
        /// <summary>
        /// ����
        /// </summary>
        public const String Password = "PASSWORD";
        /// <summary>
        /// ע��
        /// </summary>
        public const String Logout = "LOGOUT";
        /// <summary>
        /// �û�����:/0ȫȨ�û���1��������ά��
        /// </summary>
        public const String UserType = "USER_TYPE";
        ///// <summary>
        ///// �绰
        ///// </summary>
        //public const String Phone = "PHONE";
        ///// <summary>
        ///// �ֻ�
        ///// </summary>
        //public const String Mobile = "MOBILE";
        ///// <summary>
        ///// ����ID
        ///// </summary>
        //public const String DeptId = "DEPT_ID";
        ///// <summary>
        ///// ��������
        ///// </summary>
        //public const String DeptName = "DEPT_NAME";
        ///// <summary>
        ///// ְ��
        ///// </summary>
        //public const String Duties = "DUTIES";
        ///// <summary>
        ///// ���ά����ID
        ///// </summary>
        //public const String EditUser = "EDIT_USER";
        ///// <summary>
        ///// ���ά��������
        ///// </summary>
        //public const String EditUserName = "EDIT_USER_NAME";
        ///// <summary>
        ///// ���ά��ʱ��
        ///// </summary>
        //public const String EditDate = "EDIT_DATE";
        ///// <summary>
        ///// ��ע
        ///// </summary>
        //public const String Remark = "REMARK";
    }

    [Serializable()]
    public class UserInfoConst
    {
        /// <summary>
        /// ����
        /// </summary>
        public const String ClassName = "UserInfo";
        /// <summary>
        /// �û�ID
        /// </summary>
        public const String UserId = "UserId";
        /// <summary>
        /// �û�����
        /// </summary>
        public const String UserName = "UserName";
        /// <summary>
        /// ��¼��
        /// </summary>
        public const String LogonName = "LogonName";
        /// <summary>
        /// ����
        /// </summary>
        public const String Password = "Password";
        /// <summary>
        /// ע��
        /// </summary>
        public const String Logout = "Logout";
        /// <summary>
        /// �û�����:/0ȫȨ�û���1��������ά��
        /// </summary>
        public const String UserType = "UserType";
        ///// <summary>
        ///// �绰
        ///// </summary>
        //public const String Phone = "Phone";
        ///// <summary>
        ///// �ֻ�
        ///// </summary>
        //public const String Mobile = "Mobile";
        ///// <summary>
        ///// ����ID
        ///// </summary>
        //public const String DeptId = "DeptId";
        ///// <summary>
        ///// ��������
        ///// </summary>
        //public const String DeptName = "DeptName";
        ///// <summary>
        ///// ְ��
        ///// </summary>
        //public const String Duties = "Duties";
        ///// <summary>
        ///// ���ά����ID
        ///// </summary>
        //public const String EditUser = "EditUser";
        ///// <summary>
        ///// ���ά��������
        ///// </summary>
        //public const String EditUserName = "EditUserName";
        ///// <summary>
        ///// ���ά��ʱ��
        ///// </summary>
        //public const String EditDate = "EditDate";
        ///// <summary>
        ///// ��ע
        ///// </summary>
        //public const String Remark = "Remark";
    }

    /// <summary>
    /// UserInfo ��ժҪ˵��
    /// </summary>
    [Serializable()]
    [ORTableMapping("[USER_LOG]")]
    public class UserLogInfo
    {
        /// <summary>
        /// �û�ID
        /// </summary>
        [ORFieldMapping("USER_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String UserId { set; get; }
        /// <summary>
        /// ��¼��
        /// </summary>
        [ORFieldMapping("LOGON_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String LogonName { set; get; }
        /// <summary>
        /// �û�����
        /// </summary>
        [ORFieldMapping("INPUT_DATE", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public DateTime InputDate { set; get; }
    }
}