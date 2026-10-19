namespace Atata.PerformanceShowcase.Tests.Infrastructure;

internal static class TestSettings
{
    public static string BaseUrl => ReadString("ATATA_BASE_URL", "https://demo.atata.io/");

    public static string UserName => ReadString("ATATA_USERNAME", "admin@mail.com");

    public static string Password => ReadString("ATATA_PASSWORD", "abc123");

    public static TimeSpan WaitTimeout => TimeSpan.FromSeconds(ReadInt("ATATA_WAIT_SECONDS", 5, 1, 60));

    public static BrowserSettings Browser { get; } = new()
    {
        Headless = ReadBoolean("HEADLESS", defaultValue: false),
        MaximizeWindow = ReadBoolean("MAXIMIZE_WINDOW", defaultValue: true)
    };

    private static string ReadString(string name, string defaultValue) =>
        Environment.GetEnvironmentVariable(name) is { Length: > 0 } value ? value : defaultValue;

    private static bool ReadBoolean(string name, bool defaultValue) =>
        bool.TryParse(Environment.GetEnvironmentVariable(name), out bool value) ? value : defaultValue;

    private static int ReadInt(string name, int defaultValue, int minimum, int maximum) =>
        int.TryParse(Environment.GetEnvironmentVariable(name), out int value)
            ? Math.Clamp(value, minimum, maximum)
            : defaultValue;
}
