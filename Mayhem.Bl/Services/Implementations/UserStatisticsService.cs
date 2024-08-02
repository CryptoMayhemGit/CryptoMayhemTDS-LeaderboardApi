using FluentValidation;
using Mayhem.Bl.Services.Interfaces;
using Mayhem.Configuration;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Responses;
using Mayhem.Dal.Models;
using Mayhem.Dal.Repositories.Interfaces;
using Mayhem.Dal.Tables;
using Mayhem.Util.Classes;
using Mayhem.Util.Exceptions;
using Microsoft.Extensions.Logging;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Util;
using Nethereum.Web3;
using System.Numerics;

namespace Mayhem.Bl.Services.Implementations
{
    public class UserStatisticsService(MayhemConfiguration mayhemConfiguration, IWeb3 web3, ITournamentRepository tournamentRepository, IGameCodeRepository gameCodeRepository, IValidator<AuthorizationDecodedRequest> authorizationRequestValidator, ITicketEndoceService ticketEndoceService, ITournamentUserStatisticRepository userStatisticRepository, ILogger<UserStatisticsService> logger) : IUserStatisticsService
    {
        public async Task<StatsResponse> GetGameStatsAsync()
        {
            List<TournamentUserStatistics> tournamentUserStatistics = await userStatisticRepository.GetActiveTournamentGameUsersAsync();
            List<TournamentUserStatistics> yesterdaytournamentUserStatistics = GetUserStatisticsFromTesterday(tournamentUserStatistics);
            List<SimpleRanking> yesterdayRankingList = GetYesterdayRankingList(yesterdaytournamentUserStatistics);
            List<SimpleRanking> currentRankingList = GetYesterdayRankingList(tournamentUserStatistics);

            return GetStatsResponse(tournamentUserStatistics, yesterdayRankingList, currentRankingList);
        }

        public async Task<FinishGameResponse> FinishGameAsync(FinishGameRequest finishGameRequest)
        {
            if (string.IsNullOrEmpty(finishGameRequest.Ticket) || finishGameRequest.GameCode == Guid.Empty)
            {
                AddErrorBadRequest();
            }

            AuthorizationDecodedRequest authorizationDecodedRequest = ticketEndoceService.DecodeTicket(finishGameRequest.Ticket);
            await ValidateRequest(authorizationDecodedRequest);

            Tournaments tournaments  = await tournamentRepository.GetActiveAsync();
            if (tournaments == null)
            {
                AddErrorBadRequest("There is no active tournament.");
            }

            bool isActiveGameCode = await gameCodeRepository.IsGameCodeActiveAsync(authorizationDecodedRequest.signedData.Wallet, finishGameRequest.GameCode, tournaments.Id);

            if (isActiveGameCode == false)
            {
                AddErrorBadRequest($"Game code is not active for wallet {authorizationDecodedRequest.signedData.Wallet}, TournamentId {tournaments.Id} and GameCode {finishGameRequest.GameCode}."); // TODO dodaĆ iD NAJNOWSZEGO TURNIEJU.
            }

            TournamentUserStatisticsDto tournamentUserStatisticsDto = new()
            {
                Wallet = authorizationDecodedRequest.signedData.Wallet,
                IsWin = finishGameRequest.IsWin,
                Kills = finishGameRequest.Kills,
                CreateDate = DateTime.UtcNow,
                TournamentId = tournaments.Id
            };

            await gameCodeRepository.RemoveAsync(finishGameRequest.GameCode);

            await userStatisticRepository.AddAsync(tournamentUserStatisticsDto);
            return new FinishGameResponse() { Success = true };
        }

        private StatsResponse GetStatsResponse(List<TournamentUserStatistics> tournamentUserStatistics, List<SimpleRanking> yesterdayRankingList, List<SimpleRanking> currentRankingList)
        {
            StatsResponse statsResponse = new();
            foreach (SimpleRanking simpleRanking in currentRankingList)
            {
                var playerStatistic = new PlayerStatistic()
                {
                    Order = simpleRanking.Order,
                    Wallet = simpleRanking.Address,
                    Win = tournamentUserStatistics.Where(x => x.Wallet == simpleRanking.Address && x.IsWin == true).Count(),
                    Lose = tournamentUserStatistics.Where(x => x.Wallet == simpleRanking.Address && x.IsWin == false).Count(),
                    Kills = tournamentUserStatistics.Where(x => x.Wallet == simpleRanking.Address).Sum(x => x.Kills),
                    Points = tournamentUserStatistics.Where(x => x.Wallet == simpleRanking.Address && x.IsWin == true)
                        .OrderByDescending(x => x.Kills).FirstOrDefault()?.Kills ?? 0
                };

                if(yesterdayRankingList.Where(x => x.Address == simpleRanking.Address).Any() == false)
                {
                    playerStatistic.Change = 0;
                }
                else
                {
                    playerStatistic.Change = yesterdayRankingList.Where(x => x.Address == simpleRanking.Address).Single().Order - simpleRanking.Order;
                }

                statsResponse.PlayerStatistic.Add(playerStatistic);
            }

            return statsResponse;
        }

