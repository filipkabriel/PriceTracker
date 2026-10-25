namespace PriceTracker.App.Models;

public record PriceRecord(
    int AppId,
    string GameName,
    decimal Price,
    DateTime Timestamp
);