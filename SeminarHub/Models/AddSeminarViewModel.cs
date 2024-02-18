using SeminarHub.Data.DataConstants;
using System.ComponentModel.DataAnnotations;

namespace SeminarHub.Models
{
    public class AddSeminarViewModel
    {
        [Required(ErrorMessage = ErrorConstants.RequireErrorMessage)]
        [StringLength(SeminarConstants.TopicMaxLength, MinimumLength = SeminarConstants.TopicMinLength, ErrorMessage = ErrorConstants.StringLengthErrorMessage)]
        public string Topic { get; set; } = string.Empty;

        [Required(ErrorMessage = ErrorConstants.RequireErrorMessage)]
        [StringLength(SeminarConstants.LecturerMaxLength, MinimumLength = SeminarConstants.LecturerMinLength, ErrorMessage = ErrorConstants.StringLengthErrorMessage)]
        public string Lecturer { get; set; } = string.Empty;

        [Required(ErrorMessage = ErrorConstants.RequireErrorMessage)]
        [StringLength(SeminarConstants.DetailsMaxLength, MinimumLength = SeminarConstants.DetailsMinLength, ErrorMessage = ErrorConstants.StringLengthErrorMessage)]
        public string Details { get; set; } = string.Empty;

        [Required(ErrorMessage = ErrorConstants.RequireErrorMessage)]
        [Display(Name = SeminarConstants.SeminarDateFormat)]
        public String DateAndTime { get; set; } = String.Empty;
        public string? Duration { get; set; }

        [Required(ErrorMessage = ErrorConstants.RequireErrorMessage)]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

    }
}
