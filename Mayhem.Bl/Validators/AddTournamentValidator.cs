using FluentValidation;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Repositories.Interfaces;
using Mayhem.Dal.Tables;

namespace Mayhem.Bl.Validators
{
    public class AddTournamentValidator : AbstractValidator<AddTournamentRequest>
    {
        private readonly ITournamentRepository tournamentRepository;
        public AddTournamentValidator(ITournamentRepository tournamentRepository)
        {
            Validation();
            this.tournamentRepository = tournamentRepository;
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyDuplicateRecords();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x).NotNull().WithMessage("Invalid add tournament request.");
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Please specify a start date.");
            RuleFor(x => x.EndDate).NotEmpty().WithMessage("Please specify a end date.");
            RuleFor(x => x.StartDate).GreaterThan(DateTime.UtcNow).WithMessage("Start date should be greater ten now.");
            RuleFor(x => x.EndDate).GreaterThan(DateTime.UtcNow).WithMessage("End date should be greater ten now.");
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).WithMessage("End date should be greater ten start time.");
        }

        private void VerifyDuplicateRecords()
        {
            RuleFor(x => x).CustomAsync(async (addTournamentRequest, context, cancellation) =>
            {
                Tournaments tournament = await tournamentRepository.GetAsync(addTournamentRequest.Name);
                if (tournament != null)
                {
                    context.AddFailure("Such tournament exist.");
                }
            });
        }
    }
}
