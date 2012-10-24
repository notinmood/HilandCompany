using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using HiLand.Utility.Event;

namespace Hiland.Project.ADStatistics.UI
{
    public partial class UserControl2 : UCBase
    {
        public UserControl2()
        {
            InitializeComponent();
        }

        protected override void OnBeforeClose(EventArgs<EventControlEntity> eventArgs)
        {
            eventArgs.Data.Token = false;
        }
    }
}
