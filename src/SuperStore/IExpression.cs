namespace SuperStore;

public interface IExpression
{
    Money Reduce(Bank bank, string to);
    IExpression Plus(IExpression addend);
    IExpression Minus(IExpression subtrahend);
    IExpression Times(int multiplier);
}