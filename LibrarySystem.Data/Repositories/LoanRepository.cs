using LibrarySystem.Core.Models;

namespace LibrarySystem.Data.Repositories;

/// <summary>
/// Интерфейс репозитория выдач.
/// </summary>
public interface ILoanRepository
{
    List<Loan> GetActiveLoans();
    List<Loan> GetActiveLoansByReader(int readerId);
    Loan? GetById(int id);
    void IssueBook(int bookId, int readerId, DateTime dueDate);
    void ReturnBook(int loanId, DateTime returnDate);
    List<Loan> GetAll();
}

/// <summary>
/// Репозиторий выдач. Данные хранятся в памяти (заглушка вместо БД).
/// </summary>
public sealed class LoanRepository : ILoanRepository
{
    private readonly List<Loan> _loans = new();
    private readonly IBookRepository _bookRepository;
    private readonly IReaderRepository _readerRepository;

    public LoanRepository()
        : this(new BookRepository(), new ReaderRepository())
    {
    }

    public LoanRepository(IBookRepository bookRepository, IReaderRepository readerRepository)
    {
        _bookRepository = bookRepository;
        _readerRepository = readerRepository;
        SeedData();
    }

    public List<Loan> GetAll() => _loans.ToList();

    public List<Loan> GetActiveLoans() =>
        _loans.Where(l => l.ReturnDate == null).ToList();

    public List<Loan> GetActiveLoansByReader(int readerId) =>
        _loans.Where(l => l.ReaderId == readerId && l.ReturnDate == null).ToList();

    public Loan? GetById(int id) => _loans.FirstOrDefault(l => l.Id == id);

    public void IssueBook(int bookId, int readerId, DateTime dueDate)
    {
        var book = _bookRepository.GetById(bookId);
        var reader = _readerRepository.GetById(readerId);

        if (book == null)
            throw new InvalidOperationException($"Книга с кодом {bookId} не найдена.");
        if (reader == null)
            throw new InvalidOperationException($"Читатель с кодом {readerId} не найден.");
        if (book.AvailableCount <= 0)
            throw new InvalidOperationException("Нет свободных экземпляров для выдачи.");

        book.AvailableCount--;

        var loan = new Loan
        {
            Id = _loans.Count == 0 ? 1 : _loans.Max(l => l.Id) + 1,
            BookId = bookId,
            ReaderId = readerId,
            IssueDate = DateTime.Now,
            DueDate = dueDate,
            BookTitle = book.Title,
            ReaderName = reader.FullName
        };

        _loans.Add(loan);
    }

    public void ReturnBook(int loanId, DateTime returnDate)
    {
        var loan = _loans.FirstOrDefault(l => l.Id == loanId);
        if (loan == null)
            throw new InvalidOperationException($"Выдача с кодом {loanId} не найдена.");

        if (loan.ReturnDate != null)
            throw new InvalidOperationException("Книга уже возвращена.");

        loan.ReturnDate = returnDate;

        var book = _bookRepository.GetById(loan.BookId);
        if (book != null)
            book.AvailableCount++;
    }

    private void SeedData()
    {
        // По умолчанию пусто, выдачи создаются в процессе работы
        // Можно добавить тестовые выдачи если нужно
    }
}