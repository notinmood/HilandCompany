using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Salary.Web.Utility;
using Salary.Web.BasePage;
using Salary.Core.Utility;
using Salary.Biz;

public partial class _Default : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UrlParamBuilder urlEdit = new UrlParamBuilder(SalaryConst.IndexPageUrl);
        Redirect(urlEdit.ToUrlString());
    }

    /*
    CREATE TABLE [dbo].[USER_LOG](
	[USER_ID] [varchar](36) NOT NULL,
	[LOGON_NAME] [varchar](36) NULL,
	[INPUT_DATE] [datetime] NOT NULL,
     CONSTRAINT [PK_USER_LOG] PRIMARY KEY CLUSTERED 
    (
	    [USER_ID] ASC,
	    [INPUT_DATE] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    ) ON [PRIMARY]
     */
}
