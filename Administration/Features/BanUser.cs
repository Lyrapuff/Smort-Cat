using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using FluentResults;
using MediatR;
using SmortCat.Domain.Services;

namespace Administration.Features
{
    public class BanUser : IRequest<Result>
    {
        public IUser User { get; }
        public IGuild Guild { get; }

        public BanUser(IUser user, IGuild guild)
        {
            User = user;
            Guild = guild;
        }
    }
    
    public class BanUserHandler : IRequestHandler<BanUser, Result>
    {
        private ILogger _logger;

        public BanUserHandler(ILogger logger)
        {
            _logger = logger;
        }


        public async Task<Result> Handle(BanUser request, CancellationToken cancellationToken)
        {
            try
            {
                await request.Guild.AddBanAsync(request.User);
                _logger.LogInformation($"Banned {request.User.Id} on {request.Guild.Id}");
                
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