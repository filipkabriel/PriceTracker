namespace PriceTracker.App.Scrapers;

public interface ISteamPriceScraper
{
    Task<(decimal Price, int DiscountPercent)> GetPriceAsync(int appId);
}
