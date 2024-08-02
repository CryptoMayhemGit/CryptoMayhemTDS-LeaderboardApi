using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem.Dal.Dto.Request.Models
{
    public class SignedFreshData
    {
        public string Data { get; set; }
        public string Signature { get; set; }
    }
}
