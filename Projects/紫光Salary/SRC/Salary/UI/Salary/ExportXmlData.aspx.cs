using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
using Salary.Web.BasePage;
using Salary.Biz;

public partial class UI_Salary_ExportXmlData : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string yearMonth = DecodedQueryString["yearMonth"];
        string reportId = DecodedQueryString["reportId"];
        GetXmlData(yearMonth, reportId);
    }

    //导出到Excel数据产生xml
    protected void GetXmlData(String yearMonth, String reportId)
    {
        DataSet ds = CommonTools.ConvertDsGuidToName(PayMonthInfoAdapter.Instance.LoadPayMonthInfoDS(yearMonth, reportId));
        // 把xml对象发送给客户端
        Response.ContentType = "text/xml";

        Response.Write(XmlDocumentToString(GetXmlDoc(ds)));
        Response.End();
    }
    /// <summary>
    /// 在枚举Xml NodeList的时候回调的函数接口
    /// </summary>
    public delegate void DoNodeList(XmlNode nodeRoot, object oParam);

    #region GetXmlDoc   由DataSet生成XML ,可以解决字段中的NULL值在XML文档中不显示问题

    /// <summary>
    /// 把数据集合DataSet中的所有数据转换成XML文档对象返回，解决字段中的NULL值在XML文档中不显示问题
    /// </summary>
    /// <param name="dataSet">要被转换的标准DataSet数据</param>
    /// <returns>符合指定XML格式的XML文档对象</returns>
    /// <remarks>
    /// 把数据集合DataSet中的所有数据转换成XML文档对象返回，解决字段中的NULL值在XML文档中不显示问题。
    /// 最后返回的XML文档对象中包括了所有DataSet中的所有数据，如果该DataSet中包含有多个DataTable，
    /// 返回的结果中会把所有这些数据都包括在内。（即允许多个DataTable在其中）
    /// </remarks>
    /// <example>
    /// 返回值：
    /// <code>
    /// &lt;DataSet&gt;
    ///		&lt;table&gt;
    ///			&lt;colName&gt;columnValue1&lt;/colName&gt;
    ///			........
    ///		&lt;/table&gt;
    ///		&lt;table&gt;
    ///			&lt;colName&gt;columnValue2&lt;/colName&gt;
    ///			........
    ///		&lt;/table&gt;
    ///		.........
    ///		&lt;table1&gt;	////多个DataTable情况
    ///			&lt;colNameA&gt;colValue&lt;/colNameA&gt;
    ///			........
    ///		&lt;/table1&gt;
    ///		..........
    /// &lt;/DataSet&gt;
    /// </code>
    /// </example>
    public static XmlDocument GetXmlDoc(DataSet dataSet)
    {
        return GetXmlDoc(dataSet, true);//false
    }

    /// <summary>
    /// 把数据集合DataSet中的所有数据转换成XML文档对象返回，解决字段中的NULL值在XML文档中不显示问题
    /// </summary>
    /// <param name="dataSet">要被转换的标准DataSet数据</param>
    /// <param name="bCDataSection">返回时每个字段的值是否由CData括起来</param>
    /// <returns>符合指定XML格式的XML文档对象</returns>
    /// <remarks>
    /// 把数据集合DataSet中的所有数据转换成XML文档对象返回，解决字段中的NULL值在XML文档中不显示问题。
    /// 最后返回的XML文档对象中包括了所有DataSet中的所有数据，如果该DataSet中包含有多个DataTable，
    /// 返回的结果中会把所有这些数据都包括在内。（即允许多个DataTable在其中）
    /// </remarks>
    /// <example>
    /// 返回值：
    /// <code>
    ///  <![CDATA[
    ///  <DataSet>
    ///		<Table1>
    ///			<Field1>Value</Field1>
    ///			<Field2>Value</Field2>
    ///		</Table1>
    ///		<Table2>
    ///			<Field1>Value</Field1>
    ///			<Field2>Value</Field2>
    ///		</Table2>
    ///  </DataSet>
    ///  ]]>
    /// </code>
    /// </example>
    public static XmlDocument GetXmlDoc(DataSet dataSet, bool bCDataSection)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml("<DataSet/>");

        XmlElement root = xmlDoc.DocumentElement;

        foreach (DataTable dataTable in dataSet.Tables)
        {
            //取列名
            XmlNode xmlTableNodeCaption = AppendNode(root, dataTable.TableName);
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                string strColumnValue = DBValueToString(dataColumn.Caption);

                if (bCDataSection)
                    AppendCDataNode(xmlTableNodeCaption, dataColumn.ColumnName, strColumnValue);
                else
                    AppendNode(xmlTableNodeCaption, dataColumn.ColumnName, strColumnValue);
            }
            //
            foreach (DataRow dataRow in dataTable.Rows)
            {
                XmlNode xmlTableNode = AppendNode(root, dataTable.TableName);

                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    string strColumnValue = DBValueToString(dataRow[dataColumn]);

                    if (bCDataSection)
                        AppendCDataNode(xmlTableNode, dataColumn.ColumnName, strColumnValue);
                    else
                        AppendNode(xmlTableNode, dataColumn.ColumnName, strColumnValue);
                }
            }
        }

        return xmlDoc;
    }

    /// <summary>
    /// 把数据集合DataSet中的所有数据转换成XML文档对象返回，解决字段中的NULL值在XML文档中不显示问题，
    /// 结果放置在xmlDoc中
    /// </summary>
    /// <param name="xmlDoc">文档对象（保存函数的结果值）</param>
    /// <param name="dataSet">要求被转换的标准DataSet</param>
    /// <remarks>
    /// 把数据集合DataSet中的所有数据转换成XML文档对象返回，解决字段中的NULL值在XML文档中不显示问题，
    /// 结果放置在xmlDoc中。该函数调用了GetXmlDoc(DataSet dataSet)，取得的结果是它的InnerXml形成的XML文档对象
    /// </remarks>
    /// <example>
    /// 返回值：
    /// <code>
    /// &lt;DataSet&gt;
    ///		&lt;table&gt;
    ///			&lt;colName&gt;columnValue1&lt;/colName&gt;
    ///			........
    ///		&lt;/table&gt;
    ///		&lt;table&gt;
    ///			&lt;colName&gt;columnValue2&lt;/colName&gt;
    ///			........
    ///		&lt;/table&gt;
    ///		.........
    ///		&lt;table1&gt;	////多个DataTable情况
    ///			&lt;colNameA&gt;colValue&lt;/colNameA&gt;
    ///			........
    ///		&lt;/table1&gt;
    ///		..........
    /// &lt;/DataSet&gt;
    /// </code>
    /// </example>
    public static void GetXmlDoc(XmlDocument xmlDoc, DataSet dataSet)
    {
        xmlDoc.LoadXml(GetXmlDoc(dataSet).InnerXml);
    }

    #endregion

    #region GetXmlDoc 由IDataReader生成XML

    /// <summary>
    /// 将IDataReader中的数据转换成XML文档对象
    /// </summary>
    /// <param name="dataReader">标准的IDataReader数据集</param>
    /// <returns>符合由DataSet.GetXml()定义格式的XML文档对象</returns>
    /// <remarks>
    /// 将IDataReader中的数据转换成XML文档对象。该文档对象的结构要求符合一定的格式，其中的格式定义如下
    /// <code>
    /// &lt;DataSet&gt;
    ///		&lt;table&gt;
    ///			&lt;colName&gt;columnValue1&lt;/colName&gt;
    ///			........
    ///		&lt;/table&gt;
    ///		&lt;table&gt;
    ///			&lt;colName&gt;columnValue2&lt;/colName&gt;
    ///			........
    ///		&lt;/table&gt;
    ///		.........
    ///		&lt;table1&gt;	////多个DataTable情况
    ///			&lt;colNameA&gt;colValue&lt;/colNameA&gt;
    ///			........
    ///		&lt;/table1&gt;
    ///		..........
    /// &lt;/DataSet&gt;
    /// </code>
    /// 这里我们对多表（DataTable）的处理也是综合的处理，产生的XML文档对象结构是一种并列的形势，但是注意：
    /// 在不同的DataTable之间我们使用了不同的节点名称（如上实例中的Table与Table1）
    /// </remarks>
    public static XmlDocument GetXmlDoc(IDataReader dataReader)
    {
        return GetXmlDoc(dataReader, false);
    }

    /// <summary>
    /// 将IDataReader中的数据转换成XML文档对象
    /// </summary>
    /// <param name="dataReader">标准的IDataReader数据集</param>
    /// <param name="bCDataSection">返回时每个字段的值是否由CData括起来</param>
    /// <returns>符合由DataSet.GetXml()定义格式的XML文档对象</returns>
    /// <remarks>
    /// 将IDataReader中的数据转换成XML文档对象。该文档对象的结构要求符合一定的格式，其中的格式定义如下
    /// <code>
    ///  <![CDATA[
    ///  <DataSet>
    ///		<Table1>
    ///			<Field1>Value</Field1>
    ///			<Field2>Value</Field2>
    ///		</Table1>
    ///		<Table2>
    ///			<Field1>Value</Field1>
    ///			<Field2>Value</Field2>
    ///		</Table2>
    ///  </DataSet>
    ///  ]]>
    /// </code>
    /// 这里我们对多表（DataTable）的处理也是综合的处理，产生的XML文档对象结构是一种并列的形势，但是注意：
    /// 在不同的DataTable之间我们使用了不同的节点名称（如上实例中的Table与Table1）
    /// </remarks>
    public static XmlDocument GetXmlDoc(IDataReader dataReader, bool bCDataSection)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml("<DataSet/>");

        XmlElement root = xmlDoc.DocumentElement;

        int nColumn = dataReader.FieldCount;

        while (dataReader.Read())
        {
            XmlNode xmlTableNode = AppendNode(root, "Table");

            for (int n = 0; n < nColumn; n++)
            {
                string strColumnValue = DBValueToString(dataReader[n]);

                XmlNode xmlColumnNode = null;

                if (bCDataSection)
                    xmlColumnNode = AppendCDataNode(xmlTableNode, dataReader.GetName(n), strColumnValue);
                else
                    xmlColumnNode = AppendNode(xmlTableNode, dataReader.GetName(n), strColumnValue);
            }
        }

        return xmlDoc;
    }
    /// <summary>
    ///  将IDataReader中的数据转换成XML文档对象
    /// </summary>
    /// <param name="xmlDoc">用来存放新生成XML文档对象</param>
    /// <param name="dataReader">标准的DdataReader数据集</param>
    /// <remarks>
    /// 将IDataReader中的数据转换成XML文档对象。该文档对象的结构要求符合一定的格式，其中的格式定义如下
    /// <code>
    /// &lt;DataSet&gt;
    ///		&lt;table&gt;
    ///			&lt;colName&gt;columnValue1&lt;/colName&gt;
    ///			........
    ///		&lt;/table&gt;
    ///		&lt;table&gt;
    ///			&lt;colName&gt;columnValue2&lt;/colName&gt;
    ///			........
    ///		&lt;/table&gt;
    ///		.........
    ///		&lt;table1&gt;	////多个DataTable情况
    ///			&lt;colNameA&gt;colValue&lt;/colNameA&gt;
    ///			........
    ///		&lt;/table1&gt;
    ///		..........
    /// &lt;/DataSet&gt;
    /// </code>
    /// 这里我们对多表（DataTable）的处理也是综合的处理，产生的XML文档对象结构是一种并列的形势，但是注意：
    /// 在不同的DataTable之间我们使用了不同的节点名称（如上实例中的Table与Table1）。最后的结果将会在参数
    /// xmlDoc带回。
    /// </remarks>
    public static void GetXmlDoc(XmlDocument xmlDoc, IDataReader dataReader)
    {
        xmlDoc.LoadXml(GetXmlDoc(dataReader).InnerXml);
    }

    /// <summary>
    /// 将数据库字段值转换为字符串
    /// </summary>
    /// <param name="objValue"></param>
    /// <returns></returns>
    private static string DBValueToString(object objValue)
    {
        string strResult = string.Empty;

        if (objValue != null)
        {
            if (objValue is DateTime)
            {
                if ((DateTime)objValue != DateTime.MinValue && (DateTime)objValue != DateTime.MaxValue && (DateTime)objValue != new DateTime(1900, 1, 1, 0, 0, 0, 0))
                    strResult = string.Format("{0:yyyy-MM-dd HH:mm:ss}", objValue);
            }
            else
                strResult = objValue.ToString();
        }

        return strResult;
    }

    #endregion

    #region 根据Exception声称Xml文档对象

    /// <summary>
    /// 将异常对象生成一个XML文档对象
    /// </summary>
    /// <param name="ex">异常对象</param>
    /// <returns>表示异常对象的Xml文档</returns>
    public static XmlDocument GetExceptionXmlDoc(System.Exception ex)
    {
        XmlDocument xmlDoc = CreateDomDocument("<ResponseError/>");
        XmlElement root = xmlDoc.DocumentElement;

        AppendExceptionNode(root, ex);

        return xmlDoc;
    }

    private static void AppendExceptionNode(XmlNode root, System.Exception ex)
    {
        AppendNode(root, "Value", ex.Message);
        AppendNode(root, "Stack", ex.StackTrace);

        if (ex.InnerException != null)
        {
            XmlNode nodeInnerEx = AppendNode(root, "InnerException");

            AppendExceptionNode(root, ex.InnerException);
        }
    }
    #endregion

    #region 与XML文档相关的辅助函数

    /// <summary>
    /// 从某个磁盘文件加载XML文档对象。该方法和XmlDocument.Load的不同点在于它支持共享读，即使在别的程序正在写这个文件
    /// </summary>
    /// <param name="strPath">文件路径</param>
    /// <returns>XML文档对象</returns>
    public static XmlDocument LoadDocument(string strPath)
    {
        FileStream fs = File.Open(strPath, FileMode.Open, FileAccess.Read, FileShare.Read);

        try
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(fs);

            return xmlDoc;
        }
        finally
        {
            fs.Close();
        }
    }

    /// <summary>
    /// 建立一个XML文档对象，并且可以初始化文档（方便文档对象的创建和初始化）
    /// </summary>
    /// <param name="strXML">XML文档对象在初始化的时候要求的数据值</param>
    /// <returns>创建以后并且被初始化的XmlDocument对象</returns>
    /// <remarks>
    /// 建立一个XML文档对象，并且可以初始化文档（方便文档对象的创建和初始化）。该函数包含了XML文档对象
    /// 创建和初始化的两步工作，这样方便了编程人员的创建XML文档对象和初始化的工作。
    /// </remarks>
    /// <example>
    /// <code>
    /// XmlDocument xmlDoc = XMLHelper.CreateDomDocument("&lt;DataSet/&gt;");
    /// </code>
    /// </example>
    public static XmlDocument CreateDomDocument(string strXML)
    {
        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(strXML);

        return xmlDoc;
    }

    /// <summary>
    /// 在xml文档对象中由root指定的节点下面添加一个名称为strNodeName指定的子节点
    /// </summary>
    /// <param name="xmlDoc">XML文档对象</param>
    /// <param name="root">文档对象中的指定要添加子节点的根节点</param>
    /// <param name="strNodeName">要被添加的子节点名称</param>
    /// <returns>被添加的子节点对象</returns>
    /// <remarks>
    /// 在xml文档对象中由root指定的节点下面添加一个名称为strNodeName指定的子节点。该子节点的名称由
    /// strNodeName指定，它的内容（InnerXml）为空。
    /// </remarks>
    /// <example>
    /// <code>
    /// XmlDocument xmlDoc = CreateDomDocument("&lt;FIRST&gt;&lt;SECOND&gt;&lt;THIRD/&gt;&lt;/SECOND&gt;&lt;/FIRST&gt;");
    /// XmlNode root = xmlDoc.DocumentElement.FirstChild;
    /// XmlNode node = XMLHelper.AppendNode(xmlDoc, root, "FOURTH");
    /// </code>
    /// 结果为：
    /// <code>
    /// &lt;FIRST&gt;
    ///		&lt;SECOND&gt;
    ///			&lt;THIRD/&gt;
    ///			&lt;FOURTH/&gt;
    ///		&lt;/SECOND&gt;
    /// &lt;/FIRST&gt;
    /// </code>
    /// </example>
    public static XmlNode AppendNode(XmlNode root, string strNodeName)
    {
        XmlNode node = root.OwnerDocument.CreateNode(XmlNodeType.Element, strNodeName, "");

        root.AppendChild(node);

        return node;
    }

    /// <summary>
    /// 在xml文档对象中由root指定的节点下面添加一个名称为strNodeName指定的子节点，其中该节点的内容为strNodeText指定
    /// </summary>
    /// <param name="xmlDoc">XML文档对象</param>
    /// <param name="root">文档对象中的指定要添加子节点的根节点</param>
    /// <param name="strNodeName">要被添加的子节点名称</param>
    /// <param name="strNodeText">要被添加的子节点的内容</param>
    /// <returns>被添加的子节点对象</returns>
    /// <remarks>
    /// 在xml文档对象中由root指定的节点下面添加一个名称为strNodeName指定的子节点。该子节点的名称由
    /// strNodeName指定，它的内容由strNodeText指定。
    /// </remarks>
    /// <example>
    /// <code>
    /// XmlDocument xmlDoc = CreateDomDocument("&lt;FIRST&gt;&lt;SECOND&gt;&lt;THIRD/&gt;&lt;/SECOND&gt;&lt;/FIRST&gt;");
    /// XmlNode root = xmlDoc.DocumentElement.FirstChild;
    /// XmlNode node = XMLHelper.AppendNode(xmlDoc, root,, "FOURTH", "数据整理完成");
    /// </code>
    /// 返回的结果为：
    /// <code>
    /// &lt;FIRST&gt;
    ///		&lt;SECOND&gt;
    ///			&lt;THIRD/&gt;
    ///			&lt;FOURTH&gt;数据整理完成&lt;/FOURTH&gt;
    ///		&lt;/SECOND&gt;
    /// &lt;/FIRST&gt;
    /// </code>
    /// </example>
    public static XmlNode AppendNode(XmlNode root, string strNodeName, string strNodeText)
    {
        XmlNode node = AppendNode(root, strNodeName);

        node.InnerText = strNodeText;

        return node;
    }

    /// <summary>
    /// 在xml文档对象中由root指定的节点下面添加一个名称为strNodeName指定的子节点，该节点的内容由nodeSrc的子节点复制过来
    /// </summary>
    /// <param name="xmlDoc">XML文档对象</param>
    /// <param name="root">文档对象中的指定要添加子节点的根节点</param>
    /// /// <param name="strNodeName">要被添加的子节点名称</param>
    /// <param name="nodeSrc">被复制的源节点</param>
    /// <returns>被添加的子节点对象</returns>
    /// <remarks>
    /// 在xml文档对象中由root指定的节点下面添加一个名称为strNodeName指定的子节点。该子节点的名称由
    /// strNodeName指定，它的内容由strNodeText指定。
    /// </remarks>
    public static XmlNode AppendNode(XmlNode root, string strNodeName, XmlNode nodeSrc)
    {
        XmlNode node = AppendNode(root, strNodeName);

        node.InnerXml = nodeSrc.InnerXml;

        return node;
    }

    /// <summary>
    /// 在xml文档对象中由root指定的节点下面添加一个名称为strNodeName指定的子节点，其中该节点的内容为strNodeText指定。但是节点的内容会被CData Section包围起来
    /// </summary>
    /// <param name="xmlDoc">XML文档对象</param>
    /// <param name="root">文档对象中的指定要添加子节点的根节点</param>
    /// <param name="strNodeName">要被添加的子节点名称</param>
    /// <param name="strNodeText">要被添加的子节点的内容</param>
    /// <returns>被添加的子节点对象</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// XmlDocument xmlDoc = CreateDomDocument("<Document/>");
    /// 
    /// XmlNode root = xmlDoc.DocumentElement;
    /// 
    /// XmlNode node = AppendCDataNode(xmlDoc, root, "Data", "  Hello world !");
    ///
    /// 结果为：
    /// <Document>
    ///		<Data>
    ///			<![CDATA[  Hello world !]]/>
    ///		</Data>
    ///	</Document>
    /// ]]>
    /// </code>
    /// </example>
    public static XmlNode AppendCDataNode(XmlNode root, string strNodeName, string strNodeText)
    {
        XmlNode node = root.OwnerDocument.CreateNode(XmlNodeType.Element, strNodeName.Replace("(","").Replace(")",""), "");

        root.AppendChild(node);

        XmlCDataSection cdata = root.OwnerDocument.CreateCDataSection(strNodeText);

        node.AppendChild(cdata);

        return node;
    }

    /// <summary>
    /// 在XML文档对象xmlDoc中的root节点下添加一个名称由strNodeName指定内容由strNodeText指定的节点。如果在root指定
    /// 的根节点下已经存在有节点名称为strNodeName，那我们就仅仅在它的内容为空的时候把值修改为strNodeText
    /// </summary>
    /// <param name="xmlDoc">XML文档对象</param>
    /// <param name="root">文档对象中的指定要添加子节点的根节点</param>
    /// <param name="strNodeName">要被添加的子节点名称（或xPath方式）</param>
    /// <param name="strNodeText">要被添加的子节点的内容</param>
    /// <returns>被添加的子节点对象</returns>
    /// <remarks>
    /// 在XML文档对象xmlDoc中的root节点下添加一个名称由strNodeName指定内容由strNodeText指定的节点。如果在root指定
    /// 的根节点下已经存在有节点名称为strNodeName，那我们就仅仅在它的内容为空的时候把值修改为strNodeText。
    /// </remarks>
    /// <example>
    /// 下面我们可以看一下该函数的处理（原数据与结果数据的对比，这里我们针对节点FOURTH, strNodeText设定为“1234”，
    /// 根节点设定为SECOND指定节点）：
    /// <table align="center">
    ///		<tr>
    ///			<td align="center">XML文档对象原数据</td>
    ///			<td align="center">XML文档对象新数据</td>
    ///		</tr>
    ///		<tr>
    ///			<td>
    ///				<code>
    ///	&lt;FIRST&gt;
    ///		&lt;SECOND&gt;
    ///			&lt;THIRD/&gt;
    ///		&lt;/SECOND&gt;
    ///	&lt;/FIRST&gt;
    ///				</code>
    ///			</td>
    ///			<td>
    ///				<code>
    ///	&lt;FIRST&gt;
    ///		&lt;SECOND&gt;
    ///			&lt;THIRD/&gt;
    ///			&lt;FOURTH&gt;1234&lt;/FOURTH&gt;
    ///		&lt;/SECOND&gt;
    ///	&lt;/FIRST&gt;
    ///				</code>
    ///			</td>
    ///		</tr>
    ///		<tr>
    ///			<td>
    ///				<code>
    ///	&lt;FIRST&gt;
    ///		&lt;SECOND&gt;
    ///			&lt;THIRD/&gt;
    ///			&lt;FOURTH/&gt;
    ///		&lt;/SECOND&gt;
    ///	&lt;/FIRST&gt;
    ///				</code>
    ///			</td>
    ///			<td>
    ///				<code>
    ///	&lt;FIRST&gt;
    ///		&lt;SECOND&gt;
    ///			&lt;THIRD/&gt;
    ///			&lt;FOURTH&gt;1234&lt;/FOURTH&gt;
    ///		&lt;/SECOND&gt;
    ///	&lt;/FIRST&gt;
    ///				</code>
    ///			</td>
    ///		</tr>
    ///		<tr>
    ///			<td>
    ///				<code>
    ///	&lt;FIRST&gt;
    ///		&lt;SECOND&gt;
    ///			&lt;THIRD/&gt;
    ///			&lt;FOURTH&gt;6789&lt;/FOURTH&gt;
    ///		&lt;/SECOND&gt;
    ///	&lt;/FIRST&gt;
    ///				</code>
    ///			</td>
    ///			<td>
    ///				<code>
    ///	&lt;FIRST&gt;
    ///		&lt;SECOND&gt;
    ///			&lt;THIRD/&gt;
    ///			&lt;FOURTH&gt;6789&lt;/FOURTH&gt;
    ///		&lt;/SECOND&gt;
    ///	&lt;/FIRST&gt;
    ///				</code>
    ///			</td>
    ///		</tr>
    /// </table>
    /// </example>
    public static XmlNode AppendNotExistsNode(XmlNode root, string strNodeName, string strNodeText)
    {
        XmlNode node = root.SelectSingleNode(strNodeName);

        if (node == null)
            node = AppendNode(root, strNodeName);

        if (node.InnerText == string.Empty)
            node.InnerText = strNodeText;

        return node;
    }

    /// <summary>
    /// 在XML文档对象xmlDoc中的指定根节点root下，如果有名称为strNodeName的节点就把该节点的内容修改为strNodeText指定的内容。
    /// 否则就在root下创建一个名称为strNodeName内容为strNodeText指定的新节点
    /// </summary>
    /// <param name="xmlDoc">XML文档对象</param>
    /// <param name="root">被替换节点的的根节点</param>
    /// <param name="strNodeName">被替换子节点的名称(或xpath)</param>
    /// <param name="strNodeText">被替换子节点的内容</param>
    /// <returns>被替换节点的节点对象</returns>
    /// <remarks>
    /// 在XML文档对象xmlDoc中的指定根节点root下，如果有名称为strNodeName的节点就把该节点的内容修改为strNodeText指定的内容。
    /// 否则就在root下创建一个名称为strNodeName内容为strNodeText指定的新节点
    /// </remarks>
    public static XmlNode ReplaceExistsNode(XmlNode root, string strNodeName, string strNodeText)
    {
        XmlNode node = root.SelectSingleNode(strNodeName);

        if (node == null)
            node = AppendNode(root, strNodeName);

        node.InnerText = strNodeText;

        return node;
    }

    /// <summary>
    /// 在XML文档对象xmlDoc中的指定根节点root下，如果有名称为strNodeName的节点就把该节点的内容修改为strNodeText指定的内容。
    /// 否则就在root下创建一个名称为strNodeName内容为strNodeText指定的新节点
    /// </summary>
    /// <param name="xmlDoc">XML文档对象</param>
    /// <param name="root">被替换节点的的根节点</param>
    /// <param name="strNodeName">被替换子节点的名称(或xpath)</param>
    /// <returns>被替换节点的节点对象</returns>
    /// <remarks>
    /// 在XML文档对象xmlDoc中的指定根节点root下，如果有名称为strNodeName的节点就把该节点的内容修改为strNodeText指定的内容。
    /// 否则就在root下创建一个名称为strNodeName内容为strNodeText指定的新节点
    /// </remarks>
    public static XmlNode ReplaceExistsNode(XmlNode root, string strNodeName)
    {
        return ReplaceExistsNode(root, strNodeName, string.Empty);
    }

    /// <summary>
    /// 为xml文档对象的node指定节点上创建一个属性，属性名称为strAttrName，默认该属性的值为空
    /// </summary>
    /// <param name="xmlDoc">xml文档对象</param>
    /// <param name="node">要添加属性的节点</param>
    /// <param name="strAttrName">要添加的属性名称</param>
    /// <returns>添加的属性对象（此时属性的内容还是空的）</returns>
    /// <remarks>
    /// 为xml文档对象的node指定节点上创建一个属性，属性名称为strAttrName，默认该属性的值为空。如果您需要添加具体的属性内容
    /// 可以采用该函数的重载函数AppendAttr(XmlDocument xmlDoc, XmlNode node, string strAttrName, string strValue)
    /// </remarks>
    /// <example>
    /// <code>
    /// XmlDocument xmlDoc = XMLHelper.CreateDomDocument("&lt;FIRST&gt;&lt;SECOND&gt;&lt;THIRD/&gt;&lt;/SECOND&gt;&lt;/FIRST&gt;");
    /// XmlAttribute attr = XMLHelperr.AppendAttr(xmlDoc, xmlDoc.DocumentElement.FirstChild, "WEIGHT");
    /// attr.Value = "65kg";
    /// </code>
    /// </example>
    public static XmlAttribute AppendAttr(XmlNode node, string strAttrName)
    {
        XmlAttribute attr = node.OwnerDocument.CreateAttribute(strAttrName);

        node.Attributes.SetNamedItem(attr);

        return attr;
    }

    /// <summary>
    /// 为xml文档对象的node指定节点上创建一个属性，属性名称为strAttrName，默认该属性的值为strValue指定
    /// </summary>
    /// <param name="xmlDoc">xml文档对象</param>
    /// <param name="node">要添加属性的节点</param>
    /// <param name="strAttrName">要添加的属性名称</param>
    /// <param name="strValue">要添加的属性内容</param>
    /// <returns>添加的属性对象</returns>
    /// <remarks>
    /// 为xml文档对象的node指定节点上创建一个属性，属性名称为strAttrName，默认该属性的值为strValue指定。注意，该函数调用了
    /// AppendAttr(XmlDocument xmlDoc, XmlNode node, string strAttrName)来创建一个默认值为空的属性，然后再把属性内容修改成
    /// strValue指定的内容
    /// </remarks>
    public static XmlAttribute AppendAttr(XmlNode node, string strAttrName, string strValue)
    {
        XmlAttribute attr = AppendAttr(node, strAttrName);

        attr.Value = strValue;

        return attr;
    }

    /// <summary>
    /// 对节点列表nodeList中的每一个节点进行nodeOP指定的操作，其中oParam是操作使用到的参数
    /// </summary>
    /// <param name="nodeList">节点列表</param>
    /// <param name="nodeOP">每次枚举一个节点所做的操作</param>
    /// <param name="oParam">每次操作所传入的参数</param>
    /// <remarks>
    /// 对节点列表nodeList中的每一个节点进行nodeOP指定的操作，其中oParam是操作使用到的参数
    /// </remarks>
    /// <example>
    /// <code>
    /// public static void myMethod(XmlNode nodeRoot, object oParam)
    /// {
    /// }
    ///
    /// public static void Invoke(XmlNodeList nodeList)
    /// {
    /// 	XMLHelper.EnumNodeList(nodeList, new XMLHelper.DoNodeList(myMethod), new object());
    /// }
    /// </code>
    /// </example>
    public static void EnumNodeList(XmlNodeList nodeList, DoNodeList nodeOP, object oParam)
    {
        foreach (XmlNode node in nodeList)
            nodeOP(node, oParam);
    }

    /// <summary>
    /// 得到一个节点的SelectSingleNode的查询结果中的正文，如果该节点不存在，则返回空字符串
    /// </summary>
    /// <param name="nodeRoot">执行查询的根节点</param>
    /// <param name="strPath">查询的XPath</param>
    /// <returns>查询结果中的正文，如果该节点不存在，则返回空字符串</returns>
    public static string GetSingleNodeText(XmlNode nodeRoot, string strPath)
    {
        return GetSingleNodeText(nodeRoot, strPath, string.Empty);
    }

    /// <summary>
    /// 得到一个节点的SelectSingleNode的查询结果中的正文，如果该节点不存在，则返回缺省值
    /// </summary>
    /// <param name="nodeRoot">执行查询的根节点</param>
    /// <param name="strPath">查询的XPath</param>
    /// <param name="strDefault">如果节点没有找到，返回的缺省值</param>
    /// <returns>查询结果中的正文，如果该节点不存在，则返回缺省值</returns>
    public static string GetSingleNodeText(XmlNode nodeRoot, string strPath, string strDefault)
    {
        return (string)GetSingleNodeValue(nodeRoot, strPath, strDefault);
    }

    /// <summary>
    /// 得到一个节点的SelectSingleNode的查询结果中的内容(内容可以是不同类型)，如果该节点不存在，则返回缺省值
    /// </summary>
    /// <param name="nodeRoot">执行查询的根节点</param>
    /// <param name="strPath">查询的XPath</param>
    /// <returns>查询结果中的内容，如果该节点不存在，则返回DBNull.Value</returns>
    public static object GetSingleNodeValue(XmlNode nodeRoot, string strPath)
    {
        return GetSingleNodeValue(nodeRoot, strPath, DBNull.Value);
    }

    /// <summary>
    /// 得到一个节点的SelectSingleNode的查询结果中的内容(内容可以是不同类型)，如果该节点不存在，则返回缺省值
    /// </summary>
    /// <param name="nodeRoot">执行查询的根节点</param>
    /// <param name="strPath">查询的XPath</param>
    /// <param name="oDefault">如果节点没有找到，返回的缺省值</param>
    /// <returns>查询结果中的内容，如果该节点不存在，则缺省值</returns>
    public static object GetSingleNodeValue(XmlNode nodeRoot, string strPath, object oDefault)
    {
        object oResult = DBNull.Value;

        XmlNode node = nodeRoot.SelectSingleNode(strPath);

        if (node == null || (node != null && node.InnerText == string.Empty))
            oResult = oDefault;
        else
            oResult = Convert.ChangeType(node.InnerText, oDefault.GetType());

        return oResult;
    }

    /// <summary>
    /// 得到一个节点的SelectSingleNode的查询结果中的内容，如果该节点不存在，则抛出异常
    /// </summary>
    /// <param name="nodeRoot">执行查询的根节点</param>
    /// <param name="strPath">查询的XPath</param>
    /// <returns>查询结果中的内容</returns>
    public static XmlNode GetSingleNodeException(XmlNode nodeRoot, string strPath)
    {
        return GetSingleNodeException(nodeRoot, strPath, string.Empty);
    }

    /// <summary>
    /// 得到一个节点的SelectSingleNode的查询结果中的内容，如果该节点不存在，则抛出异常
    /// </summary>
    /// <param name="nodeRoot">执行查询的根节点</param>
    /// <param name="strPath">查询的XPath</param>
    /// <param name="strException">异常文本</param>
    /// <returns>查询结果中的内容</returns>
    public static XmlNode GetSingleNodeException(XmlNode nodeRoot, string strPath, string strException)
    {
        XmlNode node = nodeRoot.SelectSingleNode(strPath);

        if (strException == null || strException == string.Empty)
            strException = string.Format("不能在Xml节点{0}下找到{1}", nodeRoot.Name, strPath);

        //HBExceptionTools.FalseThrow(node != null, typeof(System.ArgumentException), strException);

        return node;
    }

    /// <summary>
    /// 得到一个节点的缺省值，如果该节点不存在，则返回缺省值
    /// </summary>
    /// <param name="nodeRoot">执行查询的节点</param>
    /// <param name="attrName">属性名称</param>
    /// <returns>属性的值，如果该属性不存在，则返回空串</returns>
    public static string GetAttributeText(XmlNode nodeRoot, string attrName)
    {
        return GetAttributeText(nodeRoot, attrName, string.Empty);
    }

    /// <summary>
    /// 得到一个节点的缺省值，如果该节点不存在，则返回缺省值(字符串类型)
    /// </summary>
    /// <param name="nodeRoot">执行查询的节点</param>
    /// <param name="attrName">属性名称</param>
    /// <param name="strDefault"></param>
    /// <returns>属性的值，如果该属性不存在，则返回缺省的字符串</returns>
    public static string GetAttributeText(XmlNode nodeRoot, string attrName, string strDefault)
    {
        return (string)GetAttributeValue(nodeRoot, attrName, strDefault);
    }

    /// <summary>
    /// 得到一个节点的缺省值，如果该节点不存在，则返回缺省值
    /// </summary>
    /// <param name="nodeRoot">执行查询的节点</param>
    /// <param name="attrName">属性名称</param>
    /// <returns>属性的值，如果该属性不存在，则返回DBNull.Value</returns>
    public static object GetAttributeValue(XmlNode nodeRoot, string attrName)
    {
        return GetAttributeValue(nodeRoot, attrName, DBNull.Value);
    }

    /// <summary>
    /// 得到一个节点的缺省值，如果该节点不存在，则返回缺省值
    /// </summary>
    /// <param name="nodeRoot">执行查询的节点</param>
    /// <param name="attrName">属性名称</param>
    /// <param name="oDefault"></param>
    /// <returns>属性的值，如果该属性不存在，则返回缺省值</returns>
    public static object GetAttributeValue(XmlNode nodeRoot, string attrName, object oDefault)
    {
        XmlElement elem = (XmlElement)nodeRoot;

        string strResult = elem.GetAttribute(attrName);
        object oResult = null;

        if (strResult == string.Empty)
            oResult = oDefault;
        else
            oResult = Convert.ChangeType(strResult, oDefault.GetType());

        return oResult;
    }
    #endregion



    static public string XmlDocumentToString(XmlDocument doc)
    {
        MemoryStream stream = new MemoryStream();
        XmlTextWriter writer = new XmlTextWriter(stream, null);
        writer.Formatting = Formatting.Indented;
        doc.Save(writer);

        StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
        stream.Position = 0;
        string xmlString = sr.ReadToEnd();
        sr.Close();
        stream.Close();

        return xmlString;
    }

}
