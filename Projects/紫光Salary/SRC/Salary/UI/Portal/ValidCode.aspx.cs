using System;
using Salary.Web.BasePage;
using Salary.Web.Utility;
using System.IO;
using Salary.Biz;
using System.Web.UI;

public partial class ValieCode : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MemoryStream image = null;
        String validateCode = GenerateValidateImage.DrawImage(ref image);
        Session[SalaryConst.ValidateCode] = validateCode;
        Response.ClearContent();
        Response.ContentType = "image/Jpeg";
        Response.BinaryWrite(image.ToArray());
    }
}
