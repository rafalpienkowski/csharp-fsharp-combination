using System.ComponentModel.DataAnnotations;

namespace SuperStore.Api.Banks;

public class MoneyDto : IValidatableObject
{
    public string Currency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Amount < 0)
        {
            yield return new ValidationResult("Money has to be greater than 0", new []{ nameof(Amount)});
        }
        
        if (!SupportedCurrencies.List.Contains(Currency))
        {
            yield return new ValidationResult("Not supported currency");
        }
    }
}