        private List<SimpleRanking> GetYesterdayRankingList(List<TournamentUserStatistics> yesterdaytournamentUserStatistics)
        {
            List<SimpleRanking> rankingList = new();
            foreach (var uniqueWallet in yesterdaytournamentUserStatistics.Select(o => o.Wallet).Distinct())
            {
                List<TournamentUserStatistics> userData = yesterdaytournamentUserStatistics.Where(x => x.Wallet == uniqueWallet).ToList();
                rankingList.Add
                (
                    new SimpleRanking()
                    {
                        Address = uniqueWallet,
                        Points = userData.Where(x => x.Wallet == uniqueWallet && x.IsWin == true)
                            .OrderByDescending(x => x.Kills).FirstOrDefault()?.Kills ?? 0
                    }
                );
            }
            int order = 1;
            rankingList = rankingList.OrderByDescending(item => item.Points).Select(item => { item.Order = order; order++; return item; }).ToList();
            return rankingList;
        }

        private List<TournamentUserStatistics> GetUserStatisticsFromTesterday(List<TournamentUserStatistics> tournamentUserStatistics)
        {
            return tournamentUserStatistics.Where(x => DateTime.Now.AddDays(-1) > x.CreateDate).ToList();
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

        private void AddErrorAccessDenied()//TODO to separate class.
        {
            string errorMessage = $"Access denied.";
            logger.LogInformation(errorMessage);
            throw new NotFoundException(new ValidationMessage("ACCESS_DENIED", errorMessage));
        }

        private void AddErrorBadRequest(string errorMessage = $"Bad request.")//TODO to separate class.
        {
            logger.LogInformation(errorMessage);
            throw new NotFoundException(new ValidationMessage("BAD_REQUEST", errorMessage));
        }

        public async Task<PlayerPointsResponse> GetPlayerPointsAsync(PlayerPointsRequest playerPointsRequest)
        {
            int resultPoints = 0;
            if (playerPointsRequest.IsWin == false)
            {
                return new PlayerPointsResponse
                {
                    Points = resultPoints
                };
            }
            else
            {
                return new PlayerPointsResponse
                {
                    Points = playerPointsRequest.Kills
                };
            }
        }

        public async Task<StartGameResponse> StartGameAsync(StartGameRequest request)
        {
            if (string.IsNullOrEmpty(request.Ticket))
            {
                AddErrorBadRequest();
            }

            AuthorizationDecodedRequest authorizationDecodedRequest = ticketEndoceService.DecodeTicket(request.Ticket);
            await ValidateRequest(authorizationDecodedRequest);

            Tournaments tournaments = await tournamentRepository.GetActiveAsync();
            if (tournaments == null)
            {
                AddErrorBadRequest("There is no active tournament.");
            }

            await CheckTicket(mayhemConfiguration, authorizationDecodedRequest);

            await BurnTicket(mayhemConfiguration, authorizationDecodedRequest);

            Guid newGameCode = Guid.NewGuid();

            ActiveGameCodesDto gameCodeDto = new()
            {
                Wallet = authorizationDecodedRequest.signedData.Wallet,
                GameCode = newGameCode,
                TournamentId = tournaments.Id,
                CreateDate = DateTime.UtcNow
            };

            await gameCodeRepository.CreateGameCodeAsync(gameCodeDto);

            return new StartGameResponse() { GameCode = newGameCode };
        }

        private async Task BurnTicket(MayhemConfiguration mayhemConfiguration, AuthorizationDecodedRequest authorizationDecodedRequest)
        {
            Contract? web3Contract = web3.Eth.GetContract(mayhemConfiguration.AlturaTournamentAbi, mayhemConfiguration.AlturaTournamentAddress);
            var account = new Nethereum.Web3.Accounts.Account(mayhemConfiguration.PrivateKeyWallet);

            Function function = web3Contract.GetFunction("consumeItem");
            try
            {
                var gass = await function.EstimateGasAsync(account.Address, null, null, authorizationDecodedRequest.signedData.Wallet, new BigInteger(mayhemConfiguration.AlturaTournamentTicketId), new BigInteger(1));
                
                var consumeItemHandler = web3.Eth.GetContractTransactionHandler<ConsumeItemFunction>();
                var consumeItemFunction = new ConsumeItemFunction()
                {
                    From = authorizationDecodedRequest.signedData.Wallet,
                    ItemId = new BigInteger(mayhemConfiguration.AlturaTournamentTicketId),
                    Amount = new BigInteger(1),
                    GasPrice = Nethereum.Web3.Web3.Convert.ToWei(25, UnitConversion.EthUnit.Gwei),
                    Gas = gass.Value,
                };

                var transactionReceipt = await consumeItemHandler.SendRequestAndWaitForReceiptAsync(mayhemConfiguration.AlturaTournamentAddress, consumeItemFunction);

                logger.LogInformation($"Ticket has been burend for Wallet {authorizationDecodedRequest.signedData.Wallet}.");
            }
            catch (Exception ex)
            {
                string errorMessage = $"Ticket burn was failed: {ex.Message}.";
                logger.LogError(ex, errorMessage);
                throw new NotFoundException(new ValidationMessage("BAD_REQUEST", errorMessage));
            }
        }

        [Function("consumeItem")]
        public class ConsumeItemFunction : FunctionMessage
        {
            [Parameter("address", "from", 1)]
            public string From { get; set; }

            [Parameter("uint256", "itemId", 2)]
            public BigInteger ItemId { get; set; }

            [Parameter("uint256", "amount", 3)]
            public BigInteger Amount { get; set; }
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

    }
}
