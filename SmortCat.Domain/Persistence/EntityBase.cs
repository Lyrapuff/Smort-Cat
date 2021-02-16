using System.ComponentModel.DataAnnotations;

namespace SmortCat.Domain.Persistence
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}