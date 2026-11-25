using LibrarySystem.Logic.Navigation;
using LibrarySystem.UI.Screens.Panels;

namespace LibrarySystem.UI.Screens;

/// <summary>
/// Фабрика панелей разделов (шаблон «Фабричный метод»).
/// Создаёт UserControl для нужного раздела.
/// </summary>
public sealed class ScreenFactory : IScreenFactory
{
    public System.Windows.Forms.Panel Create(ScreenId screen)
    {
        return screen switch
        {
            ScreenId.Catalog => new CatalogPanel(),
            ScreenId.Readers => new ReadersPanel(),
            ScreenId.Loan => new LoanPanel(),
            ScreenId.Return => new ReturnPanel(),
            ScreenId.Search => new SearchPanel(),
            ScreenId.Profile => new ProfilePanel(),
            _ => new CatalogPanel()
        };
    }
}
