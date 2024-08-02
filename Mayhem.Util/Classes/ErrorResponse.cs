using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem.Util.Classes
{
    public class ErrorResponse
    {
        public ErrorModel Error { get; }

        public ErrorResponse(ErrorModel error)
        {
            Error = error;
        }

        public override string ToString()
        {
            if (Error != null)
            {
                return $"{Error.Code} - {Error.Message}";
            }

            return base.ToString();
        }
    }
}
