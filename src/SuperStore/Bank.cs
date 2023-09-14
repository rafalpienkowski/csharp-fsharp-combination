namespace SuperStore;

public class Bank
{
    private readonly Dictionary<RatePair, decimal> _rates = new();
    private decimal _commission = 0;

    public Money Reduce(IExpression source, string to) => source.Reduce(this, to);

    public void AddRate(string from, string to, decimal value)
    {
        _rates.Add(new RatePair(from, to), value);
    }

    public void AddCommission(decimal commission) => _commission = commission;

    public decimal Rate(string from, string to)
    {
        return from == to ? 1 : Math.Round((1 + _commission) * _rates[new RatePair(from, to)], 2);
    }

    private sealed record RatePair(string From, string To);
}