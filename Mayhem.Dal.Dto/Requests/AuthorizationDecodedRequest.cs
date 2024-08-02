using Mayhem.Dal.Dto.Request.Models;

namespace Mayhem.Dal.Dto.Requests
{
    public class AuthorizationDecodedRequest
    {
        public SignedData signedData { get; set; }
        public SignedFreshData signedFreshData { get; set; }
        public bool isCyberConnect { get; set; }
    }
}
