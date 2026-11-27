using LibrarySystem.Core.Models;
using Microsoft.Data.Sqlite;

namespace LibrarySystem.Data.Repositories;

/// <summary>
/// SQLite-реализация репозитория выдач.
/// </summary>
public sealed class SqliteLoanRepository : ILoanRepository
{
    private readonly SqliteConnectionFactory _factory;
    private readonly IBookRepository _bookRepository;
    private readonly IReaderRepository _readerRepository;

    public SqliteLoanRepository(SqliteConnectionFactory factory, IBookRepository bookRepository, IReaderRepository readerRepository)
    {
        _factory = factory;
        _bookRepository = bookRepository;
        _readerRepository = readerRepository;
    }

    public List<Loan> GetAll()
    {
        var loans = new List<Loan>();
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT l.Id, l.BookId, l.ReaderId, l.IssueDate, l.DueDate, l.ReturnDate,
                   b.Title, r.LastName || ' ' || r.FirstName || ' ' || COALESCE(r.MiddleName, '')
            FROM Loans l
            JOIN Books b ON l.BookId = b.Id
            JOIN Readers r ON l.ReaderId = r.Id
            ORDER BY l.IssueDate DESC";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            loans.Add(new Loan
            {
                Id = reader.GetInt32(0),
                BookId = reader.GetInt32(1),
                ReaderId = reader.GetInt32(2),
                IssueDate = DateTime.Parse(reader.GetString(3)),
                DueDate = DateTime.Parse(reader.GetString(4)),
                ReturnDate = reader.IsDBNull(5) ? null : DateTime.Parse(reader.GetString(5)),
                BookTitle = reader.GetString(6),
                ReaderName = reader.GetString(7).Trim()
            });
        }
        return loans;
    }

    public List<Loan> GetActiveLoans() => GetAll().Where(l => l.ReturnDate == null).ToList();

    public List<Loan> GetActiveLoansByReader(int readerId)
    {
        return GetAll().Where(l => l.ReaderId == readerId && l.ReturnDate == null).ToList();
    }

    public Loan? GetById(int id)
    {
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT l.Id, l.BookId, l.ReaderId, l.IssueDate, l.DueDate, l.ReturnDate,
                   b.Title, r.LastName || ' ' || r.FirstName || ' ' || COALESCE(r.MiddleName, '')
            FROM Loans l
            JOIN Books b ON l.BookId = b.Id
            JOIN Readers r ON l.ReaderId = r.Id
            WHERE l.Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Loan
            {
                Id = reader.GetInt32(0),
                BookId = reader.GetInt32(1),
                ReaderId = reader.GetInt32(2),
                IssueDate = DateTime.Parse(reader.GetString(3)),
                DueDate = DateTime.Parse(reader.GetString(4)),
                ReturnDate = reader.IsDBNull(5) ? null : DateTime.Parse(reader.GetString(5)),
                BookTitle = reader.GetString(6),
                ReaderName = reader.GetString(7).Trim()
            };
        }
        return null;
    }

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

        using var conn = _factory.CreateConnection();
        using var transaction = conn.BeginTransaction();

        try
        {
            // Создаём запись о выдаче
            using var cmd = conn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = @"
                INSERT INTO Loans (BookId, ReaderId, IssueDate, DueDate, ReturnDate)
                VALUES ($bookId, $readerId, $issueDate, $dueDate, NULL);
                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$bookId", bookId);
            cmd.Parameters.AddWithValue("$readerId", readerId);
            cmd.Parameters.AddWithValue("$issueDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("$dueDate", dueDate.ToString("yyyy-MM-dd HH:mm:ss"));
            var loanId = Convert.ToInt32(cmd.ExecuteScalar());

            // Уменьшаем AvailableCount
            using var updateCmd = conn.CreateCommand();
            updateCmd.Transaction = transaction;
            updateCmd.CommandText = "UPDATE Books SET AvailableCount = AvailableCount - 1 WHERE Id = $id";
            updateCmd.Parameters.AddWithValue("$id", bookId);
            updateCmd.ExecuteNonQuery();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void ReturnBook(int loanId, DateTime returnDate)
    {
        var loan = GetById(loanId);
        if (loan == null)
            throw new InvalidOperationException($"Выдача с кодом {loanId} не найдена.");
        if (loan.ReturnDate != null)
            throw new InvalidOperationException("Книга уже возвращена.");

        using var conn = _factory.CreateConnection();
        using var transaction = conn.BeginTransaction();

        try
        {
            // Отмечаем возврат
            using var cmd = conn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = "UPDATE Loans SET ReturnDate = $returnDate WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", loanId);
            cmd.Parameters.AddWithValue("$returnDate", returnDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();

            // Увеличиваем AvailableCount
            using var updateCmd = conn.CreateCommand();
            updateCmd.Transaction = transaction;
            updateCmd.CommandText = "UPDATE Books SET AvailableCount = AvailableCount + 1 WHERE Id = $id";
            updateCmd.Parameters.AddWithValue("$id", loan.BookId);
            updateCmd.ExecuteNonQuery();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}