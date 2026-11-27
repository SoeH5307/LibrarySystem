using LibrarySystem.Logic.Navigation;
using LibrarySystem.Logic.Views;

namespace LibrarySystem.Logic.Presenters;

/// <summary>
/// Presenter формы входа. При выборе роли библиотекаря запрашивает и проверяет пароль.
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
    }

    private void OnRoleSelected(object? sender, UserRole role)
    {
        if (role == UserRole.Librarian)
        {
            string? entered = _view.RequestPassword();

            if (entered is null)
            {
                // Пользователь отменил ввод пароля
                _view.ShowError(string.Empty);
                return;
            }

            if (string.IsNullOrWhiteSpace(entered))
            {
                _view.ShowError("Пароль не введён. Вход отменён.");
                return;
            }

            if (entered != LibrarianPassword)
            {
                _view.ShowError("Неверный пароль библиотекаря.");
                return;
            }
        }

        _view.ShowError(string.Empty);
        RoleSelected?.Invoke(this, role);
    }

    public event EventHandler<UserRole>? RoleSelected;
}