using LibrarySystem.Logic.Navigation;
using LibrarySystem.Logic.Views;

namespace LibrarySystem.UI;

/// <summary>
/// Форма входа в систему с выбором роли.
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
        
        Load += (_, _) => ViewLoaded?.Invoke(this, EventArgs.Empty);
    }

    public void ShowError(string message)
    {
        lblError.Text = message;
        lblError.Visible = !string.IsNullOrEmpty(message);
    }
}