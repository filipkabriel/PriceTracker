using System.ComponentModel.Design;

namespace PriceTracker.App;

public static class Run
{
    private static readonly string[] selectionMainMenu = new[] { "Search game", "Wishlist", "Storage", "Settings" };
    public static void MainMenu()
    {
        Console.WriteLine("CLI Steam PriceTracker");
        Thread.Sleep(500);
        Console.WriteLine();
        Console.WriteLine("Pick your action: ");
        
        switch (Select(selectionMainMenu))
        {
            
        }
    }
    private static int Select(string[] selection)
    {
        int id = 0;
        return id;
    }
}