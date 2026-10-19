
using Atata;
using Atata.NUnit;
using Atata.PerformanceShowcase.Tests.Domain.Models;
using Atata.PerformanceShowcase.Tests.Infrastructure;
using Atata.PerformanceShowcase.Tests.Pages;
using Atata.PerformanceShowcase.Tests.TestData.DataProviders;
using NUnit.Framework;

namespace Atata.PerformanceShowcase.Tests.Tests;

[TestFixture]
[NonParallelizable]
public sealed class ProductsTestB : AtataTestSuite
{
    private const string ScenarioName = "removeArmchair";

    protected override void ConfigureSuiteAtataContext(
        AtataContextBuilder builder)
    {
        builder.Sessions.ConfigureWebDriver(
            "primary",
            session => session
                .UseStartScopes(AtataContextScopes.TestSuite)
                .UseAsShared());
    }

    protected override void ConfigureTestAtataContext(
        AtataContextBuilder builder)
    {
        builder.Sessions.Borrow<WebDriverSession>("primary");
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        LogIn();
    }

    private static void LogIn()
    {
        Go.To<SignInPage>()
            .Email.Set(TestSettings.UserName)
            .Password.Set(TestSettings.Password)
            .SignIn.Click();
    }

    [Test]
    [Description("Test B: Atata AggregateAssert, typed Currency controls, trigger waits and one-time login.")]
    public void RemoveArmchair_VerifySummaryWithAggregateAssert()
    {
        string productName =
            ProductsTestDataProvider.GetProductName(ScenarioName);

        CartSummary expected =
            ProductsTestDataProvider.GetExpectedSummary(ScenarioName);

        ProductsPage page = Go.To<ProductsPage>();
        page.RemoveProduct(productName);

        IReadOnlyList<Product> remainingProducts = page.ReadProducts();
        CartSummary actual = CartSummary.FromProducts(remainingProducts);

        AtataContext.Current!.AggregateAssert(() =>
        {
            page.Products.Rows[x => x.Name == productName]
                .Should.Not.BePresent();

            Assert.That(actual.TotalPrice, Is.EqualTo(expected.TotalPrice),
                "Total price of remaining products.");

            Assert.That(actual.ProductCount, Is.EqualTo(expected.ProductCount),
                "Number of remaining product rows.");

            Assert.That(actual.TotalAmount, Is.EqualTo(expected.TotalAmount),
                "Total amount of remaining products.");
        });
    }
}
