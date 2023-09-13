module Money.Api.Tests.TestFunctions

open System.Net.Http
open System.Text
open System.Text.Json
open Microsoft.AspNetCore.TestHost
open SuperStore.Api

let getTestHost () =
    Program.CreateHostBuilder([|  |])

let callServer (request: HttpRequestMessage) =
    let resp =
        task {
            use server = new TestServer(getTestHost())
            use client = server.CreateClient()
            let! response = request |> client.SendAsync
            return response
        }

    resp.Result

let createPostRequest (url: string) dto =
    let httpRequest = new HttpRequestMessage(HttpMethod.Post, url)

    let json = JsonSerializer.Serialize(dto)
    let content = new StringContent(json, UnicodeEncoding.UTF8, "application/json")
    httpRequest.Content <- content
    httpRequest