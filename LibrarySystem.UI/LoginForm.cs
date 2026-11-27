using LibrarySystem.Logic.Navigation;
using LibrarySystem.Logic.Views;

namespace LibrarySystem.UI;

/// <summary>
/// Форма входа в систему: выбор роли. Пароль запрашивается отдельным диалогом
/// только при выборе роли библиотекаря.
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

    /// <summary>
    /// Показать диалог ввода пароля.
    /// Возвращает введённый пароль или null, если пользователь отказался.
    /// </summary>
    public string? RequestPassword()
    {
        using var dlg = new Form
        {
            Text = "Вход библиотекаря",
            Size = new Size(370, 190),
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false,
            ShowInTaskbar = false,
        };

        var lblPrompt = new Label
        {
            Text = "Введите пароль библиотекаря:",
            Location = new Point(20, 20),
            AutoSize = true,
        };

        var txtPassword = new TextBox
        {
            Location = new Point(20, 48),
            Size = new Size(310, 23),
            UseSystemPasswordChar = true,
        };

        var btnOk = new Button
        {
            Text = "Войти",
            Location = new Point(140, 90),
            Size = new Size(95, 32),
            DialogResult = DialogResult.OK,
        };

        var btnCancel = new Button
        {
            Text = "Отмена",
            Location = new Point(245, 90),
            Size = new Size(85, 32),
            DialogResult = DialogResult.Cancel,
        };

        dlg.Controls.AddRange(new Control[] { lblPrompt, txtPassword, btnOk, btnCancel });
        dlg.AcceptButton = btnOk;
        dlg.CancelButton = btnCancel;

        var result = dlg.ShowDialog(this);
        return result == DialogResult.OK ? txtPassword.Text : null;
    }
}