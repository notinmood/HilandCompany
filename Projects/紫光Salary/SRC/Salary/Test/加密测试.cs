using Salary.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void 加密_Click(object sender, EventArgs e)
    {
        this.Literal1.Text= CryptoHelper.Encode(this.TextBox1.Text);
    }
}