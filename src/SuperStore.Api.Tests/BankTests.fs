module ``Bank NPB should``

open System.Text.Json
open Xunit
open System.Net.Http
open FsUnit.Xunit
open Money.Api.Tests.TestFunctions

type MoneyDto = { currency: string; amount: decimal }

type SumMoneyRequest =
    { augend: MoneyDto
      addend: MoneyDto
      toCurrency: string }

let takeResult (httpResponse: HttpResponseMessage) =
    let content = httpResponse.Content.ReadAsStringAsync().Result
    JsonSerializer.Deserialize<MoneyDto> content

let dollars amount = { currency = "USD"; amount = amount }
let francs amount = { currency = "CHF"; amount = amount }

let twoDollars = dollars 2m
let threeDollars = dollars 3m
let fiveDollars = dollars 5m
let sevenDollars = dollars 7m
let tenFrancs = francs 10m

let take addend augend =
    let request = { toCurrency = "USD"; augend = augend; addend = addend }
    createPostRequest "/banks/nbp/sum" request

[<Fact>]
let ``calculate sum for money in the same currency`` () =
    take twoDollars threeDollars
    |> callServer
    |> takeResult
    |> should equal fiveDollars

[<Fact>]
let ``calculate sum of 2 USD and 10 CHF in USD`` () =
    take twoDollars tenFrancs
    |> callServer
    |> takeResult
    |> should equal sevenDollars
