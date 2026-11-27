# LibrarySystem (ИС «Библиотека»)

Информационная система учёта книг, читателей и выдачи литературы для специализированной библиотеки (Вариант 9).
Разработана в рамках учебной практики «Технология разработки программного обеспечения».

## Структура решения

| Проект | Назначение |
|--------|------------|
| `LibrarySystem.UI` | Windows Forms представление (View), корень композиции (`Program.cs`), навигация между панелями |
| `LibrarySystem.Logic` | Presenters (шаблон MVP), интерфейсы представления и навигации |
| `LibrarySystem.Core` | Модели-сущности предметной области (`Book`, `Reader`, `Loan`) |
| `LibrarySystem.Data` | Репозитории доступа к данным, фабрика соединений SQLite, инициализация БД |

## Архитектура

**MVP (Model-View-Presenter):**
- `View` - Windows Forms формы, ничего не знают о базе данных
- `Presenter` - обрабатывает действия пользователя, управляет отображением
- `Model` - сущности `Book`, `Reader`, `Loan`

**Шаблоны проектирования:**
- **MVP** - разделение представления и логики
- **Наблюдатель (События)** - связь View и Presenter через события
- **Фабричный метод (`ScreenFactory`)** - создание панелей разделов
- **Внедрение зависимостей (DI)** - репозитории создаются один раз в `Program.cs` и передаются в панели

## Стек технологий

- .NET 9.0
- Windows Forms (WinForms)
- SQLite (файловая база данных `library.db`)
- Microsoft.Data.Sqlite

## Функциональность по ролям

### Библиотекарь
- Каталог и фонд: просмотр книг, поиск, добавление изданий, приём/списание экземпляров
- Читатели: регистрация, просмотр, удаление (если нет невозвращённых книг)
- Выдача книги: выбор книги и читателя, оформление выдачи
- Возврат книги: приём возвращённой книги
- Поиск книг: фильтр по названию, автору, году

### Читатель
- Каталог: просмотр книг
- Поиск: поиск книг
- Профиль: просмотр своих активных выдач

## Запуск

1. Откройте `LibrarySystem.sln` в Visual Studio 2022 (17.14 и выше)
2. Убедитесь, что установлен .NET SDK 9.0
3. Назначьте `LibrarySystem.UI` запускаемым проектом (по умолчанию так и есть)
4. Нажмите F5

**При первом запуске** файл базы данных `library.db` создаётся автоматически в папке запуска приложения с начальными данными (8 книг, 5 читателей).

**При каждом запуске** данные сохраняются в `library.db` - изменения не теряются при перезапуске.

## Схема базы данных (SQLite)

```sql
CREATE TABLE Books (
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
);

CREATE TABLE Readers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    LastName TEXT NOT NULL,
    FirstName TEXT NOT NULL,
    MiddleName TEXT DEFAULT '',
    Phone TEXT DEFAULT '',
    Address TEXT DEFAULT ''
);

CREATE TABLE Loans (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    BookId INTEGER NOT NULL,
    ReaderId INTEGER NOT NULL,
    IssueDate TEXT NOT NULL,
    DueDate TEXT NOT NULL,
    ReturnDate TEXT,
    FOREIGN KEY (BookId) REFERENCES Books(Id),
    FOREIGN KEY (ReaderId) REFERENCES Readers(Id)
);
```

## Репозитории (доступ к данным)

| Репозиторий | Интерфейс | Файл |
|-------------|-----------|------|
| Книги | `IBookRepository` | `SqliteBookRepository.cs` |
| Читатели | `IReaderRepository` | `SqliteReaderRepository.cs` |
| Выдачи | `ILoanRepository` | `SqliteLoanRepository.cs` |

## Интерфейс

- Одно окно с меню сверху и панелями снизу (без открытия новых окон)
- Активный пункт меню подсвечивается
- Читатель видит только разрешенные разделы (Каталог, Поиск, Профиль)
- Библиотекарь имеет доступ ко всем разделам
- Все подписи на русском языке

## Репозиторий на GitHub

https://github.com/SoeH5307/LibrarySystem
