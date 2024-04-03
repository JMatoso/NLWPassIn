using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassIn.Infrastructure.Entities;

[Table("Events")]
public class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.MultilineText)]
    public string Details { get; set; } = string.Empty;

    private string slug = string.Empty;

    [Required]
    public string Slug 
    {   
        get => slug; 
        set => slug = value.ToLower().Replace(" ", "-"); 
    }

    [Range(1, int.MaxValue)]
    public int MaximumAttendees { get; set; }

    [ForeignKey("EventId")]
    public List<Attendee> Attendees { get; set; } = [];
}