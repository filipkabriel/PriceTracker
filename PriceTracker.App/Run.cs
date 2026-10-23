using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.SqlTypes;

namespace PriceTracker.App;

public static class Run
{
    private static readonly string[] selectionMainMenu = new[] { "Search game", "Wishlist", "Storage", "Settings" };
    public static void MainMenu()
    {
        Console.WriteLine(">> CLI Steam PriceTracker <<");
        Thread.Sleep(500);
        Console.WriteLine();
        Console.WriteLine("Pick your action: ");
        Console.WriteLine();
        switch (Select(selectionMainMenu))
        {
            case 0:
                SearchGame();
                break;
            case 1:
                Wishlist();
                break;
            case 2:
                Storage();
                break;
            case 3:
                Settings();
                break;
                
        }
    }
    private static async Task SearchGame()
    {
        
    }
    private static void Wishlist()
    {
        
    }
    private static void Storage()
    {
        
    }
    private static void Settings()
    {
        
    }
    private static int Select(string[] selection)
    {
        int id = 0;
        ConsoleKey key;
        int top = Console.CursorTop;
        do
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, top);
            for (int i = 0; i < selection.Length; i++)
            {
                if (i == id)
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
            key = Console.ReadKey().Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    id--;
                    if (id < 0) id = selection.Length - 1;
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    id++;
                    if (id >= selection.Length) id = 0;
                    break;
            }
        } while (key != ConsoleKey.Enter && key != ConsoleKey.Spacebar);
        return id;
    }
}