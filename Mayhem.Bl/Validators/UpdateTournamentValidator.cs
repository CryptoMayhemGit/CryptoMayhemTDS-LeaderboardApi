using FluentValidation;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Repositories.Interfaces;
using Mayhem.Dal.Tables;
using Microsoft.IdentityModel.Tokens;

namespace Mayhem.Bl.Validators
{
    public class UpdateTournamentValidator : AbstractValidator<UpdateTournamentRequest>
    {
        private readonly ITournamentRepository tournamentRepository;
        public UpdateTournamentValidator(ITournamentRepository tournamentRepository)
        {
            Validation();
            this.tournamentRepository = tournamentRepository;
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyDuplicateRecords();
            VerifyExistence();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x).NotNull().WithMessage("Invalid update tournament request.");
            RuleFor(x => x.Id).NotNull().WithMessage("Id is required.");
            RuleFor(x => x.StartDate).GreaterThan(DateTime.UtcNow).WithMessage("Start date should be greater then now.")
            .When(update => update.StartDate != null);
            RuleFor(x => x.EndDate).GreaterThan(DateTime.UtcNow).WithMessage("End date should be greater then now.")
            .When(update => update.EndDate != null);
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).WithMessage("End date should be greater then start time.")
            .When(update => update.StartDate != null && update.EndDate != null);
        }

        private void VerifyDuplicateRecords()
        {
            RuleFor(x => x).CustomAsync(async (updateTournamentRequest, context, cancellation) =>
            {
                if (updateTournamentRequest.Name.IsNullOrEmpty()) 
                { 
                    return;
                }

                Tournaments tournament = await tournamentRepository.GetAsync(updateTournamentRequest.Name);
                if (tournament != null)
                {
                    context.AddFailure("Such tournament exist.");
                }
            });
        }

        private void VerifyExistence()
        {
            RuleFor(x => x).CustomAsync(async (updateTournamentRequest, context, cancellation) =>
            {
                Tournaments tournament = await tournamentRepository.GetAsync(updateTournamentRequest.Id);
                if (tournament == null)
                {
                    context.AddFailure("Such tournament not exist.");
                }
            });
        }
    }
}
