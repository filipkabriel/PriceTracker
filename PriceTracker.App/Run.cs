using System.Text;
using PriceTracker.App.Models;
using PriceTracker.App.Scrapers;

namespace PriceTracker.App;

public static class Run
{
    private static List<Wishlist> _wishlists = new List<Wishlist>();
    private static readonly string[] SelectionSettings = new[] { "Manage storage", "Set AI", "Manage app"};
    private static readonly string[] SelectionWishlist = new[] { "View and manage wishlists", "Create new wishlist" };
    private static readonly string[] SelectionMainMenu = new[] { "Search game", "Wishlist", "History", "Settings", "Quit" };
    private static readonly string[] SelectionSearchMenu = new[] { "Search by game name", "Search by game id", "Filter games", "Search by ai" };
    /// <summary>
    /// Main menu for price tracker
    /// </summary>
    public static async Task MainMenu(ISteamPriceScraper scraper)
    {
        bool program = true;
        do
        {
            Console.Clear();
            Console.WriteLine(">> CLI Steam PriceTracker <<");
            Thread.Sleep(100);
            Console.WriteLine();
            Console.WriteLine("Pick your action: ");
            Console.WriteLine();
            switch (Select(SelectionMainMenu))
            {
                case 0:
                    Console.Clear();
                    await SearchMenu(scraper);
                    break;
                case 1:
                    Console.Clear();
                    Wishlist();
                    break;
                case 2:
                    Console.Clear();
                    History();
                    break;
                case 3:
                    Console.Clear();
                    Settings();
                    break;
                case 4:
                    program = false;
                    break;
                default: 
                    program = false;
                    break;
            }
        } while (program);
    }
    /// <summary>
    /// Search menu - User will select his action and type input. For dowloading and rendering data
    /// </summary>
    private static async Task SearchMenu(ISteamPriceScraper scraper)
    {
        Console.Clear();
        Console.WriteLine(">> Search Menu <<");
        Thread.Sleep(100);
        Console.WriteLine();
        Console.WriteLine("Pick your action: ");
        Console.WriteLine();
        switch (Select(SelectionSearchMenu))
        {
            case 0:
                Console.Clear();
                Console.WriteLine("Write name of a game:");
                Console.WriteLine();
                string? input = CancelReadInput();
                if (input == null) break;
                var response = await scraper.SearchGameAsync(input);
                Console.Clear();
                Console.WriteLine("Founded games:");
                Console.WriteLine();
                List<string> gameList = new List<string>();
                foreach (var item in response)
                {
                    gameList.Add($"Name: {item.Name} Id: {item.AppId} Price: {item.CurrentPrice} EUR EUR Discount: {item.DiscountPercent} %");
                }
                int index = Select(gameList.ToArray());
                SteamGame game;
                if (index !> response.Count) 
                {
                    game = response[index];
                    Console.Clear();
                    Console.WriteLine("Choose wishlist:");
                    if (_wishlists.Count == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("First you must create wishlist");
                        break;
                    }
                    List<string> wl = new List<string>();
                    foreach (var item in _wishlists)
                    {
                        wl.Add(item.Name);
                    }
                    int indexWl = Select(wl.ToArray());
                    if (indexWl ! > wl.Count)
                    {
                        _wishlists[indexWl].GameList.Add(game);
                        Console.Clear();
                        Console.WriteLine("Successfully added game to wishlist");
                        Console.ReadKey();
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                break;
            case 1:
                Console.Clear();
                Console.WriteLine("Write yours game id:");
                Console.WriteLine();
                try
                {
                    int appId = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine($"Checking price for App ID {appId}...");
                    var item = await scraper.SearchGameIdAsync(appId);
                    Console.Clear();
                    Console.WriteLine($"Name: {item.Name}");
                    Console.WriteLine($"Current price: {item.CurrentPrice} EUR");
                    Console.WriteLine($"Discount: {item.DiscountPercent} %");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                break;
            case 2:
                Console.WriteLine("Out of services");
                Console.ReadKey();
                Console.Clear();
                break;
            case 3:
                Console.WriteLine("Out of services");
                Console.ReadKey();
                Console.Clear();
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private static void Wishlist()
    {
        bool active = true;
        do
        {
            Console.Clear();
            Console.WriteLine(">> Wishlist <<");
            Thread.Sleep(100);
            Console.WriteLine();
            Console.WriteLine("Pick your action: ");
            Console.WriteLine();
            switch (Select(SelectionWishlist))
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("Your wishlists:");
                    Console.WriteLine();
                    List<string> wlList = new List<string>();
                    foreach (var wl in _wishlists)
                    {
                        wlList.Add(wl.Name);
                    }
                    int index =  Select(wlList.ToArray());
                    if (index !> wlList.Count)
                    {
                        Console.Clear();
                        Console.WriteLine($"Manage your {wlList[index]}");
                        Console.ReadKey();
                    }
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case 1:
                    Console.Clear();
                    Console.WriteLine("Write name for your new wishlist:");
                    Console.WriteLine();
                    string? name = CancelReadInput();
                    if (name == null) break;
                    if (name.IsWhiteSpace())
                    {
                        name = $"Wishlist {_wishlists.Count + 1}";
                    }
                    Wishlist wishlist = new Wishlist(name);
                    _wishlists.Add(wishlist);
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                default:
                    active = false;
                    break;
            }
        } while (active);
    }
    /// <summary>
    /// 
    /// </summary>
    private static void History()
    {
        Console.WriteLine("Out of services");
        Console.ReadKey();
    }
    /// <summary>
    /// 
    /// </summary>
    private static void Settings()
    {
        bool active = true;
        do
        {
            Console.Clear();
            Console.WriteLine(">> Settings <<");
            Thread.Sleep(100);
            Console.WriteLine();
            Console.WriteLine("Pick your action: ");
            Console.WriteLine();
            switch (Select(SelectionSettings))
            {
                case 0:
                    Console.WriteLine("Out of services");
                    Console.ReadKey();
                    break;
                case 1:
                    Console.WriteLine("Out of services");
                    Console.ReadKey();
                    break;
                case 2:
                    Console.WriteLine("Out of services");
                    Console.ReadKey();
                    break;
                default:
                    active = false;
                    break;
                    
            }
        } while (active);
    }
    
    /// <summary>Select method for selecting choices</summary>
    /// <param name="selection">Contains choices</param>
    /// <returns>Index of choice in []</returns>
    private static int Select(string[] selection)
    {
        int index = 0;
        ConsoleKey key;
        int top = Console.CursorTop;
        do
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, top);
            for (int i = 0; i < selection.Length; i++)
            {
                if (i == index)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(">> " + selection[i] + " <<");
                }
                else
                {
                    Console.ResetColor();
                    Console.WriteLine("   " + selection[i] + "   ");
                }
            }
            Console.ResetColor();
            key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    index--;
                    if (index < 0) index = selection.Length - 1;
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    index++;
                    if (index >= selection.Length) index = 0;
                    break;
                case ConsoleKey.Escape:
                    index = selection.Length; // for default
                    return index;
            }
        } while (key != ConsoleKey.Enter && key != ConsoleKey.Spacebar);
        return index;
    }
    /// <summary>
    /// Method for input and still be able to write or cancel your input by pressing selected key/s
    /// </summary>
    /// <returns>Users input in string</returns>
    private static string? CancelReadInput()
    {
        Console.CursorVisible = true;
        StringBuilder input = new StringBuilder();
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
            {
                return null;
            }
            if (key.Key == ConsoleKey.Enter)
            {
                return input.ToString();
            }
            if (key.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.Write("\b \b");
                }
            }
            else if (!char.IsControl(key.KeyChar))
            {
                input.Append(key.KeyChar);
                Console.Write(key.KeyChar);
            }
        }
    }
}