using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using HiLand.Utility.Data;

namespace SimulatorApp
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }

        /// <summary>
        /// 模拟请求
        /// </summary>
        private void SimulateRequest()
        {
            string targetUrl = "http://qdmax.net";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUrl);
            request.Method = "GET";

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string returnValue = HtmlHelper.StripHTML(reader.ReadToEnd());
                            returnValue.Replace("\r\n", string.Empty);
                            if (returnValue.Length > 30)
                            {
                                returnValue = returnValue.Substring(30);
                            }
                            this.textBox1.Text += String.Format("[{0}]{1}\r\n", DateTime.Now, returnValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
            finally
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SimulateRequest();
        }
    }
}
