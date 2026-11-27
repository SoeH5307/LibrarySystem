using Microsoft.Data.Sqlite;

namespace LibrarySystem.Data;

/// <summary>
/// Инициализация базы данных: создание таблиц и начальных данных.
/// </summary>
public static class DbInitializer
{
    public static void Initialize(SqliteConnectionFactory factory)
    {
        using var conn = factory.CreateConnection();

        // Включаем поддержку внешних ключей
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "PRAGMA foreign_keys = ON;";
            cmd.ExecuteNonQuery();
        }

        // Создаём таблицы
        ExecuteNonQuery(conn, @"
            CREATE TABLE IF NOT EXISTS Books (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Author TEXT DEFAULT '',
                Publisher TEXT DEFAULT '',
                Year INTEGER DEFAULT 0,
                Pages INTEGER DEFAULT 0,
                Subject TEXT DEFAULT '',
                Price REAL DEFAULT 0,
                TotalCopies INTEGER DEFAULT 0,
                AvailableCount INTEGER DEFAULT 0
            );");

        ExecuteNonQuery(conn, @"
            CREATE TABLE IF NOT EXISTS Readers (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                LastName TEXT NOT NULL,
                FirstName TEXT NOT NULL,
                MiddleName TEXT DEFAULT '',
                Phone TEXT DEFAULT '',
                Address TEXT DEFAULT ''
            );");

        ExecuteNonQuery(conn, @"
            CREATE TABLE IF NOT EXISTS Loans (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                BookId INTEGER NOT NULL,
                ReaderId INTEGER NOT NULL,
                IssueDate TEXT NOT NULL,
                DueDate TEXT NOT NULL,
                ReturnDate TEXT,
                FOREIGN KEY (BookId) REFERENCES Books(Id) ON DELETE CASCADE,
                FOREIGN KEY (ReaderId) REFERENCES Readers(Id) ON DELETE CASCADE
            );");

        ExecuteNonQuery(conn, @"
            CREATE INDEX IF NOT EXISTS IX_Loans_BookId ON Loans(BookId);
            CREATE INDEX IF NOT EXISTS IX_Loans_ReaderId ON Loans(ReaderId);
            CREATE INDEX IF NOT EXISTS IX_Loans_ReturnDate ON Loans(ReturnDate);");

        // Seed-данные, если таблицы пустые
        if (IsTableEmpty(conn, "Books"))
        {
            SeedBooks(conn);
        }

        if (IsTableEmpty(conn, "Readers"))
        {
            SeedReaders(conn);
        }
    }

    private static void ExecuteNonQuery(SqliteConnection conn, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    private static bool IsTableEmpty(SqliteConnection conn, string table)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT COUNT(*) FROM {table};";
        return (long)cmd.ExecuteScalar()! == 0;
    }

    private static void SeedBooks(SqliteConnection conn)
    {
        var books = new[]
        {
            new { Title = "Мастер и Маргарита", Author = "Булгаков М.А.", Publisher = "Советский писатель", Year = 1990, Pages = 480, Subject = "Классика", Price = 450m, Total = 3, Avail = 2 },
            new { Title = "Преступление и наказание", Author = "Достоевский Ф.М.", Publisher = "Азбука", Year = 2019, Pages = 512, Subject = "Классика", Price = 520m, Total = 2, Avail = 2 },
            new { Title = "Основы программирования", Author = "Смирнов А.В.", Publisher = "Питер", Year = 2021, Pages = 640, Subject = "Программирование", Price = 890m, Total = 4, Avail = 3 },
            new { Title = "Базы данных", Author = "Иванова Е.П.", Publisher = "Юрайт", Year = 2022, Pages = 720, Subject = "Программирование", Price = 1050m, Total = 2, Avail = 1 },
            new { Title = "Краткая история времени", Author = "Хокинг С.", Publisher = "АСТ", Year = 2018, Pages = 336, Subject = "Наука", Price = 610m, Total = 1, Avail = 0 },
            new { Title = "Архитектура программного обеспечения", Author = "Клеппман М.", Publisher = "Питер", Year = 2023, Pages = 544, Subject = "Программирование", Price = 1240m, Total = 2, Avail = 2 },
            new { Title = "Дети капитана Гранта", Author = "Верн Ж.", Publisher = "Эксмо", Year = 2017, Pages = 576, Subject = "Приключения", Price = 430m, Total = 3, Avail = 3 },
            new { Title = "Чистый код", Author = "Мартин Р.", Publisher = "Питер", Year = 2020, Pages = 464, Subject = "Программирование", Price = 990m, Total = 2, Avail = 2 }
        };

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Books (Title, Author, Publisher, Year, Pages, Subject, Price, TotalCopies, AvailableCount)
            VALUES ($title, $author, $publisher, $year, $pages, $subject, $price, $total, $avail)";

        var pTitle = cmd.Parameters.Add("$title", SqliteType.Text);
        var pAuthor = cmd.Parameters.Add("$author", SqliteType.Text);
        var pPublisher = cmd.Parameters.Add("$publisher", SqliteType.Text);
        var pYear = cmd.Parameters.Add("$year", SqliteType.Integer);
        var pPages = cmd.Parameters.Add("$pages", SqliteType.Integer);
        var pSubject = cmd.Parameters.Add("$subject", SqliteType.Text);
        var pPrice = cmd.Parameters.Add("$price", SqliteType.Real);
        var pTotal = cmd.Parameters.Add("$total", SqliteType.Integer);
        var pAvail = cmd.Parameters.Add("$avail", SqliteType.Integer);

        foreach (var b in books)
        {
            pTitle.Value = b.Title;
            pAuthor.Value = b.Author;
            pPublisher.Value = b.Publisher;
            pYear.Value = b.Year;
            pPages.Value = b.Pages;
            pSubject.Value = b.Subject;
            pPrice.Value = (double)b.Price;
            pTotal.Value = b.Total;
            pAvail.Value = b.Avail;
            cmd.ExecuteNonQuery();
        }
    }

    private static void SeedReaders(SqliteConnection conn)
    {
        var readers = new[]
        {
            new { LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Phone = "+375(29)348-19-02", Address = "г. Минск, пр-т Независимости, д. 24, кв. 15" },
            new { LastName = "Петрова", FirstName = "Мария", MiddleName = "Сергеевна", Phone = "+375(33)612-84-57", Address = "г. Минск, ул. Ленина, д. 48, кв. 7" },
            new { LastName = "Сидоров", FirstName = "Алексей", MiddleName = "Владимирович", Phone = "+375(25)904-31-76", Address = "г. Витебск, ул. Советская, д. 67" },
            new { LastName = "Козлова", FirstName = "Ольга", MiddleName = "Александровна", Phone = "+375(29)753-09-44", Address = "г. Минск, ул. Ожешко, д. 19, кв. 32" },
            new { LastName = "Новиков", FirstName = "Дмитрий", MiddleName = "Сергеевич", Phone = "+375(44)581-26-80", Address = "г. Гродно, пр-т Победы, д. 12, кв. 5" }
        };

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Readers (LastName, FirstName, MiddleName, Phone, Address)
            VALUES ($ln, $fn, $mn, $phone, $addr)";

        var pLn = cmd.Parameters.Add("$ln", SqliteType.Text);
        var pFn = cmd.Parameters.Add("$fn", SqliteType.Text);
        var pMn = cmd.Parameters.Add("$mn", SqliteType.Text);
        var pPhone = cmd.Parameters.Add("$phone", SqliteType.Text);
        var pAddr = cmd.Parameters.Add("$addr", SqliteType.Text);

        foreach (var r in readers)
        {
            pLn.Value = r.LastName;
            pFn.Value = r.FirstName;
            pMn.Value = r.MiddleName;
            pPhone.Value = r.Phone;
            pAddr.Value = r.Address;
            cmd.ExecuteNonQuery();
        }
    }
}