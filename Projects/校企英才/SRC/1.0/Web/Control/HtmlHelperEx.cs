using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Enums;
using HiLand.Utility4.MVC;
using HiLand.Utility4.MVC.Controls;
using XQYC.Business.BLL;
using XQYC.Business.Entity;
using XQYC.Business.Enums;

namespace XQYC.Web.Control
{
    public static class HtmlHelperEx
    {
        #region 计算机逻辑控件
        /// <summary>
        /// 日期输入控件
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IHtmlString DateInput(System.Web.Mvc.HtmlHelper html, string name, string value, string datetimeFormat = "yyyy/mm/dd")
        {
            List<string> cssFiles = new List<string>() { UrlHelperEx.UrlHelper.Content("~/Content/jQuery.tools.dateinput.css") };
            List<string> javaScriptFiles = new List<string>() { /*UrlHelperEx.UrlHelper.Content("~/Scripts/jQuery.tools.min.js")*/ };
            string dateTimeOptions = string.Format("format:'{0}',selectors:true,yearRange:[-50,10]", datetimeFormat);
            dateTimeOptions = "{" + dateTimeOptions + "}";
            if (DateTimeHelper.Parse(value, DateFormats.YMD) == DateTimeHelper.Min)
            {
                value = string.Empty;
            }
            return html.HiDateTime(name).Value(value).StyleSheetFiles(cssFiles).JavaScriptFiles(javaScriptFiles).DateInputOptions(dateTimeOptions).Render();
        }
        #endregion

        #region 部门人员控件
        /// <summary>
        /// 部门下拉列表
        /// </summary>
        /// <param name="html"></param>
        /// <param name="controlName"></param>
        /// <param name="selectedDepartmentGuid"></param>
        /// <param name="isDisplayChoosenItem"></param>
        public static void XQYCDDLDepartment(System.Web.Mvc.HtmlHelper html, string controlName, string selectedDepartmentGuid = GuidHelper.EmptyString, bool isDisplayChoosenItem = false)
        {
            List<SelectListItem> itemList = XQYCItemsDepartment(selectedDepartmentGuid);

            if (isDisplayChoosenItem == true)
            {
                html.DropDownList(controlName, itemList);
            }
            else
            {
                html.DropDownList(controlName, itemList, "请选择...");
            }
        }

        /// <summary>
        /// 获取部门项集合的数据集
        /// </summary>
        /// <param name="selectedDepartmentGuid"></param>
        /// <returns></returns>
        public static List<SelectListItem> XQYCItemsDepartment(string selectedDepartmentGuid = GuidHelper.EmptyString)
        {
            List<SelectListItem> itemList = new List<SelectListItem>();
            List<BusinessDepartment> departmentList = BusinessDepartmentBLL.Instance.GetOrdedList(Logics.True, string.Empty);
            if (departmentList != null)
            {
                foreach (BusinessDepartment department in departmentList)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = StringHelper.Repeate(">", department.DepartmentLevel) + department.DepartmentNameShort;
                    item.Value = department.DepartmentGuid.ToString();
                    if (department.DepartmentGuid.ToString() == selectedDepartmentGuid)
                    {
                        item.Selected = true;
                    }

                    itemList.Add(item);
                }
            }

            return itemList;
        }

        /// <summary>
        /// 部门下的人员选择器
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IHtmlString XQYCEmployeeChooser(System.Web.Mvc.HtmlHelper html, string name, string text, string value)
        {
            List<string> scriptList = new List<string>();
            scriptList.Add(UrlHelperEx.UrlHelper.Content("~/Scripts/jquery-1.4.4.min.js"));
            scriptList.Add(UrlHelperEx.UrlHelper.Content("~/Content/ztree/js/jquery.ztree.all-3.3.min.js"));

            List<string> cssList = new List<string>();
            cssList.Add(UrlHelperEx.UrlHelper.Content("~/Content/ztree/css/zTreeStyle/zTreeStyle.css"));
            string employeeJson = GetDepartmentEmployeeNodesJson();
            return html.HiTreeSelect(name).JavaScriptFiles(scriptList).StyleSheetFiles(cssList).Text(text).Value(value).IsInPopupWindow(true).DataSelectType(DataSelectTypes.Radio).StaticDataNodes(employeeJson).Render();
        }

