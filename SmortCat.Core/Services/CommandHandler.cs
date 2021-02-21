using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using SmortCat.Domain.Services;
using SmortCat.Domain.Services.Obstacles;

namespace SmortCat.Core.Services
{
    public class CommandHandler : ICommandHandler
    {
        private ILogger _logger;
        private DiscordSocketClient _client;
        private CommandService _commandService;
        private IServiceProvider _provider;
        private IEnumerable<IEarlyObstacle> _earlyObstacles;
        private IEnumerable<ILateObstacle> _lateObstacles;

        public CommandHandler(ILogger logger, DiscordSocketClient client, CommandService commandService, IServiceProvider provider, IEnumerable<IEarlyObstacle> earlyObstacles, IEnumerable<ILateObstacle> lateObstacles)
        {
            _logger = logger;
            _client = client;
            _commandService = commandService;
            _provider = provider;
            _earlyObstacles = earlyObstacles;
            _lateObstacles = lateObstacles;
        }
        
        public void Start()
        {
            _client.MessageReceived += MessageReceived;
        }

        private string GetPrefix()
        {
            // TODO loading prefix from config.
            
            return "c!";
        }

        private async Task<bool> TryPassObstacles(IEnumerable<IObstacle> obstacles, CommandContext context)
        {
            foreach (IObstacle obstacle in obstacles)
            {
                if (!await obstacle.TryPass(context))
                {
                    return false;
                }
            }

            return true;
        }
        
        private async Task MessageReceived(SocketMessage message)
        {
            try
            {
                if (message.Author.IsBot)
                {
                    return;
                }

                if (!(message is SocketUserMessage userMessage))
                {
                    return;
                }

                string prefix = GetPrefix();

                if (!message.Content.StartsWith(prefix))
                {
                    return;
                }

                string input = message.Content.Substring(prefix.Length);
                CommandContext context = new(_client, userMessage);

                if (!await TryPassObstacles(_earlyObstacles, context))
                {
                    return;
                }
                
                if (!await TryPassObstacles(_lateObstacles, context))
                {
                    return;
                }
                
                await ExecuteCommand(input, context);
            }
            catch (Exception e)
            {
                _logger.LogException("CommandHandler", e);
            }
        }

        private async Task ExecuteCommand(string input, CommandContext context)
        {
            SearchResult searchResult = _commandService.Search(context, input);

            if (!searchResult.IsSuccess)
            {
                return;
            }

            CommandMatch commandMatch = searchResult.Commands[0];
            PreconditionResult preconditionResult = await commandMatch.CheckPreconditionsAsync(context, _provider);

            if (!preconditionResult.IsSuccess)
            {
                return;
            }

            ParseResult parseResult =
                await commandMatch.ParseAsync(context, searchResult, preconditionResult, _provider);

            await commandMatch.ExecuteAsync(context, parseResult, _provider);
        }
    }
}