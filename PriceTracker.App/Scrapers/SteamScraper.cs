using System.Text.Json;
using PriceTracker.App.Models;


namespace PriceTracker.App.Scrapers;

public class SteamScraper : ISteamPriceScraper
{
    private readonly string _langRegio = "&l=english&cc=CZ";
    private readonly HttpClient _httpClient;
    public SteamScraper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<SteamGame> SearchGameIdAsync(int appId)
    {
        string url = $"https://store.steampowered.com/api/appdetails?appids={appId}{_langRegio}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement.GetProperty(appId.ToString());
        bool success = root.GetProperty("success").GetBoolean();
        if (!success)
        {
            throw new Exception($"Failed to load data for Steam App ID: {appId}");
        }
        var data = root.GetProperty("data");
        decimal finalPrice = 0m;
        int discountPercent = 0;
        if (data.TryGetProperty("price_overview", out var priceElement))
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
        string name = data.GetProperty("name").GetString() ?? string.Empty;
        return new SteamGame(appId, name, finalPrice, discountPercent);
    }
    public async Task<List<SteamGame>> SearchGameAsync(string searchTerm)
    {
        var results = new List<SteamGame>();
        if (string.IsNullOrWhiteSpace(searchTerm)) return results;
        string encodedTerm = Uri.EscapeDataString(searchTerm);
        string url = $"https://store.steampowered.com/api/storesearch/?term={encodedTerm}{_langRegio}";
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

                results.Add(new SteamGame(id, name, finalPrice, discountPercent));
            }
        }
        return results;
    }
}