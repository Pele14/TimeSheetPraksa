using System.ComponentModel.DataAnnotations;

namespace TimeSheet.Application.Requests.TimeEntryRequests;

public class GetTimeEntriesSummaryRequest : IValidatableObject
{
    public required DateOnly From { get; set; }
    public required DateOnly To { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (From > To)
            yield return new ValidationResult("From must be before To.", new[] { nameof(From) });

        if (To.DayNumber - From.DayNumber > 42)
            yield return new ValidationResult("Period cannot be longer than 42 days.", new[] { nameof(To) });
    }
}
