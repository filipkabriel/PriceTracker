#  CLI Steam PriceTracker

**CLI Steam PriceTracker** is an interactive C# console application that lets you search for Steam games, organize them into custom wishlists, track their prices automatically in the background, and visualize price history using built-in ASCII charts right in your terminal!


---

##  Features

* ** Live Steam Store Search:** Search for games directly by name or Steam App ID using the official Steam Store API (`storesearch` & `appdetails`), displaying live prices in EUR and current discount percentages.
* ** Custom Wishlists:** Create multiple wishlists and add games from your search results to track them.
* ** Background Price Tracking:** Automatically fetches and logs the latest prices for all games across your wishlists every 2 hours in the background using `PeriodicTimer`, saving records safely to `price_history.json`.
* ** Terminal ASCII Price Charts:** View historical price trends for any tracked game with a color-coded console chart showing up to the last 15 price records with timestamps and dates.
* ** Interactive Keyboard Menu:** Navigate smoothly without typing numbers—use arrows/WASD to highlight options and `ESC` to go back or cancel input at any time.

---

##  Screenshots

> *(TODO: Nahraj sem screenshot hlavního menu, vyhledávání a grafu!)*
> `![Main Menu](screenshots/menu.png)`
> `![Price Chart](screenshots/chart.png)`

---

##  Controls

| Key | Action |
| :--- | :--- |
| **`↑` / `↓`** or **`W` / `S`** | Move up and down in the menu |
| **`Enter`** or **`Spacebar`** | Confirm selection |
| **`Escape (ESC)`** | Go back to the previous menu / Cancel text input |

---

##  How to Run

### Option 1: Download Pre-built Release (Easiest)
1. Go to the **[Releases](../../releases)** page on the right side of this repository.
2. Download the latest executable for your operating system (`PriceTracker-win-x64.exe`, Linux, or macOS).
3. Run the executable!

### Option 2: Build from Source (.NET SDK Required)
Make sure you have the **.NET SDK** (8.0 / 9.0) installed.

```bash
# 1. Clone the repository
git clone [https://github.com/YOUR-USERNAME/YOUR-REPO-NAME.git](https://github.com/YOUR-USERNAME/YOUR-REPO-NAME.git)
cd YOUR-REPO-NAME

# 2. Restore dependencies and run
dotnet run
