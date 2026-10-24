using PriceTracker.App.Models;

namespace PriceTracker.App.Scrapers;

public interface ISteamPriceScraper
{
    Task<SteamGame> SearchGameIdAsync(int appId, string region);
    Task<List<SteamGame>>SearchGameAsync(string searchTerm,  string region);
}
