
using System.Text.Json;


namespace PriceTracker.App.Scrapers;

public class SteamScraper : ISteamPriceScraper
{
    private readonly HttpClient _httpClient;

    public SteamScraper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(decimal Price, int DiscountPercent)> GetPriceAsync(int appId)
    {
        // cc=cz - price in EUR for Czech
        string url = $"https://store.steampowered.com/api/appdetails?appids={appId}&cc=cz&filters=price_overview";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        // Steam JSON: { "{appId}": { "success": true, "data": { "price_overview": { "final": 1999, "discount_percent": 20 } } } }
        var root = doc.RootElement.GetProperty(appId.ToString());
        bool success = root.GetProperty("success").GetBoolean();

        if (!success)
        {
            throw new Exception($"Nepodařilo se načíst data pro Steam App ID: {appId}");
        }

        var data = root.GetProperty("data");

        if (!data.TryGetProperty("price_overview", out var priceOverview))
        {
            // free game
            return (0m, 0);
        }

        // Steam returns price - 1999 = 19.99 EUR)
        int finalPriceCents = priceOverview.GetProperty("final").GetInt32();
        int discountPercent = priceOverview.GetProperty("discount_percent").GetInt32();

        decimal finalPrice = finalPriceCents / 100m;

        return (finalPrice, discountPercent);
    }

    public async Task<List<(int AppId,string Name)>> SearchGameAsync(string searchTerm)
    {
        List<(int AppId,string Name)> search = new List<(int AppId,string Name)>();
        return search;
    }
}