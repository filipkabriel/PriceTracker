using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using PriceTracker.App.Scrapers;

namespace PriceTracker.App;

public static class Run
{
    private static readonly string[] selectionMainMenu = new[] { "Search game", "Wishlist", "History", "Settings", "Quit" };
    private static readonly string[] selectionSearchMenu = new[] { "Search by game name", "Search by game id", "Filter games", "Search by ai" };
    /// <summary>
    /// Main menu for price tracker
    /// </summary>
    public static void MainMenu()
    {
        bool program = true;
        do
        {
            Console.WriteLine(">> CLI Steam PriceTracker <<");
            Thread.Sleep(100);
            Console.WriteLine();
            Console.WriteLine("Pick your action: ");
            Console.WriteLine();
            switch (Select(selectionMainMenu))
            {
                case 0:
                    Console.Clear();
                    SearchGame();
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
    private static async Task SearchGame()
    {
        var services = new ServiceCollection();
        services.AddHttpClient<ISteamPriceScraper, SteamScraper>(client =>
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("PriceTrackerBot/1.0");
        });
        var serviceProvider = services.BuildServiceProvider();
        var scraper = serviceProvider.GetRequiredService<ISteamPriceScraper>();
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
                var response = scraper.SearchGameAsync(CancelReadInput());
                for (int i = 0; i < response.Result.Count; i++)
                {
                    Console.WriteLine(response.Result[i]);
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
                    var (price, discount) = await scraper.GetPriceAsync(appId);

                    Console.WriteLine($"Current price: {price} EUR");
                    Console.WriteLine($"Discount: {discount} %");
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
    private static void Settings()
    {
        Console.WriteLine("Out of services");
        Console.ReadKey();
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