        /// <summary>
        /// 获取部门人员节点的json数据
        /// </summary>
        /// <returns></returns>
        private static string GetDepartmentEmployeeNodesJson()
        {
            string result = string.Empty;

            List<ZTreeNodeEntity> nodeList = new List<ZTreeNodeEntity>();

            List<BusinessDepartment> departmentList = BusinessDepartmentBLL.Instance.GetList(Logics.True, "", 0, "");
            foreach (BusinessDepartment item in departmentList)
            {
                ZTreeNodeEntity node = new ZTreeNodeEntity();
                node.id = item.DepartmentGuid.ToString();
                node.pId = item.DepartmentParentGuid.ToString();
                node.name = item.DepartmentNameShort;
                node.open = false;
                node.nocheck = true;

                nodeList.Add(node);
            }

            List<EmployeeEntity> employeeList = EmployeeBLL.Instance.GetListBySQL("SELECT 	EMP.*,CU.* FROM XQYCEmployee EMP LEFT JOIN CoreUser CU ON EMP.UserGuid= CU.UserGuid");
            foreach (EmployeeEntity item in employeeList)
            {
                ZTreeNodeEntity node = new ZTreeNodeEntity();
                node.id = item.UserGuid.ToString();
                node.pId = item.DepartmentGuid.ToString();
                node.name = item.UserNameCN;
                node.open = false;
                node.nocheck = false;

                nodeList.Add(node);
            }

            result = JsonHelper.Serialize(nodeList);

            return result;
        }
        #endregion

