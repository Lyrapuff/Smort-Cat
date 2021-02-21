using SmortCat.Domain.Services.Persistence.Models;

namespace DiscordUsers.Persistence.Models
{
    public class DiscordUserEntity : EntityBase
    {
        public ulong DiscordId { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
    }
}