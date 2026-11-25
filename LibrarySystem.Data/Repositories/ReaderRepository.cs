using LibrarySystem.Core.Models;

namespace LibrarySystem.Data.Repositories;

/// <summary>
/// Интерфейс репозитория читателей.
/// </summary>
public interface IReaderRepository
{
    List<Reader> GetAll();
    Reader? GetById(int id);
    List<Reader> Search(string query);
    void Add(Reader reader);
    void Delete(int id);
}

/// <summary>
/// Репозиторий читателей. Данные хранятся в памяти (заглушка вместо БД).
/// </summary>
public sealed class ReaderRepository : IReaderRepository
{
    private readonly List<Reader> _readers = new();

    public ReaderRepository()
    {
        SeedData();
    }

    public List<Reader> GetAll() => _readers.ToList();

    public Reader? GetById(int id) => _readers.FirstOrDefault(r => r.Id == id);

    public List<Reader> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return GetAll();

        return _readers.Where(r =>
            r.FullName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            r.Id.ToString() == query)
            .ToList();
    }

    public void Add(Reader reader)
    {
        reader.Id = _readers.Count == 0 ? 1 : _readers.Max(r => r.Id) + 1;
        _readers.Add(reader);
    }

    public void Delete(int id)
    {
        var reader = GetById(id);
        if (reader == null)
            throw new InvalidOperationException($"Читатель с кодом {id} не найден.");

        _readers.Remove(reader);
    }

    private void SeedData()
    {
        var readers = new[]
        {
            new Reader { Id = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Phone = "+375(29)348-19-02", Address = "пр-т Независимости, д. 24, корп. 1" },
            new Reader { Id = 2, LastName = "Петрова", FirstName = "Мария", MiddleName = "Сергеевна", Phone = "+375(33)612-84-57", Address = "ул. Ленина, д. 48, корп. 2" },
            new Reader { Id = 3, LastName = "Сидоров", FirstName = "Алексей", MiddleName = "Владимирович", Phone = "+375(25)904-31-76", Address = "ул. Советская, д. 67, корп. 3" },
            new Reader { Id = 4, LastName = "Козлова", FirstName = "Ольга", MiddleName = "Александровна", Phone = "+375(29)753-09-44", Address = "ул. Ожешко, д. 19, корп. 1" },
            new Reader { Id = 5, LastName = "Новиков", FirstName = "Дмитрий", MiddleName = "Сергеевич", Phone = "+375(44)581-26-80", Address = "пр-т Победы, д. 12, корп. 4" }
        };

        _readers.AddRange(readers);
    }
}