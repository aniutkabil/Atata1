namespace Atata.PerformanceShowcase.Tests.Infrastructure;

internal sealed class BrowserSettings
{
    public bool Headless { get; init; }

    public bool MaximizeWindow { get; init; } = true;

    public int WindowWidth { get; init; } = 1440;

    public int WindowHeight { get; init; } = 1000;

    public IReadOnlyList<string> AdditionalArguments { get; init; } = [];

    public IReadOnlyList<string> GetArguments()
    {
        var arguments = new List<string>();

        if (MaximizeWindow)
        {
            arguments.Add("start-maximized");
        }
        else
        {
            arguments.Add($"window-size={WindowWidth},{WindowHeight}");
        }

        if (Headless)
        {
            arguments.Add("headless=new");
        }

        arguments.AddRange(AdditionalArguments);
        return arguments;
    }
}
