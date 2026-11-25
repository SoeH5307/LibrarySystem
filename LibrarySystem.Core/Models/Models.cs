namespace LibrarySystem.Core.Models;

/// <summary>
/// Книга (издание).
/// </summary>
public sealed class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Pages { get; set; }
    public string Subject { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TotalCopies { get; set; } = 1;
    public int AvailableCount { get; set; } = 1;
}

/// <summary>
/// Читатель.
/// </summary>
public sealed class Reader
{
    public int Id { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
}

/// <summary>
/// Выдача книги.
/// </summary>
public sealed class Loan
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int ReaderId { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    // Навигационные свойства для отображения
    public string BookTitle { get; set; } = string.Empty;
    public string ReaderName { get; set; } = string.Empty;
}