using LibrarySystem.Logic.Navigation;

namespace LibrarySystem.Logic.Views;

/// <summary>
/// Интерфейс представления формы входа.
/// </summary>
public interface ILoginView
{
    /// <summary>Введённый пароль (для входа библиотекаря).</summary>
    string Password { get; }

    event EventHandler<UserRole>? RoleSelected;
    event EventHandler? ViewLoaded;

    void ShowError(string message);
    void ClearPassword();
}