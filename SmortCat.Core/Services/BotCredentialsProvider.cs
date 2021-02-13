using System;
using System.IO;
using Newtonsoft.Json;
using SmortCat.Domain.Models;
using SmortCat.Domain.Services;

namespace SmortCat.Core.Services
{
    public class BotCredentialsProvider : IBotCredentialsProvider
    {
        private ILogger _logger;
        
        private BotCredentials _credentials;

        public BotCredentialsProvider(ILogger logger)
        {
            _logger = logger;
        }
        
        public BotCredentials GetCredentials()
        {
            if (_credentials == null)
            {
                LoadCredentials();
            }

            return _credentials;
        }

        private void LoadCredentials()
        {
            const string path = "Credentials.json";

            if (!File.Exists(path))
            {
                _logger.LogError("Can't load credentials, no Credentials.json found.");
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                _credentials = JsonConvert.DeserializeObject<BotCredentials>(json);
            }
            catch (Exception e)
            {
                _logger.LogError($"An exception occured while loading credentials.{Environment.NewLine}{e}");
            }
        }
    }
}