using PriceTracker.App.Models;

namespace PriceTracker.App.Scrapers;

public interface ISteamPriceScraper
{
    Task<SteamGame> SearchGameIdAsync(int appId);
    Task<List<SteamGame>>SearchGameAsync(string searchTerm);
}
