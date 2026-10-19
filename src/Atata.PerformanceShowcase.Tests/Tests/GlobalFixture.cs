using Atata.NUnit;
using Atata.PerformanceShowcase.Tests.Infrastructure;
using Atata.WebDriverSetup;
using NUnit.Framework;

namespace Atata.PerformanceShowcase.Tests.Tests;

[SetUpFixture]
public sealed class GlobalFixture : AtataGlobalFixture
{
    protected override void ConfigureAtataContextBaseConfiguration(
        AtataContextBuilder builder)
    {
        builder
            .UseCulture("en-US")
            .UseNUnitAggregateAssertionStrategy()
            .UseWaitingTimeout(TestSettings.WaitTimeout)
            .UseVerificationTimeout(TestSettings.WaitTimeout)
            .Sessions.AddWebDriver(session => session
                .UseName("primary")
                .UseStartScopes(AtataContextScopes.Test)
                .UseChrome(chrome => chrome.WithArguments(
                    [.. TestSettings.Browser.GetArguments()]))
                .UseBaseUrl(TestSettings.BaseUrl));
    }

    protected override void ConfigureGlobalAtataContext(
        AtataContextBuilder builder) =>
        builder.SetUpWebDriversForUse();
}
