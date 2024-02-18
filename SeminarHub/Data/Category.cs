using Microsoft.EntityFrameworkCore;
using SeminarHub.Data.DataConstants;
using System.ComponentModel.DataAnnotations;

namespace SeminarHub.Data
{
    [Comment("This class represents a category.")]
    public class Category
    {
        [Key]
        [Comment("The unique identifier of the category.")]
        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryConstants.NameMaxLength)]
        [Comment("The name of the category.")]
        public string Name { get; set; }=string.Empty;

        [Comment("The seminars of the category.")]
        public ICollection<Seminar> Seminars { get; set; } = new List<Seminar>();
    }
}