﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using SmortCat.Domain.Services;

namespace SmortCat.Core.Services
{
    public class CommandHandler : ICommandHandler
    {
        private ILogger _logger;
        private DiscordSocketClient _client;
        private CommandService _commandService;
        private IServiceProvider _provider;

        public CommandHandler(ILogger logger, DiscordSocketClient client, CommandService commandService, IServiceProvider provider)
        {
            _logger = logger;
            _client = client;
            _commandService = commandService;
            _provider = provider;
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

                // TODO blockers

                string prefix = GetPrefix();

                if (!message.Content.StartsWith(prefix))
                {
                    return;
                }

                string input = message.Content.Substring(prefix.Length);
                CommandContext context = new(_client, userMessage);

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