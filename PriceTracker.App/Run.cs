using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.SqlTypes;

namespace PriceTracker.App;

public static class Run
{
    private static readonly string[] selectionMainMenu = new[] { "Search game", "Wishlist", "Storage", "Settings", "Quit" };
    public static void MainMenu()
    {
        bool program = true;
        do
        {
            Console.WriteLine(">> CLI Steam PriceTracker <<");
            Thread.Sleep(500);
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
                    Storage();
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
        Console.WriteLine("Out of services");
        Console.ReadKey();
        return;
        Console.WriteLine("Type game name / game id or use filters or get help with ai");
        Console.WriteLine();
        string inputText = Console.ReadLine();
        
    }
    private static void Wishlist()
    {
        Console.WriteLine("Out of services");
        Console.ReadKey();
    }
    private static void Storage()
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
}