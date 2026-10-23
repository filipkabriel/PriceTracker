namespace PriceTracker.App.Scrapers;

public interface ISteamPriceScraper
{
    Task<(decimal Price, int DiscountPercent)> GetPriceAsync(int appId);
    Task<List<(int AppId,string Name)>>SearchGameAsync(string searchTerm);
}
