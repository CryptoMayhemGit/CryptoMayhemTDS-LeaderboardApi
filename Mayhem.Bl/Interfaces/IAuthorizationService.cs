using Mayhem.Dal.Dto.Responses;

namespace Mayhem.Bl.Interfaces
{
    public interface IAuthorizationService
    {
        public Task<JWTTokenResponse> GetAuthorizationToken();
    }
}
