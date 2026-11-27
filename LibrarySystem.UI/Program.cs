using LibrarySystem.Data;
using LibrarySystem.Data.Repositories;
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

        // Инициализация SQLite БД
        var connFactory = new SqliteConnectionFactory("library.db");
        DbInitializer.Initialize(connFactory);

        // DI: создаём репозитории один раз и переиспользуем
        IBookRepository bookRepo = new SqliteBookRepository(connFactory);
        IReaderRepository readerRepo = new SqliteReaderRepository(connFactory);
        ILoanRepository loanRepo = new SqliteLoanRepository(connFactory, bookRepo, readerRepo);

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
            return;

        // После успешного входа запускаем главное окно с SQLite-репозиториями
        var mainForm = new MainForm();
        var factory = new ScreenFactory(bookRepo, readerRepo, loanRepo);
        var navigator = new FormNavigator(mainForm, factory);
        _ = new MainPresenter(mainForm, navigator, selectedRole);

        Application.Run(mainForm);
    }
}