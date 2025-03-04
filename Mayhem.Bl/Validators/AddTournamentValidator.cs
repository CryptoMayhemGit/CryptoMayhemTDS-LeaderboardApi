using FluentValidation;
using Mayhem.Dal.Dto.Request.Models;
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
            RuleFor(x => x.DurationHours).GreaterThan(0).WithMessage("Provide more than 0 hour.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Enter the name of the tournament.");
            RuleFor(x => x.MP).GreaterThanOrEqualTo(0).LessThanOrEqualTo(10).WithMessage("Set MP between 0 and 10.");
            RuleFor(x => x.HP).GreaterThan(0).WithMessage("Set HP to greater than 0.");
            RuleFor(x => x.SP).GreaterThanOrEqualTo(0).WithMessage("SP should be greater ten now.");
            RuleFor(x => x.QuestDetails).NotEmpty().WithMessage("Add at least one tournament quest request.");
            RuleFor(x => x.QuestDetails)
                .Must(list => list.Any(item => item.TournamentType != TournamentType.None.ToString()))
                .WithMessage("At least one type must be selected for AddTournamentQuestRequest and it cannot be 'None'.");
            RuleForEach(x => x.QuestDetails)
                .ChildRules(quest =>
                {
                    quest.RuleFor(q => q.Value)
                        .GreaterThan(0).WithMessage("Value must be at least 1 for Kill tournament type.")
                        .When(q => q.TournamentType == TournamentType.Kill.ToString());

                    quest.RuleFor(q => q.Value)
                        .GreaterThanOrEqualTo(30).WithMessage("Value must be at least 30 for Survive tournament type.")
                        .When(q => q.TournamentType == TournamentType.Survive.ToString());
                });
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

                Tournaments activeTournament = await tournamentRepository.GetActiveAsync();
                if (activeTournament != null && activeTournament.EndDate > DateTime.UtcNow && activeTournament.IsFinished == false)
                {
                    context.AddFailure("There is already an active tournament.");
                }
            });
        }
    }
}
