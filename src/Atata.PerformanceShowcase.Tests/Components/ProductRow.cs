using Atata;
using Atata.PerformanceShowcase.Tests.Pages;

namespace Atata.PerformanceShowcase.Tests.Components;

/// <summary>Typed representation of one product table row.</summary>
public sealed class ProductRow : TableRow<ProductsPage>
{
    public Text<ProductsPage> Name { get; private set; } = null!;

    [Culture("en-US")]
    public Currency<ProductsPage> Price { get; private set; } = null!;

    public Number<ProductsPage> Amount { get; private set; } = null!;

    [FindByXPath(".//button[normalize-space(.)='Delete Using JS Confirm']")]
    [CloseConfirmBox(Priority = TriggerPriority.High)]
    [WaitForPageObjectDomMutationEnd(TriggerEvents.AfterClick, Priority = TriggerPriority.Low)]
    public Button<ProductsPage> DeleteUsingJsConfirm { get; private set; } = null!;
}
