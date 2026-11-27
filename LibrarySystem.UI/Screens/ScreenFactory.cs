using LibrarySystem.Core.Models;
using LibrarySystem.Data.Repositories;
using LibrarySystem.Logic.Navigation;
using LibrarySystem.UI.Screens.Panels;

namespace LibrarySystem.UI.Screens;

/// <summary>
/// Фабрика панелей разделов с DI-репозиториями SQLite.
/// </summary>
public sealed class ScreenFactory : IScreenFactory
{
    private readonly IBookRepository _bookRepo;
    private readonly IReaderRepository _readerRepo;
    private readonly ILoanRepository _loanRepo;

    public ScreenFactory(IBookRepository bookRepo, IReaderRepository readerRepo, ILoanRepository loanRepo)
    {
        _bookRepo = bookRepo;
        _readerRepo = readerRepo;
        _loanRepo = loanRepo;
    }

    public ScreenFactory() : this(new LibrarySystem.Data.Repositories.BookRepository(),
                                   new LibrarySystem.Data.Repositories.ReaderRepository(),
                                   new LibrarySystem.Data.Repositories.LoanRepository())
    {
    }

    public System.Windows.Forms.Panel Create(ScreenId screen)
    {
        return screen switch
        {
            ScreenId.Catalog => new CatalogPanel(_bookRepo),
            ScreenId.Readers => new ReadersPanel(_readerRepo, _loanRepo),
            ScreenId.Loan => new LoanPanel(_bookRepo, _readerRepo, _loanRepo),
            ScreenId.Return => new ReturnPanel(_loanRepo),
            ScreenId.Search => new SearchPanel(_bookRepo),
            ScreenId.Profile => new ProfilePanel(_readerRepo, _loanRepo),
            _ => new CatalogPanel(_bookRepo)
        };
    }
}