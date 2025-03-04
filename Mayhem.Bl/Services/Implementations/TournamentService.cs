using AutoMapper;
using FluentValidation;
using Mayhem.Bl.Services.Interfaces;
using Mayhem.Configuration;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Responses;
using Mayhem.Dal.Repositories.Implementations;
using Mayhem.Dal.Repositories.Interfaces;
using Mayhem.Dal.Tables;
using Mayhem.Util.Classes;
using Mayhem.Util.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Nethereum.Contracts;
using Nethereum.Web3;
using Newtonsoft.Json;
using System.Numerics;
using System.Text;

namespace Mayhem.Bl.Services.Implementations
{
    public class TournamentService(HttpClient httpClient, MayhemConfiguration mayhemConfiguration, IWeb3 web3, ITicketEndoceService ticketEndoceService, IValidator<AuthorizationDecodedRequest> authorizationRequestValidator, IValidator<UpdateTournamentRequest> updateTournamentValidator, IMapper mapper, IValidator<AddTournamentRequest> addTournamentValidator, ITournamentRepository tournamentRepository, ILogger<TournamentService> logger) : ITournamentService
    {
        public async Task<AddTournamentResponse> AddTournamentAsync(AddTournamentRequest request)
        {
            var validatorResult = await addTournamentValidator.ValidateAsync(request);

            if (!validatorResult.IsValid)
            {
                throw new NotFoundException(new ValidationMessage("BAD_REQUEST", validatorResult.Errors.First().ErrorMessage));
            }

            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = startDate.AddHours(request.DurationHours);

            TournamentDto tournamentDto = new TournamentDto()
            {
                Name = request.Name,
                StartDate = startDate,
                EndDate = endDate,
                IsFinished = false,
                CreateDate = DateTime.UtcNow,
                MP = request.MP,
                SP = request.SP,
                HP = request.HP,
                QuestDetails = mapper.Map<List<QuestDetailsDto>>(request.QuestDetails)
            };

            tournamentDto.Id = await tournamentRepository.AddAsync(tournamentDto);

            string informationMessage = $"Tournament has been added.";
            logger.LogInformation(informationMessage);

            await SendPostRequestAsync("https://hook.eu2.make.com/1wlsmosbf3mrue4usu0ugkqjq3ldn8bl", tournamentDto);

            return new AddTournamentResponse() { Success = true };
        }

        private async Task SendPostRequestAsync(string url, TournamentDto tournamentDto)
        {
            var json = JsonConvert.SerializeObject(tournamentDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = $"Failed to send POST request. Status code: {response.StatusCode}";
                logger.LogError(errorMessage);
                throw new HttpRequestException(errorMessage);
            }
        }

        public async Task<GetActiveTournamentResponse> GetActiveTournamentAsync()
        {
            Tournaments tournaments = await tournamentRepository.GetActiveAsync();

            if (tournaments == null)
                return new GetActiveTournamentResponse();

            TournamentDto tournamentDto = mapper.Map<TournamentDto>(tournaments);

            return new GetActiveTournamentResponse()
            {
                Id = tournamentDto.Id,
                Name = tournamentDto.Name,
                StartDate = tournamentDto.StartDate,
                EndDate = tournamentDto.EndDate,
                IsFinished = tournamentDto.IsFinished,
                CreateDate = tournamentDto.CreateDate,
                HP = tournamentDto.HP,
                SP = tournamentDto.SP,
                MP = tournamentDto.MP,
                TournamentUserStatistics = tournamentDto.TournamentUserStatistics,
                QuestDetails = tournamentDto.QuestDetails
            };
        }

        public async Task<GetArchivedTournamentsResponse> GetArchivedTournamentsAsync()
        {
            List<Tournaments> tournamentsCollection = await tournamentRepository.GetArchivedAsync();

            if (tournamentsCollection.Any() == false)
                return new GetArchivedTournamentsResponse();

            GetArchivedTournamentsResponse result = new();
            result.TournamentDtos = mapper.Map<List<TournamentDto>>(tournamentsCollection);

            return result;
        }

        public async Task<IsAnyTicketTournamentActiveResponse> IsAnyTicketTournamentActiveAsync(string ticket)
        {
            if (string.IsNullOrEmpty(ticket))
            {
                AddErrorBadRequest();
            }

            AuthorizationDecodedRequest authorizationDecodedRequest = ticketEndoceService.DecodeTicket(ticket);
            await ValidateRequest(authorizationDecodedRequest);

            await CheckTicket(mayhemConfiguration, authorizationDecodedRequest);

            Tournaments tournaments = await tournamentRepository.GetActiveAsync();

            return new IsAnyTicketTournamentActiveResponse { IsActive = tournaments != null };
        }

