namespace SuperStore;

public record Sum(IExpression Augend, IExpression Addend) : IExpression
{
    public Money Reduce(Bank bank, string to)
    {
        var amount = Augend.Reduce(bank, to).Amount + Addend.Reduce(bank, to).Amount;
        return new Money(amount, to);
    }

    public IExpression Plus(IExpression addend) => new Sum(this, addend);
    public IExpression Minus(IExpression subtrahend) => new Difference(this, subtrahend);

    public IExpression Times(int multiplier) => new Sum(Augend.Times(multiplier), Addend.Times(multiplier));
}