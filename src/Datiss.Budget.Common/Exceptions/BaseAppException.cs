using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Common.Exceptions
{
    public class BaseAppException : Exception
    {

        public BaseAppException(): base() { }

        public BaseAppException(string message): base(message) { }

    } 
}
