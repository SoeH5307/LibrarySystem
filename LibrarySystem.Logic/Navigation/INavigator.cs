namespace LibrarySystem.Logic.Navigation;

/// <summary>
/// Переключение вкладок внутри главного окна.
/// Presenter знает только этот интерфейс, не конкретные контролы WinForms.
/// </summary>
public interface INavigator
{
    void NavigateTo(ScreenId screen);
    void SetUserRole(UserRole role);
}