        private async Task CheckTicket(MayhemConfiguration mayhemConfiguration, AuthorizationDecodedRequest authorizationDecodedRequest)
        {
            Contract? web3Contract = web3.Eth.GetContract(mayhemConfiguration.AlturaTournamentAbi, mayhemConfiguration.AlturaTournamentAddress);
            BigInteger ticketCount = 0;
            Function function = web3Contract.GetFunction("balanceOf");
            try
            {
                ticketCount = await function.CallAsync<BigInteger>(authorizationDecodedRequest.signedData.Wallet, new BigInteger(mayhemConfiguration.AlturaTournamentTicketId));

                logger.LogInformation($"Wallet {authorizationDecodedRequest.signedData.Wallet} already purchased {ticketCount} tokens.");
            }
            catch (Exception ex)
            {
                string errorMessage = "Cannot get purchasedticket from smart contract.";
                logger.LogError(ex, errorMessage);
                throw new NotFoundException(new ValidationMessage("BAD_REQUEST", errorMessage));
            }

            if (ticketCount <= 0)
            {
                throw new NotFoundException(new ValidationMessage("BAD_REQUEST", "No active tournament ticket."));
            }
        }

        public async Task<UpdateTournamentResponse> UpdateTournamentAsync(UpdateTournamentRequest request)
        {
            var validatorResult = await updateTournamentValidator.ValidateAsync(request);

            if (!validatorResult.IsValid)
            {
                throw new NotFoundException(new ValidationMessage("BAD_REQUEST", validatorResult.Errors.First().ErrorMessage));
            }

            Tournaments tournaments = await tournamentRepository.GetAsync(request.Id);

            if (request.Name.IsNullOrEmpty() == false)
            {
                tournaments.Name = request.Name;
            }

            if (request.EndDate.HasValue)
            {
                tournaments.EndDate = request.EndDate.Value;
            }

            if (request.StartDate.HasValue)
            {
                tournaments.StartDate = request.StartDate.Value;
            }

            await tournamentRepository.UpdateAsync(tournaments);

            return new UpdateTournamentResponse() { Success = true };
        }

        public async Task EndTournamentAsync()
        {
            Tournaments tournament = await tournamentRepository.GetActiveAsync();
            if (tournament != null)
            {
                tournament.IsFinished = true;
                await tournamentRepository.UpdateAsync(tournament);

                TournamentDto tournamentDto = mapper.Map<TournamentDto>(tournament);
                await SendPostRequestAsync("https://hook.eu2.make.com/d6g08ol96v6rtln47fxe6uk1v1w3y60f", tournamentDto);
            }
            else
            {
                string errorMessage = "No active tournament.";
                logger.LogError(errorMessage);
                throw new NotFoundException(new ValidationMessage("BAD_REQUEST", errorMessage));
            }
        }


        public async Task TryEndTournamentAsync()
        {
            Tournaments tournament = await tournamentRepository.GetActiveAsync();
            if (tournament == null)
            {
                string errorMessage = "No active tournament.";
                logger.LogError(errorMessage);
                throw new NotFoundException(new ValidationMessage("BAD_REQUEST", errorMessage));
            }

            if (tournament.EndDate > DateTime.UtcNow)
            {
                string errorMessage = "Aktywny turniej istnieje i nie można go zakończyć.";
                logger.LogError(errorMessage);
                throw new NotFoundException(new ValidationMessage("BAD_REQUEST", errorMessage));
            }

            tournament.IsFinished = true;
            await tournamentRepository.UpdateAsync(tournament);

            TournamentDto tournamentDto = mapper.Map<TournamentDto>(tournament);
            await SendPostRequestAsync("https://hook.eu2.make.com/d6g08ol96v6rtln47fxe6uk1v1w3y60f", tournamentDto);
        }

        private async Task ValidateRequest(AuthorizationDecodedRequest authorizationDecodedRequest)
        {
            var validationResult = await authorizationRequestValidator.ValidateAsync(authorizationDecodedRequest);

            if (validationResult.IsValid == false)
            {
                string firstErrorCode = validationResult.Errors.First().ErrorCode;

                if (firstErrorCode == "ACCESS_DENIED")
                {
                    AddErrorAccessDenied();
                }
                else
                {
                    AddErrorBadRequest();
                }
            }
        }

        private void AddErrorAccessDenied()//TODO to separate class or nugget.
        {
            string errorMessage = $"Access denied.";
            logger.LogInformation(errorMessage);
            throw new NotFoundException(new ValidationMessage("ACCESS_DENIED", errorMessage));
        }

        private void AddErrorBadRequest()//TODO to separate class or nugget.
        {
            string errorMessage = $"Bad request.";
            logger.LogInformation(errorMessage);
            throw new NotFoundException(new ValidationMessage("BAD_REQUEST", errorMessage));
        }
    }
}
