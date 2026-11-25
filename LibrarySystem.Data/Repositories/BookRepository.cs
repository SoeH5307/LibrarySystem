using LibrarySystem.Core.Models;

namespace LibrarySystem.Data.Repositories;

/// <summary>
/// Интерфейс репозитория книг.
/// </summary>
public interface IBookRepository
{
    List<Book> GetAll();
    Book? GetById(int id);
    List<Book> Search(string query);
    void Add(Book book);
    void Update(Book book);
    void Delete(int id);
    void RegisterCopy(int bookId);
    void WriteOffCopy(int bookId);
}

/// <summary>
/// Репозиторий книг. Данные хранятся в памяти (заглушка вместо БД).
/// </summary>
public sealed class BookRepository : IBookRepository
{
    private readonly List<Book> _books = new();

    public BookRepository()
    {
        SeedData();
    }

    public List<Book> GetAll() => _books.ToList();

    public Book? GetById(int id) => _books.FirstOrDefault(b => b.Id == id);

    public List<Book> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return GetAll();

        return _books.Where(b =>
            b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            b.Publisher.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public void Add(Book book)
    {
        book.Id = _books.Count == 0 ? 1 : _books.Max(b => b.Id) + 1;
        _books.Add(book);
    }

    public void Update(Book book)
    {
        var existing = GetById(book.Id);
        if (existing == null)
            throw new InvalidOperationException($"Книга с кодом {book.Id} не найдена.");

        existing.Title = book.Title;
        existing.Author = book.Author;
        existing.Publisher = book.Publisher;
        existing.Year = book.Year;
        existing.Pages = book.Pages;
        existing.Subject = book.Subject;
        existing.Price = book.Price;
        existing.TotalCopies = book.TotalCopies;
        existing.AvailableCount = book.AvailableCount;
    }

    public void Delete(int id)
    {
        var book = GetById(id);
        if (book == null)
            throw new InvalidOperationException($"Книга с кодом {id} не найдена.");

        _books.Remove(book);
    }

    public void RegisterCopy(int bookId)
    {
        var book = GetById(bookId);
        if (book == null)
            throw new InvalidOperationException($"Книга с кодом {bookId} не найдена.");

        book.TotalCopies++;
        book.AvailableCount++;
    }

    public void WriteOffCopy(int bookId)
    {
        var book = GetById(bookId);
        if (book == null)
            throw new InvalidOperationException($"Книга с кодом {bookId} не найдена.");

        if (book.AvailableCount <= 0)
            throw new InvalidOperationException("Нельзя списать экземпляр: свободных экземпляров нет.");

        book.TotalCopies--;
        book.AvailableCount--;
    }

    private void SeedData()
    {
        var books = new[]
        {
            new Book { Id = 1, Title = "Мастер и Маргарита", Author = "Булгаков М.А.", Publisher = "Советский писатель", Year = 1990, Pages = 480, Subject = "Классика", Price = 450, TotalCopies = 3, AvailableCount = 2 },
            new Book { Id = 2, Title = "Преступление и наказание", Author = "Достоевский Ф.М.", Publisher = "Азбука", Year = 2019, Pages = 512, Subject = "Классика", Price = 520, TotalCopies = 2, AvailableCount = 2 },
            new Book { Id = 3, Title = "Основы программирования", Author = "Смирнов А.В.", Publisher = "Питер", Year = 2021, Pages = 640, Subject = "Программирование", Price = 890, TotalCopies = 4, AvailableCount = 3 },
            new Book { Id = 4, Title = "Базы данных", Author = "Иванова Е.П.", Publisher = "Юрайт", Year = 2022, Pages = 720, Subject = "Программирование", Price = 1050, TotalCopies = 2, AvailableCount = 1 },
            new Book { Id = 5, Title = "Краткая история времени", Author = "Хокинг С.", Publisher = "АСТ", Year = 2018, Pages = 336, Subject = "Наука", Price = 610, TotalCopies = 1, AvailableCount = 0 },
            new Book { Id = 6, Title = "Архитектура программного обеспечения", Author = "Клеппман М.", Publisher = "Питер", Year = 2023, Pages = 544, Subject = "Программирование", Price = 1240, TotalCopies = 2, AvailableCount = 2 },
            new Book { Id = 7, Title = "Дети капитана Гранта", Author = "Верн Ж.", Publisher = "Эксмо", Year = 2017, Pages = 576, Subject = "Приключения", Price = 430, TotalCopies = 3, AvailableCount = 3 },
            new Book { Id = 8, Title = "Чистый код", Author = "Мартин Р.", Publisher = "Питер", Year = 2020, Pages = 464, Subject = "Программирование", Price = 990, TotalCopies = 2, AvailableCount = 2 }
        };

        _books.AddRange(books);
    }
}