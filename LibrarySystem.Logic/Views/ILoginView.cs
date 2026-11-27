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

    /// <summary>
    /// Запросить пароль у пользователя.
    /// Возвращает введённый пароль или null, если пользователь отменил ввод.
    /// </summary>
    string? RequestPassword();
}