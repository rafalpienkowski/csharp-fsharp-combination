namespace SuperStore;

public record Money(decimal Amount, string Currency) : IExpression
{
    public Money Reduce(Bank bank, string to)
    {
        var rate = bank.Rate(Currency, to);
        return new Money(Math.Round(Amount / rate, 2), to);
    }

    public IExpression Minus(IExpression subtrahend) => new Difference(this, subtrahend);
    public IExpression Times(int multiplier) => this with { Amount = Amount * multiplier };
    public IExpression Plus(IExpression addend) => new Sum(this, addend);

    public static Money Dollar(decimal amount) => new(amount, "USD");
    public static Money Franc(decimal amount) => new(amount, "CHF");

    public override string ToString() => $"{Amount} {Currency}";
}