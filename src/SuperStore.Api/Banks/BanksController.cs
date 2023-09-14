using Microsoft.AspNetCore.Mvc;

namespace SuperStore.Api.Banks;

[ApiController]
[Route("banks")]
public class BanksController : ControllerBase
{
    private readonly Bank _bank = new();
    private readonly ILogger<BanksController> _logger;

    public BanksController(ILogger<BanksController> logger)
    {
        _logger = logger;
        _bank.AddRate("CHF", "USD", 2);
        _bank.AddRate("USD", "CHF", 0.5m);
    }

    [HttpPost(Name = "sum")]
    [Route("{bankName}/sum")]
    public IActionResult Sum([FromRoute] string bankName, [FromBody] ReduceRequest request)
    {
        _logger.LogInformation("Bank {BankName} called for sum", bankName);

        var augend = From(request.Augend!);
        var addend = From(request.Addend!);

        var sum = new Sum(augend, addend);
        var result = _bank.Reduce(sum, request.ToCurrency);

        return Ok(ToDto(result));
    }
    
    private static Money From(MoneyDto dto) =>
        dto.Currency switch
        {
            "USD" => Money.Dollar(dto.Amount),
            "CHF" => Money.Franc(dto.Amount),
            _ => throw new ArgumentOutOfRangeException(nameof(dto))
        };

    private static MoneyDto ToDto(Money money) => new() { Amount = money.Amount, Currency = money.Currency };
}