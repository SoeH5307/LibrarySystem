using LibrarySystem.Core.Models;
using LibrarySystem.Data.Repositories;

namespace LibrarySystem.UI.Screens.Panels;

/// <summary>
/// Панель каталога и фонда: список книг, поиск по каталогу.
/// </summary>
public sealed partial class CatalogPanel : Panel
{
    private readonly IBookRepository _bookRepository;
    private readonly DataGridView _grid = new();
    private readonly Label _infoLabel = new();

    public CatalogPanel() : this(new BookRepository())
    {
    }

    public CatalogPanel(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
        Dock = DockStyle.Fill;
        BuildUI();
    }

    private void BuildUI()
    {
        // Поиск
        var searchLabel = new Label { Text = "Поиск по названию или автору:", Location = new System.Drawing.Point(16, 12), AutoSize = true };
        var searchBox = new TextBox { Location = new System.Drawing.Point(250, 8), Size = new System.Drawing.Size(310, 30) };
        var btnSearch = new Button { Text = "Найти", Location = new System.Drawing.Point(580, 8), Size = new System.Drawing.Size(80, 30) };
        var btnShowAll = new Button { Text = "Показать всё", Location = new System.Drawing.Point(660, 8), Size = new System.Drawing.Size(120, 30) };

        // Таблица
        _grid.Location = new System.Drawing.Point(16, 50);
        _grid.Size = new System.Drawing.Size(992, 300);
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.Columns.Add("Id", "Код");
        _grid.Columns.Add("Title", "Название");
        _grid.Columns.Add("Author", "Автор");
        _grid.Columns.Add("Publisher", "Издательство");
        _grid.Columns.Add("Year", "Год");
        _grid.Columns.Add("Pages", "Страниц");
        _grid.Columns.Add("Available", "Свободно");
        _grid.Columns["Id"].Visible = false;

        // Инфо
        _infoLabel.Location = new System.Drawing.Point(16, 365);
        _infoLabel.AutoSize = true;
        _infoLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);

        // Кнопки
        var btnRefresh = new Button { Text = "Обновить", Location = new System.Drawing.Point(16, 410), Size = new System.Drawing.Size(90, 30) };

        // События
        btnShowAll.Click += (_, _) => LoadBooks(string.Empty);
        btnSearch.Click += (_, _) => LoadBooks(searchBox.Text);
        searchBox.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadBooks(searchBox.Text); };
        btnRefresh.Click += (_, _) => LoadBooks(string.Empty);

        Controls.Add(searchLabel);
        Controls.Add(searchBox);
        Controls.Add(btnSearch);
        Controls.Add(btnShowAll);
        Controls.Add(_grid);
        Controls.Add(_infoLabel);
        Controls.Add(btnRefresh);

        LoadBooks(string.Empty);
    }

    private void LoadBooks(string filter)
    {
        try
        {
            var books = string.IsNullOrWhiteSpace(filter)
                ? _bookRepository.GetAll()
                : _bookRepository.Search(filter);

            _grid.Rows.Clear();
            foreach (var b in books)
            {
                _grid.Rows.Add(b.Id, b.Title, b.Author, b.Publisher, b.Year, b.Pages, b.AvailableCount);
            }

            _infoLabel.Text = $"Найдено записей: {books.Count}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Ошибка загрузки каталога: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}