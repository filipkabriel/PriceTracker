using PriceTracker.App.Models;

namespace PriceTracker.App.Scrapers;

public interface ISteamPriceScraper
{
    Task<(decimal Price, int DiscountPercent)> GetPriceAsync(int appId);
    Task<List<SteamGame>>SearchGameAsync(string searchTerm);
}
