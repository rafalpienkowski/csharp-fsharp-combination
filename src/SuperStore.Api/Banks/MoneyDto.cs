namespace SuperStore.Api.Banks;

public class MoneyDto
{
    public string Currency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}