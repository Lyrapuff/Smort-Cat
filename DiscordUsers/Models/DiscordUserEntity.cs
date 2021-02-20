using SmortCat.Domain.Persistence;

namespace DiscordUsers.Models
{
    public class DiscordUserEntity : EntityBase
    {
        public ulong DiscordId { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
    }
}