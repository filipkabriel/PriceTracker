using System.Text.Json;
using PriceTracker.App.Models;
using PriceTracker.App.Scrapers;

namespace PriceTracker.App.Models;

public static class PriceHistoryManager
{
    private static readonly string FilePath = "price_history.json";
    private static readonly SemaphoreSlim FileLock = new(1, 1);
    private static CancellationTokenSource? _cts;
    /// <summary>
    /// 
    /// </summary>
    public static void StartBackgroundTracking(ISteamPriceScraper scraper, List<Wishlist> wishlists, TimeSpan interval)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        _ = Task.Run(async () =>
        {
            using var timer = new PeriodicTimer(interval);
            while (await timer.WaitForNextTickAsync(token))
            {
                await SaveCurrentPricesAsync(scraper, wishlists);
            }
        }, token);
    }
    /// <summary>
    /// 
    /// </summary>
    public static async Task SaveCurrentPricesAsync(ISteamPriceScraper scraper, List<Wishlist> wishlists)
    {
        var uniqueGames = wishlists
            .SelectMany(w => w.GameList)
            .DistinctBy(g => g.AppId)
            .ToList();
        if (uniqueGames.Count == 0) return;
        var records = await LoadHistoryAsync();
        foreach (var game in uniqueGames)
        {
            try
            {
                var updated = await scraper.SearchGameIdAsync(game.AppId);
                records.Add(new PriceRecord(
                    updated.AppId,
                    updated.Name,
                    updated.CurrentPrice,
                    DateTime.Now
                ));
                await Task.Delay(300);
            }
            catch
            {
                
            }
        }
        await FileLock.WaitAsync();
        try
        {
            var json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FilePath, json);
        }
        finally
        {
            FileLock.Release();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static async Task<List<PriceRecord>> LoadHistoryAsync()
    {
        await FileLock.WaitAsync();
        try
        {
            if (!File.Exists(FilePath)) return new List<PriceRecord>();
            var json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<PriceRecord>>(json) ?? new List<PriceRecord>();
        }
        catch
        {
            return new List<PriceRecord>();
        }
        finally
        {
            FileLock.Release();
        }
    }
}