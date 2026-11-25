using LibrarySystem.Logic.Navigation;

namespace LibrarySystem.Logic.Views;

/// <summary>
/// Интерфейс представления формы входа.
/// </summary>
public interface ILoginView
{
    event EventHandler<UserRole>? RoleSelected;
    event EventHandler? ViewLoaded;
    void ShowError(string message);
}