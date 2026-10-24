using Microsoft.Extensions.DependencyInjection;
using PriceTracker.App.Scrapers;

namespace PriceTracker.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Run.MainMenu();
        }
    }
}