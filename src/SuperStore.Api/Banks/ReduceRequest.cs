namespace SuperStore.Api.Banks;

public class ReduceRequest
{
    public MoneyDto? Augend { get; set; }
    public MoneyDto? Addend { get; set; }
    public string ToCurrency { get; set; } = string.Empty;
}