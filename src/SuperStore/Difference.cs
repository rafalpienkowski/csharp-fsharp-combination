namespace SuperStore;

public record Difference(IExpression Minuend, IExpression Subtrahend) : IExpression
{
    public Money Reduce(Bank bank, string to)
    {
        var amount = Minuend.Reduce(bank, to).Amount - Subtrahend.Reduce(bank, to).Amount;
        return new Money(amount, to);
    }

    public IExpression Plus(IExpression addend) => new Sum(this, addend);

    public IExpression Minus(IExpression subtrahend) => new Difference(this, subtrahend);

    public IExpression Times(int multiplier) => new Difference(Minuend.Times(multiplier), Subtrahend.Times(multiplier));
}