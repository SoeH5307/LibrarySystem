using LibrarySystem.Core.Models;
using LibrarySystem.Data.Repositories;
using System.Windows.Forms;

namespace LibrarySystem.UI.Screens.Panels;

/// <summary>
/// Панель каталога и фонда: список книг, поиск, добавление/списание экземпляров.
/// </summary>
public sealed partial class CatalogPanel : Panel
{
    private readonly IBookRepository _bookRepository;
    private readonly DataGridView _grid = new();
    private readonly Label _infoLabel = new();
    private readonly TextBox _searchBox = new();
    private readonly Button _btnSearch = new();
    private readonly Button _btnShowAll = new();
    private readonly Button _btnRefresh = new();
    private readonly Button _btnAddEdition = new();
    private readonly Button _btnAddCopy = new();
    private readonly Button _btnWriteOff = new();

    public CatalogPanel(IBookRepository bookRepository)
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
            ColumnCount = 5,
            RowCount = 3,
            Padding = new Padding(16),
            AutoSize = false,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));

        var lblSearch = new Label { Text = "Поиск (название/автор):", Anchor = AnchorStyles.Left, AutoSize = true };
        _searchBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _searchBox.Size = new Size(250, 23);
        _btnSearch.Text = "Найти";
        _btnSearch.Size = new Size(80, 30);
        _btnSearch.Anchor = AnchorStyles.Left;
        _btnShowAll.Text = "Показать всё";
        _btnShowAll.Size = new Size(100, 30);
        _btnShowAll.Anchor = AnchorStyles.Left;

        layout.Controls.Add(lblSearch, 0, 0);
        layout.Controls.Add(_searchBox, 1, 0);
        layout.Controls.Add(_btnSearch, 2, 0);
        layout.Controls.Add(_btnShowAll, 3, 0);

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
        layout.SetColumnSpan(_grid, 5);

        var btnPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(0, 8, 0, 0) };
        _btnAddEdition.Text = "Добавить издание";
        _btnAddEdition.Size = new Size(140, 30);
        _btnAddCopy.Text = "Принять экземпляр";
        _btnAddCopy.Size = new Size(140, 30);
        _btnWriteOff.Text = "Списать экземпляр";
        _btnWriteOff.Size = new Size(140, 30);
        _btnRefresh.Text = "Обновить";
        _btnRefresh.Size = new Size(90, 30);
        btnPanel.Controls.Add(_btnAddEdition);
        btnPanel.Controls.Add(_btnAddCopy);
        btnPanel.Controls.Add(_btnWriteOff);
        btnPanel.Controls.Add(_btnRefresh);

        var bottomPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Padding = new Padding(16, 8, 16, 16) };
        bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        _infoLabel.AutoSize = true;
        _infoLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        _infoLabel.Anchor = AnchorStyles.Left;
        _infoLabel.Margin = new Padding(0, 6, 0, 0);
        bottomPanel.Controls.Add(_infoLabel, 0, 0);
        bottomPanel.Controls.Add(btnPanel, 1, 0);

        layout.Controls.Add(bottomPanel, 0, 2);
        layout.SetColumnSpan(bottomPanel, 5);

        Controls.Add(layout);

        _btnShowAll.Click += (_, _) => LoadBooks(string.Empty);
        _btnSearch.Click += (_, _) => LoadBooks(_searchBox.Text);
        _searchBox.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadBooks(_searchBox.Text); };
        _btnRefresh.Click += (_, _) => LoadBooks(string.Empty);
        _btnAddEdition.Click += (_, _) => AddEdition();
        _btnAddCopy.Click += (_, _) => AddCopy();
        _btnWriteOff.Click += (_, _) => WriteOffCopy();

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

    private void AddEdition()
    {
        using var dlg = new Form
        {
            Text = "Добавить издание",
            Size = new Size(400, 300),
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false,
        };

        var lblTitle = new Label { Text = "Название:", Location = new Point(20, 20), AutoSize = true };
        var txtTitle = new TextBox { Location = new Point(120, 16), Size = new Size(250, 23) };
        var lblAuthor = new Label { Text = "Автор:", Location = new Point(20, 50), AutoSize = true };
        var txtAuthor = new TextBox { Location = new Point(120, 46), Size = new Size(250, 23) };
        var lblPublisher = new Label { Text = "Издательство:", Location = new Point(20, 80), AutoSize = true };
        var txtPublisher = new TextBox { Location = new Point(120, 76), Size = new Size(250, 23) };
        var lblYear = new Label { Text = "Год:", Location = new Point(20, 110), AutoSize = true };
        var txtYear = new TextBox { Location = new Point(120, 106), Size = new Size(80, 23) };
        var lblPages = new Label { Text = "Страниц:", Location = new Point(20, 140), AutoSize = true };
        var txtPages = new TextBox { Location = new Point(120, 136), Size = new Size(80, 23) };
        var btnOk = new Button { Text = "Добавить", Location = new Point(120, 190), Size = new Size(100, 30), DialogResult = DialogResult.OK };
        var btnCancel = new Button { Text = "Отмена", Location = new Point(240, 190), Size = new Size(100, 30), DialogResult = DialogResult.Cancel };

        dlg.Controls.AddRange(new Control[] { lblTitle, txtTitle, lblAuthor, txtAuthor, lblPublisher, txtPublisher, lblYear, txtYear, lblPages, txtPages, btnOk, btnCancel });
        dlg.AcceptButton = btnOk;
        dlg.CancelButton = btnCancel;

        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            try
            {
                var book = new Book
                {
                    Title = txtTitle.Text.Trim(),
                    Author = txtAuthor.Text.Trim(),
                    Publisher = txtPublisher.Text.Trim(),
                    Year = int.TryParse(txtYear.Text, out int y) ? y : 0,
                    Pages = int.TryParse(txtPages.Text, out int p) ? p : 0,
                    AvailableCount = 0,
                    TotalCopies = 0
                };
                _bookRepository.Add(book);
                LoadBooks(string.Empty);
                _infoLabel.Text = $"Издание добавлено (код {book.Id}).";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void AddCopy()
    {
        if (_grid.CurrentRow == null)
        {
            _infoLabel.Text = "Выберите издание в таблице.";
            return;
        }

        int bookId = (int)_grid.CurrentRow.Cells[0].Value;
        try
        {
            _bookRepository.RegisterCopy(bookId);
            LoadBooks(string.Empty);
            _infoLabel.Text = "Экземпляр принят в фонд.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void WriteOffCopy()
    {
        if (_grid.CurrentRow == null)
        {
            _infoLabel.Text = "Выберите издание в таблице.";
            return;
        }

        int bookId = (int)_grid.CurrentRow.Cells[0].Value;
        try
        {
            _bookRepository.WriteOffCopy(bookId);
            LoadBooks(string.Empty);
            _infoLabel.Text = "Экземпляр списан.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}