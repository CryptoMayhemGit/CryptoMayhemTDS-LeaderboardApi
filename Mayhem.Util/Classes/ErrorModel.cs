using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem.Util.Classes
{
    public class ErrorModel
    {
        public string Code { get; }

        public string Message { get; }

        public ErrorModel(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
