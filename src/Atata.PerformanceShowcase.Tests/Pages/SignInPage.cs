using Atata;

namespace Atata.PerformanceShowcase.Tests.Pages;

[Url("signin")]
[WaitForDocumentReadyState]
public sealed class SignInPage : Page<SignInPage>
{
    [FindById("email")]
    public TextInput<SignInPage> Email { get; private set; }

    [FindById("password")]
    public PasswordInput<SignInPage> Password { get; private set; }

    [FindByContent("Sign In")]
    public Button<SignInPage> SignIn { get; private set; }
}
