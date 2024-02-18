using MessagePack;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data.DataConstants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;



namespace SeminarHub.Data
{
    [Comment("This class represents a seminar.")]
    public class Seminar
    {
        [Key]
        [Comment("The unique identifier of the seminar.")]
        public int Id { get; set; }

        [Required]
        [MaxLength(SeminarConstants.TopicMaxLength)]
        [Comment("The topic of the seminar.")]
        public string Topic { get; set; }=string.Empty;

        [Required]
        [MaxLength(SeminarConstants.LecturerMaxLength)]
        [Comment("The lecturer of the seminar.")]
        public string Lecturer { get; set; }=string.Empty;

        [Required]
        [MaxLength(SeminarConstants.DetailsMaxLength)]
        [Comment("The details of the seminar.")]
        public string Details { get; set; }=string.Empty;

        [Required]
        [Comment("The organizer identifier of the seminar.")]
        public string OrganizerId { get; set; }=string.Empty;

        [ForeignKey("OrganizerId")]
        public IdentityUser Organizer { get; set; }=null!;

        [Required]
        [Comment("The date and time of the seminar.")]
        public DateTime DateAndTime { get; set; }

        [Comment("The duration of the seminar in minutes.")]
        [MaxLength(SeminarConstants.DurationMaxValue)]
        public int? Duration { get; set; }

        [Required]
        [Comment("The Category identifier of the seminar.")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [Comment("The category of the seminar.")]
        public Category Category { get; set; }=null!;

        [Comment("The participants of the seminar.")]
        public ICollection<SeminarParticipant> SeminarsParticipants { get; set; } = new List<SeminarParticipant>();
    }
}
