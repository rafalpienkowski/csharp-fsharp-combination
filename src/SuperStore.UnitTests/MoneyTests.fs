module ``Money should``

open System.Collections.Generic
open SuperStore
open Xunit
open FsUnit.Xunit

[<Fact>]
let ``multiply dollars`` () =
    let fiveDollars: Money = Money.Dollar(5)

    fiveDollars.Times(2) |> should equal (Money.Dollar(10))
    fiveDollars.Times(3) |> should equal (Money.Dollar(15))

[<Fact>]
let ``multiply francs`` () =
    let fiveFrancs: Money = Money.Franc(5)

    fiveFrancs.Times(2) |> should equal (Money.Franc(10))
    fiveFrancs.Times(3) |> should equal (Money.Franc(15))

[<Fact>]
let ``be able to compare between`` () =
    Money.Dollar(5) |> should equal (Money.Dollar(5))
    Money.Dollar(6) |> should not' (equal (Money.Dollar(5)))
    Money.Franc(5) |> should not' (equal (Money.Dollar(5)))

[<Fact>]
let ``contain currency information`` () =
    Money.Dollar(1).Currency |> should equal "USD"
    Money.Franc(1).Currency |> should equal "CHF"
    
[<Fact>]
let ``support addition of money in the same currency`` () =
    let fiveDollars = Money.Dollar(5)
    let sum = fiveDollars.Plus(fiveDollars)
    let bank = Bank()
    let reduced = bank.Reduce(sum, "USD")
    reduced |> should equal (Money.Dollar(10)) 
    
[<Fact>]
let ``support subtraction of money in the same currency`` () =
    let tenDollars = Money.Dollar(10)
    let fourDollars = Money.Dollar(4)
    let diff = tenDollars.Minus(fourDollars)
    let bank = Bank()
    let reduced = bank.Reduce(diff, "USD")
    reduced |> should equal (Money.Dollar(6))
    
[<Fact>]
let ``return sum of addition operation`` () =
    let fiveDollars = Money.Dollar(5)
    let result = fiveDollars.Plus(fiveDollars)
    let sum = result :?> Sum
    sum.Augend |> should equal fiveDollars
    sum.Addend |> should equal fiveDollars
    
[<Fact>]
let ``reduce sum`` () =
    let sum = Sum(Money.Dollar(3), Money.Dollar(4))
    let bank = Bank()
    let result = bank.Reduce(sum, "USD")
    result |> should equal (Money.Dollar(7))
    
[<Fact>]
let ``reduce money`` () =
    let bank = Bank()
    let result = bank.Reduce(Money.Dollar(1), "USD")
    result |> should equal (Money.Dollar(1))

[<Fact>]    
let ``check identity rate`` () =
    1m |> should equal (Bank().Rate("USD", "USD"))
    
[<Fact>]
let ``reduce money in different currencies`` () =
    let bank = Bank()
    bank.AddRate("CHF", "USD", 2)
    let result = bank.Reduce(Money.Franc(2), "USD")
    result |> should equal (Money.Dollar(1))
    
[<Fact>]
let ``reduce money in different currencies with bank's commission`` () =
    let bank = Bank()
    bank.AddRate("CHF", "USD", 2)
    bank.AddCommission(0.05m)
    let result = bank.Reduce(Money.Franc(200), "USD")
    result |> should equal (Money.Dollar(95.24m))
    
[<Fact>]
let ``support addition of mixed currencies`` () =
    let fiveDollars = Money.Dollar(5)
    let tenFrancs = Money.Franc(10)
    let bank = Bank()
    bank.AddRate("CHF", "USD", 2)
    let result = bank.Reduce(fiveDollars.Plus(tenFrancs), "USD")
    result |> should equal (Money.Dollar(10))
 
[<Fact>]
let ``support subtraction of mixed currencies`` () =
    let tenDollars = Money.Dollar(10)
    let tenFrancs = Money.Franc(10)
    let bank = Bank()
    bank.AddRate("CHF", "USD", 2)
    let result = bank.Reduce(tenDollars.Minus(tenFrancs), "USD")
    result |> should equal (Money.Dollar(5))
    
[<Fact>]
let ``support sum with plus money operation`` () =
    let fiveDollars = Money.Dollar(5)
    let tenFrancs = Money.Franc(10)
    let bank = Bank()
    bank.AddRate("CHF", "USD", 2)
    let sum = Sum(fiveDollars, tenFrancs).Plus(fiveDollars)
    let result = bank.Reduce(sum, "USD")
    result |> should equal (Money.Dollar(15))

[<Fact>]
let ``support sum with plus and minus operation`` () =
    let fiveDollars = Money.Dollar(5)
    let tenFrancs = Money.Franc(10)
    let elevenDollars = Money.Dollar(11)
    let bank = Bank()
    bank.AddRate("CHF", "USD", 2)
    let sum = Sum(fiveDollars, tenFrancs).Plus(fiveDollars).Minus(elevenDollars)
    let result = bank.Reduce(sum, "USD")
    result |> should equal (Money.Dollar(4))

[<Fact>]
let ``support sum with times operation`` () =
    let fiveDollars = Money.Dollar(5)
    let tenFrancs = Money.Franc(10)
    let bank = Bank()
    bank.AddRate("CHF", "USD", 2)
    let wallet = Sum(fiveDollars, tenFrancs).Times(2)
    let result = bank.Reduce(wallet, "USD")
    result |> should equal (Money.Dollar(20))
    
[<Fact>]
let ``support difference with times operation`` () =
    let tenDollars = Money.Dollar(10)
    let tenFrancs = Money.Franc(8)
    let bank = Bank()
    bank.AddRate("CHF", "USD", 2)
    let wallet = Difference(tenDollars, tenFrancs).Times(2)
    let result = bank.Reduce(wallet, "USD")
    result |> should equal (Money.Dollar(12))

[<Fact>]
let ``block reduce operation when bank doesn't know the rate`` () =
    let fiveDollars = Money.Dollar(5)
    let tenFrancs = Money.Franc(10)
    let bank = Bank()
    let sum = Sum(fiveDollars, tenFrancs)
    (fun() -> bank.Reduce(sum, "USD") |> ignore) |> should throw typeof<KeyNotFoundException>
    