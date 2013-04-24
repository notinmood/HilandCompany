using System;
using System.Collections.Generic;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.FoundationLayer;
using HiLand.Framework.FoundationLayer.Attributes;
using HiLand.Utility.Data;

namespace XQYC.Business.Entity
{
    /// <summary>
    /// 用户实体扩展
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class BusinessUserEx<TModel> : BusinessUser,IModel
        where TModel : BusinessUserEx<TModel>,new()
    {
        private static TModel empty = null;
        public static new TModel Empty
        {
            get
            {
                if (empty == null)
                {
                    empty = new TModel();
                    empty.isEmpty = true;
                }

                return empty;
            }
        }

        /// <summary>
        /// 实体克隆
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 对方法MemberwiseClone的简单暴漏
        /// </remarks>
        public new TModel Clone()
        {
            return this.MemberwiseClone() as TModel;
        }

        /// <summary>
        /// 获取实体的JSON表述
        /// </summary>
        /// <returns></returns>
        public string ToJSON()
        {
            return JsonHelper.Serialize(this);
        }

        private string[] businessKeyNames;
        /// <summary>
        /// 实体的业务主键（区别与数据库的物理主键）
        /// </summary>
        /// <remarks>
        /// 缺省为反射实现。但可以在具体派生类里面重写，以提高性能。
        /// </remarks>
        public virtual string[] BusinessKeyNames
        {
            get
            {
                if (businessKeyNames == null || businessKeyNames.Length == 0)
                {
                    List<string> businessKeyNameList = PropertyInfoWithDBFieldAttributeCollection.GetBusinessPrimaryKeyNames<TModel>();

                    if (businessKeyNameList != null)
                    {
                        businessKeyNames = businessKeyNameList.ToArray();
                    }
                }

                return businessKeyNames;
            }
        }

        private string[] businessKeyValues;
        /// <summary>
        /// 实体的业务主键的值
        /// </summary>
        /// <returns></returns>
        public virtual string[] BusinessKeyValues
        {
            get
            {
                if (businessKeyValues == null || businessKeyValues.Length == 0)
                {
                    List<string> businessKeyValueList = PropertyInfoWithDBFieldAttributeCollection.GetBusinessPrimaryKeyValues(this);

                    if (businessKeyValueList != null)
                    {
                        businessKeyValues = businessKeyValueList.ToArray();
                    }
                }

                return businessKeyValues;
            }
        }

        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName
        {
            get
            {
                Type type = typeof(TModel);
                return type.Name;
            }
        }

        /// <summary>
        /// 将当前实例强制设置为空对象（二次开发中请勿直接使用）
        /// </summary>
        public void ForceSetEmpty()
        {
            this.isEmpty = true;
        }
    }
}
