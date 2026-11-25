using LibrarySystem.Logic.Navigation;

namespace LibrarySystem.UI.Screens;

/// <summary>
/// Реализация навигации для панельного интерфейса.
/// Переключает Panel внутри MainForm вместо открытия отдельных окон.
/// </summary>
public sealed class FormNavigator : INavigator
{
    private readonly MainForm _mainForm;
    private readonly IScreenFactory _factory;

    public FormNavigator(MainForm mainForm, IScreenFactory factory)
    {
        _mainForm = mainForm ?? throw new ArgumentNullException(nameof(mainForm));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public void NavigateTo(ScreenId screen)
    {
        _mainForm.NavigateTo(screen);
    }

    public void SetUserRole(UserRole role)
    {
        _mainForm.SetUserRole(role);
    }
}
