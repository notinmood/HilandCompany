using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaCustoms.Framework.DeluxeWorks.Library.Core;


namespace Salary.Biz.Eunm
{
    /// <summary>
    /// 活动类型
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 新增
        /// </summary>
        [EnumItemDescription("新增")]
        Add = 1,
        /// <summary>
        /// 修改
        /// </summary>
        [EnumItemDescription("修改")]
        Edit = 2,
        /// <summary>
        /// 删除
        /// </summary>
        [EnumItemDescription("删除")]
        Delete = 3,
        /// <summary>
        /// 查看
        /// </summary>
        [EnumItemDescription("查看")]
        View = 9,
        /// <summary>
        /// 历史
        /// </summary>
        [EnumItemDescription("历史")]
        History = 4,
    }

    public enum Status
    {
        /// <summary>
        /// 正常
        /// </summary>
        [EnumItemDescription("正常")]
        True = 0,
        /// <summary>
        /// 停用
        /// </summary>
        [EnumItemDescription("停用")]
        False = 1,
    }

    public enum FeeType
    {
        /// <summary>
        /// 基本型
        /// </summary>
        [EnumItemDescription("组成")]
        Common = 1,
        /////// <summary>
        /////// 计提/待摊
        /////// </summary>
        ////[EnumItemDescription("计提/待摊")]
        ////JitiDaitan = 2,
        /////// <summary>
        /////// 基数
        /////// </summary>
        ////[EnumItemDescription("基数")]
        ////Base = 3,
        /////// <summary>
        /////// 系数
        /////// </summary>
        ////[EnumItemDescription("系数")]
        ////Coefficient = 4,
        /////// <summary>
        /////// 基数系数
        /////// </summary>
        ////[EnumItemDescription("基数系数")]
        ////BaseCoefficient = 5,
        /// <summary>
        /// 个税
        /// </summary>
        [EnumItemDescription("个税")]
        Tax = 6,
        /// <summary>
        /// 计算型
        /// </summary>
        [EnumItemDescription("计算")]
        Sum = 7,
        /// <summary>
        /// 参数型
        /// </summary>
        [EnumItemDescription("参数")]
        Parameter = 8,
    }

    public enum JitiDaitan
    {
        /// <summary>
        /// 计提/待摊
        /// </summary>
        [EnumItemDescription("计提/待摊")]
        JitiDaitan = 1,
        /// <summary>
        /// 计提
        /// </summary>
        [EnumItemDescription("计提")]
        JiTi = 2,
        /// <summary>
        /// 待摊
        /// </summary>
        [EnumItemDescription("待摊")]
        DaiTan = 3,
    }

    public enum CalculateSign
    {
        /// <summary>
        /// ＋
        /// </summary>
        [EnumItemDescription("+")]
        Add = 1,
        /// <summary>
        /// －
        /// </summary>
        [EnumItemDescription("-")]
        Minus = 2,
        /// <summary>
        /// ×
        /// </summary>
        [EnumItemDescription("*")]
        Multiply = 3,
        /// <summary>
        /// ÷
        /// </summary>
        [EnumItemDescription("/")]
        Divide = 4,
    }

    public enum UserType
    {
        /// <summary>
        /// 全权用户
        /// </summary>
        [EnumItemDescription("全权用户")]
        AdminUser = 0,
        /// <summary>
        /// 基础数据维护
        /// </summary>
        [EnumItemDescription("基础数据维护用户")]
        BaseUser = 1,
    }

    public enum PersonType
    {
        /// <summary>
        /// 全职
        /// </summary>
        [EnumItemDescription("全职")]
        QuanZhi = 0,
        /// <summary>
        /// 兼职
        /// </summary>
        [EnumItemDescription("兼职")]
        JianZhi = 1,
        /// <summary>
        /// 外聘
        /// </summary>
        [EnumItemDescription("外聘")]
        WaiPin = 2,
        /// <summary>
        /// 劳务
        /// </summary>
        [EnumItemDescription("劳务")]
        LaoWu = 5,
        /// <summary>
        /// 派出
        /// </summary>
        [EnumItemDescription("派出")]
        PaiChu = 3,
        /// <summary>
        /// 退休
        /// </summary>
        [EnumItemDescription("退休")]
        Retire = 4,
        /// <summary>
        /// 军人
        /// </summary>
        [EnumItemDescription("军人")]
        Soldier = 6,
    }

    public enum PersonBaseFeeTarget
    {
        /// <summary>
        /// PersonBaseFee
        /// </summary>
        [EnumItemDescription("PersonBaseFee")]
        PersonBaseFee = 1,
        /// <summary>
        /// PersonBaseFeeMonth
        /// </summary>
        [EnumItemDescription("PersonBaseFeeMonth")]
        PersonBaseFeeMonth = 2,
        /// <summary>
        /// PayMonth
        /// </summary>
        [EnumItemDescription("PayMonth")]
        PayMonth = 3,
    }

    public enum FeeFunction
    {
        /// <summary>
        /// 工资个税
        /// </summary>
        [EnumItemDescription("工资个税")]
        PersonalIncomeTax = 1,
        /// <summary>
        /// 劳务费个税
        /// </summary>
        [EnumItemDescription("劳务费个税")]
        ServiceFeeTax = 2,
        /// <summary>
        /// 公积金月缴
        /// </summary>
        [EnumItemDescription("公积金月缴")]
        GJJ = 3,
        /// <summary>
        /// 社会保险月缴
        /// </summary>
        [EnumItemDescription("社会保险基数")]
        SHBX = 4,
    }
    public enum CommonFeeType
    {
        /// <summary>
        /// 基本工资
        /// </summary>
        [EnumItemDescription("基本工资")]
        CommonSalary = 1,
        /// <summary>
        /// 合作
        /// </summary>
        [EnumItemDescription("合作")]
        Cooperate = 2,
        /// <summary>
        /// 福利
        /// </summary>
        [EnumItemDescription("福利")]
        Welfare = 3,
        /// <summary>
        /// 劳务
        /// </summary>
        [EnumItemDescription("劳务")]
        Service = 4,
        /// <summary>
        /// 发放
        /// </summary>
        [EnumItemDescription("发放")]
        Provide = 5,
        /// <summary>
        /// 岗位工资
        /// </summary>
        [EnumItemDescription("岗位工资")]
        Position = 6,
        /// <summary>
        /// 浮动工资
        /// </summary>
        [EnumItemDescription("浮动工资")]
        Float = 7,
        /// <summary>
        /// 虚拟工资
        /// </summary>
        [EnumItemDescription("虚拟工资")]
        Virtual = 8,
    }
}
