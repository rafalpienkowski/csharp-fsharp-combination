using System.ComponentModel.DataAnnotations;

namespace SuperStore.Api.Banks;

public class ReduceRequest : IValidatableObject
{
    public MoneyDto? Augend { get; set; }
    public MoneyDto? Addend { get; set; }
    public string ToCurrency { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Addend is null)
        {
            yield return new ValidationResult("Addend is required", new []{ nameof(Addend)});
        }

        if (Augend is null)
        {
            yield return new ValidationResult("Augend is required", new []{ nameof(Augend)});
        }

        if (!SupportedCurrencies.List.Contains(ToCurrency) ||
            !SupportedCurrencies.List.Contains(Addend?.Currency ?? string.Empty) ||
            !SupportedCurrencies.List.Contains(Augend?.Currency ?? string.Empty)
           )
        {
            yield return new ValidationResult("Not supported currency");
        }
    }
}