using LibrarySystem.Logic.Navigation;

namespace LibrarySystem.UI.Screens;

/// <summary>
/// Фабрика панелей разделов (шаблон «Фабричный метод»): создаёт панель нужного раздела.
/// </summary>
public interface IScreenFactory
{
    System.Windows.Forms.Panel Create(ScreenId screen, UserRole role);
}
