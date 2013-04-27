using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;

namespace Salary.Web.Utility
{
    public class PageHelper
    {
        public static Page GetCurrentPage()
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            return page;
        }
        public static List<Control> FindControlsList(Control control)
        {
            List<Control> list = new List<Control>();
            var childs = control.Controls.OfType<Control>().ToList();
            list.AddRange(childs.ToArray());
            for (int i = 0; i < childs.Count(); i++)
            {
                list.AddRange(FindControlsList(childs[i]));
            }
            return list;
        }
    }
}
