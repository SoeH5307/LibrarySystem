using LibrarySystem.Core.Models;
using LibrarySystem.Data.Repositories;
using Microsoft.Data.Sqlite;

namespace LibrarySystem.Data.Repositories;

/// <summary>
/// SQLite-реализация репозитория книг.
/// </summary>
public sealed class SqliteBookRepository : IBookRepository
{
    private readonly SqliteConnectionFactory _factory;

    public SqliteBookRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public List<Book> GetAll()
    {
        var books = new List<Book>();
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT Id, Title, Author, Publisher, Year, Pages, Subject, Price, TotalCopies, AvailableCount
            FROM Books
            ORDER BY Title";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            books.Add(new Book
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Author = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Publisher = reader.IsDBNull(3) ? "" : reader.GetString(3),
                Year = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                Pages = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                Subject = reader.IsDBNull(6) ? "" : reader.GetString(6),
                Price = reader.IsDBNull(7) ? 0m : (decimal)reader.GetDouble(7),
                TotalCopies = reader.GetInt32(8),
                AvailableCount = reader.GetInt32(9)
            });
        }
        return books;
    }

    public Book? GetById(int id)
    {
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT Id, Title, Author, Publisher, Year, Pages, Subject, Price, TotalCopies, AvailableCount
            FROM Books
            WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Book
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Author = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Publisher = reader.IsDBNull(3) ? "" : reader.GetString(3),
                Year = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                Pages = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                Subject = reader.IsDBNull(6) ? "" : reader.GetString(6),
                Price = reader.IsDBNull(7) ? 0m : (decimal)reader.GetDouble(7),
                TotalCopies = reader.GetInt32(8),
                AvailableCount = reader.GetInt32(9)
            };
        }
        return null;
    }

    public List<Book> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return GetAll();

        var books = new List<Book>();
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT Id, Title, Author, Publisher, Year, Pages, Subject, Price, TotalCopies, AvailableCount
            FROM Books
            WHERE Title LIKE $q OR Author LIKE $q OR Publisher LIKE $q
            ORDER BY Title";
        cmd.Parameters.AddWithValue("$q", $"%{query}%");
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            books.Add(new Book
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Author = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Publisher = reader.IsDBNull(3) ? "" : reader.GetString(3),
                Year = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                Pages = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                Subject = reader.IsDBNull(6) ? "" : reader.GetString(6),
                Price = reader.IsDBNull(7) ? 0m : (decimal)reader.GetDouble(7),
                TotalCopies = reader.GetInt32(8),
                AvailableCount = reader.GetInt32(9)
            });
        }
        return books;
    }

    public void Add(Book book)
    {
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Books (Title, Author, Publisher, Year, Pages, Subject, Price, TotalCopies, AvailableCount)
            VALUES ($title, $author, $publisher, $year, $pages, $subject, $price, $total, $avail);
            SELECT last_insert_rowid();";
        cmd.Parameters.AddWithValue("$title", book.Title);
        cmd.Parameters.AddWithValue("$author", book.Author);
        cmd.Parameters.AddWithValue("$publisher", book.Publisher);
        cmd.Parameters.AddWithValue("$year", book.Year);
        cmd.Parameters.AddWithValue("$pages", book.Pages);
        cmd.Parameters.AddWithValue("$subject", book.Subject);
        cmd.Parameters.AddWithValue("$price", book.Price);
        cmd.Parameters.AddWithValue("$total", book.TotalCopies);
        cmd.Parameters.AddWithValue("$avail", book.AvailableCount);
        book.Id = Convert.ToInt32(cmd.ExecuteScalar());
    }

    public void Update(Book book)
    {
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE Books
            SET Title = $title, Author = $author, Publisher = $publisher,
                Year = $year, Pages = $pages, Subject = $subject, Price = $price,
                TotalCopies = $total, AvailableCount = $avail
            WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", book.Id);
        cmd.Parameters.AddWithValue("$title", book.Title);
        cmd.Parameters.AddWithValue("$author", book.Author);
        cmd.Parameters.AddWithValue("$publisher", book.Publisher);
        cmd.Parameters.AddWithValue("$year", book.Year);
        cmd.Parameters.AddWithValue("$pages", book.Pages);
        cmd.Parameters.AddWithValue("$subject", book.Subject);
        cmd.Parameters.AddWithValue("$price", book.Price);
        cmd.Parameters.AddWithValue("$total", book.TotalCopies);
        cmd.Parameters.AddWithValue("$avail", book.AvailableCount);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Books WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    public void RegisterCopy(int bookId)
    {
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE Books
            SET TotalCopies = TotalCopies + 1, AvailableCount = AvailableCount + 1
            WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", bookId);
        cmd.ExecuteNonQuery();
    }

    public void WriteOffCopy(int bookId)
    {
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE Books
            SET TotalCopies = TotalCopies - 1, AvailableCount = AvailableCount - 1
            WHERE Id = $id AND AvailableCount > 0";
        cmd.Parameters.AddWithValue("$id", bookId);
        var affected = cmd.ExecuteNonQuery();
        if (affected == 0)
            throw new InvalidOperationException("Нельзя списать экземпляр: свободных экземпляров нет.");
    }
}