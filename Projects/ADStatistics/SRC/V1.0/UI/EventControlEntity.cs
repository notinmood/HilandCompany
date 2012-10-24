using System;
using System.Collections.Generic;
using System.Text;

namespace Hiland.Project.ADStatistics.UI
{
    public class EventControlEntity
    {
        private bool token = true;
        /// <summary>
        /// 事件令牌
        /// </summary>
        public bool Token
        {
            get { return this.token; }
            set { this.token = value; }
        }

        private int delayOperationSecond = 0;
        /// <summary>
        /// 延时操作的秒数
        /// </summary>
        /// <remarks>
        /// 比如点击保存按钮后几秒内关闭窗体等场景下使用
        /// </remarks>
        public int DelayOperationSecond
        {
            get { return this.delayOperationSecond; }
            set { this.delayOperationSecond = value; }
        }

        /// <summary>
        /// 令牌的信息
        /// </summary>
        public string TokenString { get; set; }
    }
}
