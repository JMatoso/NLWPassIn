using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassIn.Infrastructure.Entities;

[Table("CheckIns")]
public class CheckIn
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public Guid? AttendeeId { get; set; }

    [ForeignKey("AttendeeId")]
    public Attendee Attendee { get; set; } = default!;
}