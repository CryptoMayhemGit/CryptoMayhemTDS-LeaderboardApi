﻿using AutoMapper;
using Mayhem.Dal.Context;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Repositories.Interfaces;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;

namespace Mayhem.Dal.Repositories.Implementations
{
    public class GameCodeRepository(MayhemDataContext mayhemDataContext, IMapper mapper) : IGameCodeRepository
    {
        public async Task CreateGameCodeAsync(ActiveGameCodesDto gameCodeDto)
        {
            await mayhemDataContext.ActiveGameCodes.AddAsync(mapper.Map<ActiveGameCodes>(gameCodeDto));
            await mayhemDataContext.SaveChangesAsync();
        }

        public async Task<bool> IsGameCodeActiveAsync(string wallet, Guid gameCode, int tournamentId)
        {
            return await mayhemDataContext
            .ActiveGameCodes
            .AsNoTracking()
            .Where(x => x.Wallet == wallet)
            .Where(x => x.GameCode == gameCode)
            .Where(x => x.TournamentId == tournamentId)
            .AnyAsync();
        }

        public async Task RemoveAsync(Guid gameCode)
        {
            var itemToRemove = await mayhemDataContext.ActiveGameCodes.SingleOrDefaultAsync(x => x.GameCode == gameCode);

            if (itemToRemove != null)
            {
                mayhemDataContext.ActiveGameCodes.Remove(itemToRemove);
                await mayhemDataContext.SaveChangesAsync();
            }
        }
    }
}
