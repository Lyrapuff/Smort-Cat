using System.ComponentModel.DataAnnotations;

namespace SmortCat.Domain.Services.Persistence.Models
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}