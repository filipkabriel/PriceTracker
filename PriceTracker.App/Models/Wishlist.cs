
namespace PriceTracker.App.Models;

public class Wishlist
{
    public string Name { get; set; }
    public List<SteamGame> GameList { get; set; } = new List<SteamGame>();
    public Wishlist(string name)
    {
        this.Name = name;
    }
}