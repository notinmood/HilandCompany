using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaborCompare
{
    public partial class Conver : Form
    {
        public Conver()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            劳务人员前后两个批次对比 window = new 劳务人员前后两个批次对比();
            window.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            导入银行卡号 window = new 导入银行卡号();
            window.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            后勤提供数据与财务提供数据对比 window = new 后勤提供数据与财务提供数据对比();
            window.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            修改劳务人员中关联企业的guid window = new 修改劳务人员中关联企业的guid();
            window.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            创建劳务人员合同 window = new 创建劳务人员合同();
            window.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            更新劳务人员对应的工作人员 window = new 更新劳务人员对应的工作人员();
            window.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            导入企业更新企业对应的工作人员 window = new 导入企业更新企业对应的工作人员();
            window.ShowDialog();
        }
    }
}
