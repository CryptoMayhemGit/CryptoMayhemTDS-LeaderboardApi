using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mayhem.Util.Classes;

namespace Mayhem.Util.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationMessage ValidationMessage { get; set; }

        public ValidationException(ValidationMessage validationMessage)
        {
            ValidationMessage = validationMessage;
        }
    }
}
