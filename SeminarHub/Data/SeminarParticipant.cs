using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeminarHub.Data
{
    [Comment("This class represents a seminar participant.")]
    public class SeminarParticipant
    {
        [Required]
        [Comment("The seminar identifier.")]
        public int SeminarId { get; set; }

        [ForeignKey("SeminarId")]
        public Seminar Seminar { get; set; } = null!;

        [Required]
        [Comment("The participant identifier.")]
        public string ParticipantId { get; set; } = string.Empty;

        [ForeignKey("ParticipantId")]
        public IdentityUser Participant { get; set; } = null!;
    }
}
