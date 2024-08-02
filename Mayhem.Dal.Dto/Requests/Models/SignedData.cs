using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem.Dal.Dto.Request.Models
{
    public class SignedData
    {
        public string Wallet { get; set; }
        public string Nonce { get; set; }
        public string Message { get; set; }
        public string Handle { get; set; }
    }
}