        #region 企业、企业合同控件
        /// <summary>
        /// 企业选择下拉列表
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="selectedValue"></param>
        /// <param name="enterpriseServiceType">企业选用服务的类型，具体在表GeneralBasicSetting中定义的SettingKey</param>
        /// <returns></returns>
        public static IHtmlString XQYCDDLEnterprise(System.Web.Mvc.HtmlHelper html, string name, string selectedValue, string enterpriseServiceType = "0")
        {
            int enterpriserServiceTypeValue = 0;
            enterpriserServiceTypeValue = Converter.ChangeType(enterpriseServiceType, -999);
            if (enterpriserServiceTypeValue == -999)
            {
                List<BasicSettingEntity> allServiceList = BasicSettingBLL.Instance.GetListByCategory("EnterpriseServiceType");
                for (int i = 0; i < allServiceList.Count; i++)
                {
                    BasicSettingEntity currentServiceItem = allServiceList[i];
                    if (currentServiceItem.SettingKey.ToLower() == enterpriseServiceType.ToLower())
                    {
                        enterpriserServiceTypeValue = Converter.ChangeType(currentServiceItem.SettingValue, -999);
                        break;
                    }
                }
            }

            List<SelectListItem> itemList = new List<SelectListItem>();
            List<EnterpriseEntity> allEnterpriseList = EnterpriseBLL.Instance.GetList(string.Format("CanUsable={0}", (int)Logics.True));
            List<EnterpriseEntity> selectedEnterpriseList = new List<EnterpriseEntity>();

            if (enterpriserServiceTypeValue == 0)
            {
                selectedEnterpriseList = allEnterpriseList;
            }

            if (enterpriserServiceTypeValue > 0)
            {
                List<EnterpriseServiceEntity> serviceList = EnterpriseServiceBLL.Instance.GetList(string.Format("EnterpriseServiceType={0} AND EnterpriseServiceStatus={1}", enterpriserServiceTypeValue, (int)Logics.True));
                for (int i = 0; i < serviceList.Count; i++)
                {
                    EnterpriseServiceEntity currentService = serviceList[i];
                    for (int j = 0; j < allEnterpriseList.Count; j++)
                    {
                        EnterpriseEntity currentEnterprise = allEnterpriseList[j];
                        if (currentEnterprise.EnterpriseGuid == currentService.EnterpriseGuid)
                        {
                            selectedEnterpriseList.Add(currentEnterprise);
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < selectedEnterpriseList.Count; i++)
            {
                EnterpriseEntity currentEnterprise = selectedEnterpriseList[i];

                SelectListItem listItem = new SelectListItem();
                listItem.Text = currentEnterprise.CompanyName;
                listItem.Value = currentEnterprise.EnterpriseGuid.ToString();
                if (currentEnterprise.EnterpriseGuid.ToString() == selectedValue)
                {
                    listItem.Selected = true;
                }

                itemList.Add(listItem);
            }

            return html.DropDownList(name, itemList, "请选择...");
        }

        public static IHtmlString XQYCCSDDLEnterpriseContract(System.Web.Mvc.HtmlHelper html, string name, string selectedValue, string enterpriseControlID)
        {
            string dynamicItemsLoadUrl = UrlHelperEx.UrlHelper.Action("EnterpriseContractItemList", "FreePermission", new { enterpriseControlID = enterpriseControlID });
            return html.HiCascadingDropDownList(name).ParentSelectControlSelector("#" + enterpriseControlID).DynamicSelectItemsLoadUrl(dynamicItemsLoadUrl).Value(selectedValue).Render();
        }

        public static IHtmlString XQYCAutoCompleteEnterprise(System.Web.Mvc.HtmlHelper html, string name, string value = StringHelper.Empty, string realValue = StringHelper.Empty)
        {
            string autoCompleteUrl = UrlHelperEx.UrlHelper.Action("AutoCompleteData", "Enterprise");
            return html.HiTextBox(name).DynamicLoadDataUrl(autoCompleteUrl).Value(value).HiddenFieldValue(realValue).Render();
        }
        #endregion

        #region 信息员控件
        public static IHtmlString XQYCDDLInformationBroker(System.Web.Mvc.HtmlHelper html, string name, string value)
        {
            List<SelectListItem> itemList = new List<SelectListItem>();
            List<InformationBrokerEntity> brokerList = InformationBrokerBLL.Instance.GetList(string.Format("InformationBrokerStatus={0}", (int)UserStatuses.Normal));
            foreach (InformationBrokerEntity currentItem in brokerList)
            {
                SelectListItem listItem = new SelectListItem();
                listItem.Text = currentItem.UserNameCN;
                listItem.Value = currentItem.UserGuid.ToString();
                if (currentItem.UserGuid.ToString() == value)
                {
                    listItem.Selected = true;
                }

                itemList.Add(listItem);
            }

            string hiddenName = string.Format("{0}_Text", name);
            StringBuilder sb = new StringBuilder();
            sb.Append(html.DropDownList(name, itemList, "请选择...").ToHtmlString());
            sb.AppendFormat("<input id=\"{0}\" name=\"{0}\" type=\"hidden\" />", hiddenName);
            sb.AppendFormat(@"<script type='text/javascript'>
                $(document).ready(function () |<|
                    $('#{0}').change(function () |<|
                        var valueSelected = $(this).children('option:selected').val();
                        var textSelected = '';
                        if (valueSelected != '') |<|
                            textSelected = $(this).children('option:selected').text();
                        |>|
                        $('#{1}').val(textSelected);
                    |>|);
                |>|);
                </script>
            ", name, hiddenName);

            MvcHtmlString result = new MvcHtmlString(sb.Replace("|<|", "{").Replace("|>|", "}").ToString());
            return result;
        }
        #endregion

        #region 各种费用公式列表控件
        /// <summary>
        /// 保险下拉列表
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IHtmlString XQYCDDLInsurance(System.Web.Mvc.HtmlHelper html, string name, string value)
        {
            return XQYCDDLCostFormular(html, CostKinds.Insurance, name, value);
        }

        /// <summary>
        /// 管理费下拉列表
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IHtmlString XQYCDDLManageFee(System.Web.Mvc.HtmlHelper html, string name, string value)
        {
            return XQYCDDLCostFormular(html, CostKinds.ManageFee, name, value);
        }

        /// <summary>
        /// 公积金下拉列表
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IHtmlString XQYCDDLReserveFund(System.Web.Mvc.HtmlHelper html, string name, string value)
        {
            return XQYCDDLCostFormular(html, CostKinds.ReserveFund, name, value);
        }


        /// <summary>
        /// 通用的费用公式列表
        /// </summary>
        /// <param name="html"></param>
        /// <param name="costKind"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IHtmlString XQYCDDLCostFormular(System.Web.Mvc.HtmlHelper html, CostKinds costKind, string name, string value)
        {
            List<SelectListItem> itemList = new List<SelectListItem>();
            List<CostFormularEntity> entityList = CostFormularBLL.Instance.GetList(costKind, Logics.True, string.Empty);
            foreach (CostFormularEntity currentItem in entityList)
            {
                SelectListItem listItem = new SelectListItem();
                listItem.Text = currentItem.CostFormularName;
                listItem.Value = currentItem.CostFormularGuid.ToString();
                if (currentItem.CostFormularGuid.ToString() == value)
                {
                    listItem.Selected = true;
                }

                itemList.Add(listItem);
            }

            string hiddenName = string.Format("{0}_Text", name);
            StringBuilder sb = new StringBuilder();
            sb.Append(html.DropDownList(name, itemList, "请选择...").ToHtmlString());
            sb.AppendFormat("<input id=\"{0}\" name=\"{0}\" type=\"hidden\" />", hiddenName);
            sb.AppendFormat(@"<script type='text/javascript'>
                $(document).ready(function () |<|
                    $('#{0}').change(function () |<|
                        var valueSelected = $(this).children('option:selected').val();
                        var textSelected = '';
                        if (valueSelected != '') |<|
                            textSelected = $(this).children('option:selected').text();
                        |>|
                        $('#{1}').val(textSelected);
                    |>|);
                |>|);
                </script>
            ", name, hiddenName);

            MvcHtmlString result = new MvcHtmlString(sb.Replace("|<|", "{").Replace("|>|", "}").ToString());
            return result;
        }
        #endregion
    }
}