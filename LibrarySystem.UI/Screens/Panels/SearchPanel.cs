using LibrarySystem.Core.Models;
using LibrarySystem.Data.Repositories;
using System.Windows.Forms;

namespace LibrarySystem.UI.Screens.Panels;

/// <summary>
/// Панель поиска книг: поиск по названию, автору, году.
/// </summary>
public sealed partial class SearchPanel : Panel
{
    private readonly IBookRepository _bookRepository;
    private readonly DataGridView _grid = new();
    private readonly Label _infoLabel = new();
    private readonly TextBox _txtTitle = new();
    private readonly TextBox _txtAuthor = new();
    private readonly TextBox _txtYear = new();
    private readonly Button _btnSearch = new();
    private readonly Button _btnClear = new();

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
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 7,
            RowCount = 2,
            Padding = new Padding(16),
            AutoSize = false,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));   // Название:
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));  // input
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));   // Автор:
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));  // input
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));   // Год:
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));   // input
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));  // buttons
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var lblTitle = new Label { Text = "Название:", Anchor = AnchorStyles.Left, AutoSize = true };
        _txtTitle.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _txtTitle.Size = new Size(200, 23);

        var lblAuthor = new Label { Text = "Автор:", Anchor = AnchorStyles.Left, AutoSize = true };
        _txtAuthor.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _txtAuthor.Size = new Size(200, 23);

        var lblYear = new Label { Text = "Год:", Anchor = AnchorStyles.Left, AutoSize = true };
        _txtYear.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _txtYear.Size = new Size(80, 23);

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Padding = new Padding(0) };
        _btnSearch.Text = "Найти";
        _btnSearch.Size = new Size(90, 30);
        _btnClear.Text = "Сбросить";
        _btnClear.Size = new Size(90, 30);
        buttons.Controls.Add(_btnSearch);
        buttons.Controls.Add(_btnClear);

        layout.Controls.Add(lblTitle, 0, 0);
        layout.Controls.Add(_txtTitle, 1, 0);
        layout.Controls.Add(lblAuthor, 2, 0);
        layout.Controls.Add(_txtAuthor, 3, 0);
        layout.Controls.Add(lblYear, 4, 0);
        layout.Controls.Add(_txtYear, 5, 0);
        layout.Controls.Add(buttons, 6, 0);

        _grid.Dock = DockStyle.Fill;
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
        layout.Controls.Add(_grid, 0, 1);
        layout.SetColumnSpan(_grid, 7);

        _infoLabel.AutoSize = true;
        _infoLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        _infoLabel.Location = new System.Drawing.Point(16, 452);
        _infoLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

        Controls.Add(layout);
        Controls.Add(_infoLabel);

        _btnSearch.Click += (_, _) => DoSearch(_txtTitle.Text, _txtAuthor.Text, _txtYear.Text);
        _btnClear.Click += (_, _) =>
        {
            _txtTitle.Text = _txtAuthor.Text = _txtYear.Text = string.Empty;
            LoadAll();
        };

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