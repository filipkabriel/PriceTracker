using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using PriceTracker.App.Scrapers;

namespace PriceTracker.App;

public static class Run
{
    private static readonly string[] selectionSettings = new[] { "Set region", "Manage storage", "Set AI", "Manage app", "Quit"};
    private static string currentLangRegion;
    private static readonly string[] settingsLangRegion = new[] { "&l=czech&cc=CZ", "&l=slovak&cc=SK", "&l=polish&cc=PL", "&l=german&cc=DE", "&l=english&cc=US", "&l=english&cc=GB", "&l=french&cc=FR", "&l=ukrainian&cc=UA", "&l=italian&cc=IT", "&l=japanese&cc=JP" };
    private static readonly string[] selectionWishlist = new[] { "View wishilists", "Manage Wishlists", "Create new wishlist" };
    private static readonly string[] selectionMainMenu = new[] { "Search game", "Wishlist", "History", "Settings", "Quit" };
    private static readonly string[] selectionSearchMenu = new[] { "Search by game name", "Search by game id", "Filter games", "Search by ai" };
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
            switch (Select(selectionMainMenu))
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
        switch (Select(selectionSearchMenu))
        {
            case 0:
                Console.Clear();
                Console.WriteLine("Write name of a game:");
                Console.WriteLine();
                string input = CancelReadInput();
                if (input == null) break;
                var response = await scraper.SearchGameAsync(input, currentLangRegion);
                Console.Clear();
                Console.WriteLine("Founded games:");
                Console.WriteLine();
                for (int i = 0; i < response.Count; i++)
                {
                    Console.WriteLine($"{i+1}. Name: {response[i].Name} Id: {response[i].AppId} Price: {response[i].CurrentPrice} EUR Discount: {response[i].DiscountPercent} %");
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
                    var item = await scraper.SearchGameIdAsync(appId, currentLangRegion);
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
                return;
                Console.Clear();
                break;
            case 3:
                Console.WriteLine("Out of services");
                Console.ReadKey();
                return;
                Console.Clear();
                break;
        }
    }
    private static void Wishlist()
    {
        Console.WriteLine("Out of services");
        Console.ReadKey();
    }
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
            switch (Select(selectionSettings))
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("Set your region");
                    Console.WriteLine();
                    currentLangRegion = settingsLangRegion[Select(settingsLangRegion)];
                    Console.Clear();
                    Console.WriteLine($"Current region: {currentLangRegion}");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
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
                case 3:
                    Console.WriteLine("Out of services");
                    Console.ReadKey();
                    break;
                case 4:
                    active = false;
                    break;
            }
        } while (active);
    }
    
    /// <summary>Select method for selecting choices</summary>
    /// <param name="string[] selection">Contains choices</param>
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