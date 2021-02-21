using System;
using System.Threading.Tasks;
using Discord;
using FluentResults;
using SmortCat.Domain.Services;

namespace Administration.Services
{
    public class BanService
    {
        private ILogger _logger;

        public BanService(ILogger logger)
        {
            _logger = logger;
        }
        
        public async Task<Result> BanUserAsync(IUser user, IGuild guild)
        {
            try
            {
                await guild.AddBanAsync(user);
                
                _logger.LogInformation($"Banned {user.Id} on {guild.Id}");
                
                return Result.Ok();
            }
            catch (Exception e)
            {
                _logger.LogException("BanUserHandler", e);
                
                return Result.Fail(new ExceptionalError(e));
            }
        }
    }
}