using System.Text.Json.Serialization;

namespace PassIn.Communication.Responses;

public class ResponseEventJson
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int MaximumAttendees { get; set; }
    public int AttendeesAmount { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IEnumerable<ResponseAttendeeJson> Attendees { get; set; } = default!;
}
