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
            new Reader { Id = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Phone = "+7(900)123-45-67", Address = "г. Москва, ул. Ленина, 1" },
            new Reader { Id = 2, LastName = "Петрова", FirstName = "Мария", MiddleName = "Сергеевна", Phone = "+7(900)234-56-78", Address = "г. Москва, ул. Пушкина, 5" },
            new Reader { Id = 3, LastName = "Сидоров", FirstName = "Алексей", MiddleName = "Владимирович", Phone = "+7(900)345-67-89", Address = "г. Москва, ул. Гагарина, 10" },
            new Reader { Id = 4, LastName = "Козлова", FirstName = "Ольга", MiddleName = "Александровна", Phone = "+7(900)456-78-90", Address = "г. Москва, ул. Советская, 15" },
            new Reader { Id = 5, LastName = "Новиков", FirstName = "Дмитрий", MiddleName = "Сергеевич", Phone = "+7(900)567-89-01", Address = "г. Москва, ул. Мира, 20" }
        };

        _readers.AddRange(readers);
    }
}