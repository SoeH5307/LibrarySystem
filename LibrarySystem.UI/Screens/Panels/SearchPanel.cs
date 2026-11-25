using LibrarySystem.Core.Models;
using LibrarySystem.Data.Repositories;

namespace LibrarySystem.UI.Screens.Panels;

/// <summary>
/// Панель поиска книг: поиск по названию, автору, году.
/// </summary>
public sealed partial class SearchPanel : Panel
{
    private readonly IBookRepository _bookRepository;
    private readonly DataGridView _grid = new();
    private readonly Label _infoLabel = new();

    public SearchPanel() : this(new BookRepository())
    {
    }

    public SearchPanel(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
        Dock = DockStyle.Fill;
        BuildUI();
    }

    private void BuildUI()
    {
        var lbl = new Label { Text = "Поиск книг:", Location = new System.Drawing.Point(16, 12), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };

        var lblTitle = new Label { Text = "Название:", Location = new System.Drawing.Point(16, 40), AutoSize = true };
        var txtTitle = new TextBox { Location = new System.Drawing.Point(100, 36), Size = new System.Drawing.Size(200, 23) };

        var lblAuthor = new Label { Text = "Автор:", Location = new System.Drawing.Point(310, 40), AutoSize = true };
        var txtAuthor = new TextBox { Location = new System.Drawing.Point(370, 36), Size = new System.Drawing.Size(200, 23) };

        var lblYear = new Label { Text = "Год:", Location = new System.Drawing.Point(580, 40), AutoSize = true };
        var txtYear = new TextBox { Location = new System.Drawing.Point(620, 36), Size = new System.Drawing.Size(80, 23) };

        var btnSearch = new Button { Text = "Найти", Location = new System.Drawing.Point(720, 36), Size = new System.Drawing.Size(80, 30) };
        var btnClear = new Button { Text = "Сбросить", Location = new System.Drawing.Point(800, 36), Size = new System.Drawing.Size(100, 30) };

        _grid.Location = new System.Drawing.Point(16, 70);
        _grid.Size = new System.Drawing.Size(992, 320);
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

        _infoLabel.Location = new System.Drawing.Point(16, 405);
        _infoLabel.AutoSize = true;
        _infoLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);

        btnSearch.Click += (_, _) => DoSearch(txtTitle.Text, txtAuthor.Text, txtYear.Text);
        btnClear.Click += (_, _) =>
        {
            txtTitle.Text = txtAuthor.Text = txtYear.Text = string.Empty;
            LoadAll();
        };

        Controls.Add(lbl);
        Controls.Add(lblTitle);
        Controls.Add(txtTitle);
        Controls.Add(lblAuthor);
        Controls.Add(txtAuthor);
        Controls.Add(lblYear);
        Controls.Add(txtYear);
        Controls.Add(btnSearch);
        Controls.Add(btnClear);
        Controls.Add(_grid);
        Controls.Add(_infoLabel);

        LoadAll();
    }

    private void LoadAll()
    {
        var books = _bookRepository.GetAll();
        FillGrid(books);
    }

    private void DoSearch(string title, string author, string yearStr)
    {
        var books = _bookRepository.GetAll();
        var filtered = books.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(title))
            filtered = filtered.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(author))
            filtered = filtered.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(yearStr) && int.TryParse(yearStr, out int year))
            filtered = filtered.Where(b => b.Year == year);

        var result = filtered.ToList();
        FillGrid(result);
    }

    private void FillGrid(List<Book> books)
    {
        _grid.Rows.Clear();
        foreach (var b in books)
            _grid.Rows.Add(b.Id, b.Title, b.Author, b.Publisher, b.Year, b.Pages, b.AvailableCount);
        _infoLabel.Text = $"Найдено: {books.Count}";
    }
}