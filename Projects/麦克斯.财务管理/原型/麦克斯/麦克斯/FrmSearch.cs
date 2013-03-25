using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace 麦克斯
{
    public partial class FrmSearch : Form
    {
        public FrmSearch()
        {
            InitializeComponent();
        }

        public FrmSearch(string sql)
        {
            InitializeComponent();
            this.sql = sql;
        }

        private string sql;

        private void FrmSelect_Load(object sender, EventArgs e)
        {
            this.dgvSearchResult.AutoGenerateColumns = false;
            Bind();
        }

        private void Bind()
        {
            DBHelper db = new DBHelper();
            DataSet ds = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter(sql, db.Conn);
            sda.Fill(ds, "company");
            this.dgvSearchResult.DataSource = ds.Tables["company"];
        }
    }
}
