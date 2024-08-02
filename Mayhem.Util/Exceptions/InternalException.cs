using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mayhem.Util.Classes;

namespace Mayhem.Util.Exceptions
{
    public class InternalException : Exception
    {
        public ValidationMessage ValidationMessage { get; set; }

        public InternalException(ValidationMessage validationMessage)
        {
            ValidationMessage = validationMessage;
        }
    }
}
