using FluentValidation;
using Mayhem.Configuration;
using Mayhem.Dal.Dto.Requests;

namespace Mayhem.Bl.Validators
{
    public class LoginAuthorizationValidator : AbstractValidator<LoginRequest>
    {
        private readonly MayhemConfiguration mayhemConfiguration;

        public LoginAuthorizationValidator(MayhemConfiguration mayhemConfiguration)
        {
            this.mayhemConfiguration = mayhemConfiguration;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            CheckAuthorizationData();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x).NotNull().WithMessage("Invalid login request.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Please specify a login.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please specify a password.");
        }

        private void CheckAuthorizationData()
        {
            RuleFor(x => x.UserName).Equal(mayhemConfiguration.AdminLogin).WithMessage("The credentials are incorrect.");
            RuleFor(x => x.Password).Equal(mayhemConfiguration.AdminPassword).WithMessage("The credentials are incorrect.");
        }
    }
}
