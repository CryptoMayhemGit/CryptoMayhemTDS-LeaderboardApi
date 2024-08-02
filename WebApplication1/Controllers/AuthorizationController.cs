using FluentValidation;
using Mayhem.Bl.Interfaces;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Mayhem.LeaderBoardApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IValidator<LoginRequest> loginAuthorizationValidator;

        public AuthorizationController(IAuthorizationService authorizationService, IValidator<LoginRequest> loginAuthorizationValidator)
        {
            this.authorizationService = authorizationService;
            this.loginAuthorizationValidator = loginAuthorizationValidator;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var validatorResult = await loginAuthorizationValidator.ValidateAsync(loginRequest);

            if (!validatorResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, validatorResult.Errors.First().ErrorMessage);
            }

            JWTTokenResponse authorizationToken = await authorizationService.GetAuthorizationToken();
            return Ok(authorizationToken);
        }
    }
}
