using Microsoft.Extensions.DependencyInjection;
using PriceTracker.App.Scrapers;

namespace PriceTracker.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddHttpClient<ISteamPriceScraper, SteamScraper>(client =>
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("PriceTrackerBot/1.0");
            });
            await using var serviceProvider = services.BuildServiceProvider();
            var scraper = serviceProvider.GetRequiredService<ISteamPriceScraper>();
            await Run.MainMenu(scraper);
        }
    }
}