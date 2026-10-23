
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
            throw new Exception($"Failed to load data for Steam App ID: {appId}");
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

    public async Task<List<(int AppId, string Name, decimal Price, int DiscountPercent)>> SearchGameAsync(string searchTerm)
    {
        
        var results = new List<(int AppId, string Name, decimal Price, int DiscountPercent)>();
        if (string.IsNullOrWhiteSpace(searchTerm))  return results;
        string encodedTerm = Uri.EscapeDataString(searchTerm);
        string url = "https://store.steampowered.com/api/storesearch/?term=";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        if (doc.RootElement.TryGetProperty("items", out var itemsElement) && 
            itemsElement.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in itemsElement.EnumerateArray())
            {
                int id = item.GetProperty("id").GetInt32();
                string name = item.GetProperty("name").GetString() ?? string.Empty;
                decimal finalPrice = 0m;
                int discountPercent = 0;
                if (item.TryGetProperty("price", out var priceElement))
                {
                    if (priceElement.TryGetProperty("final", out var finalProp))
                    {
                        finalPrice = finalProp.GetInt32() / 100m;
                    }

                    if (priceElement.TryGetProperty("discount_percent", out var discountProp))
                    {
                        discountPercent = discountProp.GetInt32();
                    }
                }
                results.Add((id, name,  finalPrice, discountPercent));
            }
        }
        return results;
    }
}