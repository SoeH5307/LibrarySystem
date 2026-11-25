using LibrarySystem.Logic.Navigation;
using LibrarySystem.Logic.Presenters;
using LibrarySystem.UI.Screens;

namespace LibrarySystem.UI;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Сначала показываем форму входа
        var loginForm = new LoginForm();
        var loginPresenter = new LoginPresenter(loginForm);

        UserRole selectedRole = UserRole.None;
        loginPresenter.RoleSelected += (_, role) =>
        {
            selectedRole = role;
            loginForm.Close();
        };

        Application.Run(loginForm);

        if (selectedRole == UserRole.None)
            return; // Пользователь закрыл форму входа

        // После успешного входа запускаем главное окно
        var mainForm = new MainForm();
        var navigator = new FormNavigator(mainForm, new ScreenFactory());
        _ = new MainPresenter(mainForm, navigator, selectedRole);

        Application.Run(mainForm);
    }
}
