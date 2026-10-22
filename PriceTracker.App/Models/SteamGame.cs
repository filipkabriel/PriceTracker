namespace PriceTracker.App.Models;

public class SteamGame
{
    public int AppId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal TargetPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public int DiscountPercent { get; set; }
}