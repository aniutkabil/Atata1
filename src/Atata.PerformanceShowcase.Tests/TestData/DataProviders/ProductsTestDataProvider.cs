using System.Text.Json;
using Atata.PerformanceShowcase.Tests.Domain.Models;

namespace Atata.PerformanceShowcase.Tests.TestData.DataProviders;

/// <summary>
/// Loads shared product scenarios from the checked-in test-data file.
/// Keeps scenario values out of test methods without duplicating domain models.
/// </summary>
public static class ProductsTestDataProvider
{
    private const string DataFileName = "Products.json";
    private const string ProductNameProperty = "productName";
    private const string ExpectedSummaryProperty = "expectedSummary";
    private const string TotalPriceProperty = "totalPrice";
    private const string ProductCountProperty = "productCount";
    private const string TotalAmountProperty = "totalAmount";

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly Lazy<JsonDocument> TestData = new(Load);

    public static string GetProductName(string scenarioName)
    {
        JsonElement value = GetScenario(scenarioName).GetProperty(ProductNameProperty);
        string? productName = value.ValueKind == JsonValueKind.String ? value.GetString() : null;

        return !string.IsNullOrWhiteSpace(productName)
            ? productName
            : throw new InvalidDataException(
                $"Property '{ProductNameProperty}' must be a non-empty string in scenario '{scenarioName}'.");
    }

    public static CartSummary GetExpectedSummary(string scenarioName)
    {
        JsonElement value = GetScenario(scenarioName).GetProperty(ExpectedSummaryProperty);

        if (value.ValueKind != JsonValueKind.Object ||
            !value.TryGetProperty(TotalPriceProperty, out JsonElement totalPrice) ||
            totalPrice.ValueKind != JsonValueKind.Number || !totalPrice.TryGetDecimal(out _) ||
            !value.TryGetProperty(ProductCountProperty, out JsonElement productCount) ||
            productCount.ValueKind != JsonValueKind.Number || !productCount.TryGetInt32(out _) ||
            !value.TryGetProperty(TotalAmountProperty, out JsonElement totalAmount) ||
            totalAmount.ValueKind != JsonValueKind.Number || !totalAmount.TryGetDecimal(out _))
        {
            throw new InvalidDataException(
                $"Property '{ExpectedSummaryProperty}' in scenario '{scenarioName}' must define numeric " +
                $"'{TotalPriceProperty}', '{ProductCountProperty}', and " +
                $"'{TotalAmountProperty}' values.");
        }

        return value.Deserialize<CartSummary>(SerializerOptions)
            ?? throw new InvalidDataException(
                $"Property '{ExpectedSummaryProperty}' is missing or invalid in scenario '{scenarioName}'.");
    }

    private static JsonElement GetScenario(string scenarioName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(scenarioName);

        JsonElement scenarios = TestData.Value.RootElement;

        if (scenarios.ValueKind != JsonValueKind.Object ||
            !scenarios.TryGetProperty(scenarioName, out JsonElement scenario) ||
            scenario.ValueKind != JsonValueKind.Object)
        {
            throw new KeyNotFoundException(
                $"Product test scenario '{scenarioName}' was not found in '{DataFileName}'.");
        }

        if (!scenario.TryGetProperty(ProductNameProperty, out _) ||
            !scenario.TryGetProperty(ExpectedSummaryProperty, out _))
        {
            throw new InvalidDataException(
                $"Scenario '{scenarioName}' in '{DataFileName}' must define '{ProductNameProperty}' and '{ExpectedSummaryProperty}'.");
        }

        return scenario;
    }

    private static JsonDocument Load()
    {
        string filePath = Path.Combine(AppContext.BaseDirectory, "TestData", DataFileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                $"Product test data was not copied to the test output directory. Expected file: '{filePath}'. " +
                $"Ensure '{DataFileName}' has CopyToOutputDirectory enabled in the test project.",
                filePath);
        }

        try
        {
            return JsonDocument.Parse(File.ReadAllText(filePath));
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException($"Product test data file '{filePath}' contains invalid JSON.", exception);
        }
    }
}
