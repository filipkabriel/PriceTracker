
using Microsoft.Extensions.DependencyInjection;
using PriceTracker.App.Scrapers;

namespace PriceTracker.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Steam Price Tracker ===");

            var services = new ServiceCollection();

            services.AddHttpClient<ISteamPriceScraper, SteamScraper>(client =>
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("PriceTrackerBot/1.0");
            });

            var serviceProvider = services.BuildServiceProvider();
            var scraper = serviceProvider.GetRequiredService<ISteamPriceScraper>();

            // Testovací hry na Steamu:
            // 1091500 = Cyberpunk 2077
            // 292030 = The Witcher 3
            int appId = 1091500;

            try
            {
                Console.WriteLine($"Zjišťuji cenu pro App ID {appId}...");
                var (price, discount) = await scraper.GetPriceAsync(appId);

                Console.WriteLine($"Aktuální cena: {price} EUR");
                Console.WriteLine($"Sleva: {discount} %");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba: {ex.Message}");
            }

            Console.WriteLine("\nStiskni libovolnou klávesu pro ukončení...");
            Console.ReadKey();
        }
    }
}