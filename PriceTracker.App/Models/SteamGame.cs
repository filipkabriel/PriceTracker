namespace PriceTracker.App.Models;

public class SteamGame
{
    public int AppId { get; set; }
    public string Name { get; set; }
    public decimal TargetPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public int DiscountPercent { get; set; }
    public List<Wishlist> Wishlist { get; set; } 
    public SteamGame(int appId,  string name, decimal currentPrice, int discountPercent)
    {
        this.AppId = appId;
        this.Name = name;
        this.CurrentPrice = currentPrice;
        this.DiscountPercent = discountPercent;
    }
}