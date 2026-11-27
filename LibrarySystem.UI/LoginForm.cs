using LibrarySystem.Logic.Navigation;
using LibrarySystem.Logic.Views;

namespace LibrarySystem.UI;

/// <summary>
/// Форма входа в систему: выбор роли и пароль для библиотекаря.
/// </summary>
public sealed partial class LoginForm : Form, ILoginView
{
    public event EventHandler<UserRole>? RoleSelected;
    public event EventHandler? ViewLoaded;

    public LoginForm()
    {
        InitializeComponent();

        btnLibrarian.Click += (_, _) => RoleSelected?.Invoke(this, UserRole.Librarian);
        btnReader.Click += (_, _) => RoleSelected?.Invoke(this, UserRole.Reader);

        // Сброс ошибки при вводе пароля
        txtPassword.TextChanged += (_, _) => ShowError(string.Empty);

        Load += (_, _) => ViewLoaded?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Введённый пароль.</summary>
    public string Password => txtPassword.Text;

    public void ShowError(string message)
    {
        lblError.Text = message;
        lblError.Visible = !string.IsNullOrEmpty(message);
    }

    public void ClearPassword()
    {
        txtPassword.Clear();
    }
}