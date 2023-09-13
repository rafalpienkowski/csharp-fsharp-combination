using Microsoft.AspNetCore.Mvc;

namespace SuperStore.Api.Banks;

[ApiController]
[Route("banks")]
public class BanksController : ControllerBase
{
    private readonly Bank _bank = new();
    private readonly List<string> _supportedCurrencies = new() { "USD", "CHF" };
    private readonly ILogger<BanksController> _logger;

    public BanksController(ILogger<BanksController> logger)
    {
        _logger = logger;
        _bank.AddRate("CHF", "USD", 2);
    }

    [HttpPost(Name = "sum")]
    [Route("{bankName}/sum")]
    public IActionResult Sum([FromRoute] string bankName, [FromBody] ReduceRequest request)
    {
        _logger.LogInformation("Bank {BankName} called for sum", bankName);

        if (request.Addend is null)
        {
            return BadRequest("Addend is required");
        }

        if (request.Augend is null)
        {
            return BadRequest("Augend is required");
        }

        if (!_supportedCurrencies.Contains(request.ToCurrency) ||
            !_supportedCurrencies.Contains(request.Addend?.Currency ?? string.Empty) ||
            !_supportedCurrencies.Contains(request.Augend?.Currency ?? string.Empty)
           )
        {
            return BadRequest("Not supported currency");
        }

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