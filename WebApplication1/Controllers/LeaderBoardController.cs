using Mayhem.Bl.Services.Interfaces;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Mayhem.LeaderBoardApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeaderBoardController(ITournamentService tournamentService, IUserStatisticsService userStatisticsService) : ControllerBase
    {
        [HttpGet(Name = "GetGameStats")]
        [ProducesResponseType(typeof(StatsResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetStats()
        {
            StatsResponse statsResponse = await userStatisticsService.GetGameStatsAsync();
            return CreatedAtAction(nameof(GetStats), statsResponse);
        }

        [Route("GetPlayerPoints")]
        [HttpGet]
        [ProducesResponseType(typeof(PlayerPointsResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetPlayerPoints([FromQuery] PlayerPointsRequest playerPointsRequest)
        {
            PlayerPointsResponse playerPointsResponse = await userStatisticsService.GetPlayerPointsAsync(playerPointsRequest);
            return CreatedAtAction(nameof(GetPlayerPoints), playerPointsResponse);
        }

        [Route("StartGame")]
        [HttpPost]
        [ProducesResponseType(typeof(StartGameResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> StartGame([FromBody] StartGameRequest request)
        {
            StartGameResponse response = await userStatisticsService.StartGameAsync(request);
            return CreatedAtAction(nameof(StartGame), response);
        }

        [Route("FinishGame")]
        [HttpPost]
        [ProducesResponseType(typeof(FinishGameResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> FinishGame([FromBody] FinishGameRequest request)
        {
            FinishGameResponse response = await userStatisticsService.FinishGameAsync(request);
            return CreatedAtAction(nameof(FinishGame), response);
        }

        [Route("GetActiveTournament")]
        [HttpGet]
        [ProducesResponseType(typeof(GetActiveTournamentResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetActiveTournament()
        {
            GetActiveTournamentResponse getActiveTournamentResponse = await tournamentService.GetActiveTournamentAsync();
            return CreatedAtAction(nameof(GetActiveTournament), getActiveTournamentResponse);
        }

        [Route("IsAnyTicketTournamentActive")]
        [HttpGet]
        [ProducesResponseType(typeof(IsAnyTicketTournamentActiveResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> IsAnyTicketTournamentActive([FromQuery] IsAnyTicketTournamentActiveRequest request)
        {
            IsAnyTicketTournamentActiveResponse isAnyTicketTournamentActiveResponse = await tournamentService.IsAnyTicketTournamentActiveAsync(request.Ticket);
            return CreatedAtAction(nameof(IsAnyTicketTournamentActive), isAnyTicketTournamentActiveResponse);
        }

        [Route("GetArchivedTournaments")]
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(GetArchivedTournamentsResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetArchivedTournaments()
        {
            GetArchivedTournamentsResponse getArchivedTournamentsResponse = await tournamentService.GetArchivedTournamentsAsync();
            return CreatedAtAction(nameof(GetArchivedTournaments), getArchivedTournamentsResponse);
        }

        [Route("AddTournament")]
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(AddTournamentResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> AddTournament([FromBody] AddTournamentRequest request)
        {
            AddTournamentResponse response = await tournamentService.AddTournamentAsync(request);
            return CreatedAtAction(nameof(AddTournament), response);
        }

        [Route("EndTournament")]
        [Authorize]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> EndTournament()
        {
            await tournamentService.EndTournamentAsync();
            return Ok();
        }

        [Route("TryEndTournament")]
        [Authorize]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> TryEndTournament()
        {
            await tournamentService.TryEndTournamentAsync();
            return Ok();
        }

        [Route("UpdateTournament")]
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(UpdateTournamentResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateTournament([FromBody] UpdateTournamentRequest request)
        {
            UpdateTournamentResponse response = await tournamentService.UpdateTournamentAsync(request);
            return CreatedAtAction(nameof(UpdateTournament), response);
        }
    }
}
