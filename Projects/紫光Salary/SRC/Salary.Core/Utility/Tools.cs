using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using ChinaCustoms.Framework.DeluxeWorks.Library.Core;

namespace Salary.Core.Utility
{
    public static class Tools
    {
        /// <summary>
        /// 将源对象的属性值一一复制到新对象中。
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDest">目标类型</typeparam>
        /// <param name="source">源类型实例</param>
        /// <param name="dest">目标类型实例</param>
        /// <returns>目标类型实例</returns>
        public static TDest ClassDataCopy<TSource, TDest>(TSource source) where TDest : class, new()
        {
            TDest dest = new TDest();
            if (source == null)
            {
                return null;
            }
            int PropertyCount = 0;
            typeof(TSource).GetProperties().Join(typeof(TDest).GetProperties(),
                   sourceProperty => sourceProperty.Name,
                   destProperty => destProperty.Name,
                   (sourceProperty, destProperty) => new { sourceProperty = sourceProperty, destProperty = destProperty }).ToList().
                   ForEach(fieldInfo =>
                   {
                       PropertyCount++;
                       try
                       {
                           fieldInfo.destProperty.SetValue(dest, fieldInfo.sourceProperty.GetValue(source, null), null);
                       }
                       catch
                       {
                       }
                   });
            if (PropertyCount < 1) throw new Exception("GoodsTools.ClassDataCopy()：一个属性都没有转换成功,可能有问题。");
            return dest;
        }

        /// <summary>
        /// 验证符合接口范围的实例是否是‘空’状态。
        /// </summary>
        /// <typeparam name="T">范类型。</typeparam>
        /// <param name="collection">集合实例。</param>
        /// <returns>如果实例是 null 或者实例的 Count 是零则返回 true</returns>
        public static bool CollectionIsEmpty<T>(ICollection<T> collection)
        {
            return (collection == null || collection.Count == 0);
        }

        public static void DropDownListDataBindByEnum(DropDownList ddl, Type enumType, int defaultValue, bool includeSelectionToolTip, bool equalsEnumName)
        {
            if (enumType.IsEnum)
            {
                ddl.Items.Clear();
                if (includeSelectionToolTip)
                {
                    ddl.Items.Add(new ListItem("----请选择----", string.Empty));
                }

                bool existSelectedItem = false;
                string destnationEnumName = Enum.GetName(enumType, defaultValue);

                EnumItemDescriptionList list = EnumItemDescriptionAttribute.GetDescriptionList(enumType);
                foreach (EnumItemDescription desc in list)
                {
                    ListItem item = new ListItem();
                    String descName = desc.Description;
                    item.Text = descName;
                    int descValue = desc.EnumValue;
                    item.Value = descValue.ToString();
                    if (!existSelectedItem)
                    {
                        existSelectedItem = item.Selected = (
                             (equalsEnumName && descName == destnationEnumName)
                         || (!equalsEnumName && descValue == defaultValue)
                         );
                    }

                    ddl.Items.Add(item);
                }
            }
        }

        public static void DropDownListDataBindByEnum(DropDownList ddl, Type enumType, string defaultValue, bool includeAll, bool isUseValue)
        {
            if (enumType.IsEnum)
            {
                ddl.Items.Clear();

                if (includeAll)
                {
                    ddl.Items.Add(new ListItem("--请选择--", string.Empty));
                }

                EnumItemDescriptionList list = EnumItemDescriptionAttribute.GetDescriptionList(enumType);
                foreach (EnumItemDescription desc in list)
                {
                    ListItem item = new ListItem();
                    item.Text = desc.Description;
                    item.Value = isUseValue ? desc.EnumValue.ToString() : desc.Name;
                    if (item.Value == defaultValue)
                    {
                        item.Selected = true;
                    }
                    ddl.Items.Add(item);
                }
            }
        }

        public static void DropDownListDataBind(DropDownList ddl, object list, string defaultValue, bool includeAll, string dataTextField, string dataValueField)
        {
            ddl.Items.Clear();
            ddl.DataTextField = dataTextField;
            ddl.DataValueField = dataValueField;
            ddl.DataSource = list;
            ddl.DataBind();
            if (includeAll)
            {
                ddl.Items.Insert(0, new ListItem("--请选择--", string.Empty));
            }
            ListItem item = ddl.Items.FindByValue(defaultValue);
            if (item != null) item.Selected = true;
        }

        public static void ListBoxDataBind(ListBox litb, object list, string dataTextField, string dataValueField)
        {
            litb.Items.Clear();
            litb.DataTextField = dataTextField;
            litb.DataValueField = dataValueField;
            litb.DataSource = list;
            litb.DataBind();
        }
        public static void ListBoxDataBindByEnum(ListBox litb, Type enumType, bool isUseValue)
        {
            if (enumType.IsEnum)
            {
                litb.Items.Clear();
                EnumItemDescriptionList list = EnumItemDescriptionAttribute.GetDescriptionList(enumType);
                foreach (EnumItemDescription desc in list)
                {
                    ListItem item = new ListItem();
                    item.Text = desc.Description;
                    item.Value = isUseValue ? desc.EnumValue.ToString() : desc.Name;
                    litb.Items.Add(item);
                }
            }
        }
    }
}
