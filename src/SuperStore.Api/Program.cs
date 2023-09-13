using Microsoft.AspNetCore;

namespace SuperStore.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder()
            .UseStartup<Startup>();
    }
}