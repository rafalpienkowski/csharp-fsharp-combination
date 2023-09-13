module ``Ping should``

open System.Net
open System.Net.Http
open Xunit
open FsUnit.Xunit
open Money.Api.Tests.TestFunctions

[<Fact>]
let ``accept request`` () =
    let response = callServer (new HttpRequestMessage(HttpMethod.Get, "/ping"))
    response.StatusCode |> should equal HttpStatusCode.OK
    
[<Fact>]
let ``return pong`` () =
    let response = callServer (new HttpRequestMessage(HttpMethod.Get, "/ping"))
    response.StatusCode |> should equal HttpStatusCode.OK
    response.Content.ReadAsStringAsync().Result |> should equal "pong"