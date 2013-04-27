using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Salary.Core.Utility
{
    public class MyException : ApplicationException
    {
        public string MyMessage = string.Empty;
        public MyException(string message)
            : base(message)
        {
            MyMessage = message;
        }
    }
}
