using LibrarySystem.Logic.Navigation;
using LibrarySystem.Logic.Views;

namespace LibrarySystem.Logic.Presenters;

/// <summary>
/// Presenter формы входа. Передаёт выбранную роль в главное окно.
/// </summary>
public sealed class LoginPresenter
{
    private readonly ILoginView _view;

    public LoginPresenter(ILoginView view)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _view.RoleSelected += OnRoleSelected;
        _view.ViewLoaded += OnViewLoaded;
    }

    private void OnViewLoaded(object? sender, EventArgs e)
    {
        _view.ShowError(string.Empty);
    }

    private void OnRoleSelected(object? sender, UserRole role)
    {
        // В реальной системе здесь проверка логина/пароля.
        // Сейчас достаточно выбора роли.
        RoleSelected?.Invoke(this, role);
    }

    public event EventHandler<UserRole>? RoleSelected;
}