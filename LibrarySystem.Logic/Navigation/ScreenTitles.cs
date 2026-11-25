namespace LibrarySystem.Logic.Navigation;

/// <summary>
/// Названия разделов для заголовков окон и строки состояния.
/// </summary>
public static class ScreenTitles
{
    public static string Get(ScreenId screen) => screen switch
    {
        ScreenId.Login => "Вход в систему",
        ScreenId.Catalog => "Каталог и фонд",
        ScreenId.Readers => "Читатели",
        ScreenId.Loan => "Выдача книги",
        ScreenId.Return => "Возврат книги",
        ScreenId.Search => "Поиск книг",
        ScreenId.Profile => "Мой профиль",
        _ => screen.ToString()
    };
}
