using LibrarySystem.Logic.Navigation;
using LibrarySystem.Logic.Views;

namespace LibrarySystem.Logic.Presenters;

/// <summary>
/// Presenter главного окна (шаблон MVP): обрабатывает действия пользователя
/// и управляет отображением, не зная о конкретных элементах формы.
/// </summary>
public sealed class MainPresenter
{
    private readonly IMainView _view;
    private readonly INavigator _navigator;
    private readonly UserRole _userRole;

    public MainPresenter(IMainView view, INavigator navigator, UserRole userRole)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
        _userRole = userRole;

        _view.ViewLoaded += OnViewLoaded;
    }

    private void OnViewLoaded(object? sender, EventArgs e)
    {
        string roleText = _userRole == UserRole.Librarian ? "Библиотекарь" : "Читатель";
        _view.UserName = roleText;
        _view.StatusText = "Готово к работе";
        _view.SetUserRole(_userRole);
    }
}