using LibrarySystem.Logic.Navigation;
using LibrarySystem.Logic.Views;

namespace LibrarySystem.Logic.Presenters;

/// <summary>
/// Presenter формы входа. Проверяет пароль библиотекаря и передаёт роль в главное окно.
/// </summary>
public sealed class LoginPresenter
{
    /// <summary>Пароль библиотекаря.</summary>
    public const string LibrarianPassword = "1111";

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
        _view.ClearPassword();
    }

    private void OnRoleSelected(object? sender, UserRole role)
    {
        if (role == UserRole.Librarian)
        {
            string entered = _view.Password ?? string.Empty;

            if (string.IsNullOrWhiteSpace(entered))
            {
                _view.ShowError("Введите пароль библиотекаря.");
                return;
            }

            if (entered != LibrarianPassword)
            {
                _view.ShowError("Неверный пароль. Попробуйте ещё раз.");
                _view.ClearPassword();
                return;
            }
        }

        _view.ShowError(string.Empty);
        _view.ClearPassword();
        RoleSelected?.Invoke(this, role);
    }

    public event EventHandler<UserRole>? RoleSelected;
}