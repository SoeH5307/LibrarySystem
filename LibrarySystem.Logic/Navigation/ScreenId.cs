namespace LibrarySystem.Logic.Navigation;

/// <summary>
/// Разделы приложения — вкладки внутри главного окна.
/// </summary>
public enum ScreenId
{
    None,
    Login,           // Экран входа (выбор роли)
    Catalog,         // Каталог и фонд
    Readers,         // Читатели (только для библиотекаря)
    Loan,            // Выдача книги (только для библиотекаря)
    Return,          // Возврат книги (только для библиотекаря)
    Search,          // Поиск книг (все)
    Profile          // Профиль читателя (мои книги, штрафы)
}
