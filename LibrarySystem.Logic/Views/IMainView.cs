using LibrarySystem.Logic.Navigation;

namespace LibrarySystem.Logic.Views;

/// <summary>
/// Интерфейс представления (View) главного окна.
/// Presenter работает только с ним и ничего не знает о WinForms.
/// </summary>
public interface IMainView
{
    /// <summary>Имя текущего пользователя (строка состояния).</summary>
    string UserName { set; }

    /// <summary>Текст сообщения в строке состояния.</summary>
    string StatusText { set; }

    /// <summary>Окно загружено и готово к работе.</summary>
    event EventHandler? ViewLoaded;

    /// <summary>Пользователь выбрал раздел (пункт меню или кнопка).</summary>
    event EventHandler<ScreenId>? ScreenRequested;

    /// <summary>Показать пользователю сообщение.</summary>
    void ShowMessage(string message);

    /// <summary>Обновить видимость элементов меню в зависимости от роли.</summary>
    void SetUserRole(UserRole role);
}
