using LibrarySystem.Core.Models;
using LibrarySystem.Data.Repositories;
using System.Windows.Forms;

namespace LibrarySystem.UI.Screens.Panels;

/// <summary>
/// Панель выдачи книг читателям.
/// </summary>
public sealed partial class LoanPanel : Panel
{
    private readonly IBookRepository _bookRepository;
    private readonly IReaderRepository _readerRepository;
    private readonly ILoanRepository _loanRepository;

    private readonly DataGridView _gridBooks = new();
    private readonly DataGridView _gridReaders = new();
    private readonly Label _infoLabel = new();
    private readonly Button _btnIssue = new();
    private readonly Button _btnRefresh = new();

    public LoanPanel() : this(new BookRepository(), new ReaderRepository(), new LoanRepository())
    {
    }

    public LoanPanel(IBookRepository bookRepository, IReaderRepository readerRepository, ILoanRepository loanRepository)
    {
        _bookRepository = bookRepository;
        _readerRepository = readerRepository;
        _loanRepository = loanRepository;
        Dock = DockStyle.Fill;
        BuildUI();
    }

    private void BuildUI()
    {
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            Padding = new Padding(16),
            AutoSize = false,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25)); // Book label
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40));   // Book grid
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25)); // Reader label
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 35));   // Reader grid + buttons

        // Row 0: Book label
        var lblBook = new Label { Text = "Выберите книгу (свободные):", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
        layout.Controls.Add(lblBook, 0, 0);

        // Row 1: Book grid (left)
        _gridBooks.Dock = DockStyle.Fill;
        _gridBooks.ReadOnly = true;
        _gridBooks.AllowUserToAddRows = false;
        _gridBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _gridBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _gridBooks.Columns.Add("Id", "Код");
        _gridBooks.Columns.Add("Title", "Название");
        _gridBooks.Columns.Add("Author", "Автор");
        _gridBooks.Columns.Add("Available", "Свободно");
        _gridBooks.Columns["Id"].Visible = false;
        layout.Controls.Add(_gridBooks, 0, 1);

        // Row 2: Reader label (right)
        var lblReader = new Label { Text = "Выберите читателя:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
        layout.Controls.Add(lblReader, 1, 2);

        // Row 3: Reader grid + buttons (right)
        var rightPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
        };
        rightPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        rightPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
        rightPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

        _gridReaders.Dock = DockStyle.Fill;
        _gridReaders.ReadOnly = true;
        _gridReaders.AllowUserToAddRows = false;
        _gridReaders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _gridReaders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _gridReaders.Columns.Add("Id", "Код билета");
        _gridReaders.Columns.Add("FullName", "ФИО");
        _gridReaders.Columns.Add("Phone", "Телефон");
        _gridReaders.Columns["Id"].Visible = false;
        rightPanel.Controls.Add(_gridReaders, 0, 0);

        var btnPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(0, 8, 0, 0) };
        _btnIssue.Text = "Выдать книгу";
        _btnIssue.Size = new Size(140, 35);
        _btnIssue.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
        _btnIssue.ForeColor = Color.White;
        _btnRefresh.Text = "Обновить";
        _btnRefresh.Size = new Size(100, 35);
        btnPanel.Controls.Add(_btnIssue);
        btnPanel.Controls.Add(_btnRefresh);
        rightPanel.Controls.Add(btnPanel, 0, 1);

        layout.Controls.Add(rightPanel, 1, 3);

        // Info label spans bottom
        var bottomPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 1, Padding = new Padding(16, 8, 16, 16) };
        _infoLabel.AutoSize = true;
        _infoLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        bottomPanel.Controls.Add(_infoLabel, 0, 0);
        bottomPanel.Dock = DockStyle.Bottom;
        bottomPanel.Height = 40;

        Controls.Add(layout);
        Controls.Add(bottomPanel);

        _btnIssue.Click += (_, _) => IssueBook();
        _btnRefresh.Click += (_, _) => { LoadBooks(); LoadReaders(); };

        LoadBooks();
        LoadReaders();
    }

    private void LoadBooks()
    {
        _gridBooks.Rows.Clear();
        var books = _bookRepository.GetAll().Where(b => b.AvailableCount > 0).ToList();
        foreach (var b in books)
            _gridBooks.Rows.Add(b.Id, b.Title, b.Author, b.AvailableCount);
    }

    private void LoadReaders()
    {
        _gridReaders.Rows.Clear();
        var readers = _readerRepository.GetAll();
        foreach (var r in readers)
            _gridReaders.Rows.Add(r.Id, r.FullName, r.Phone);
    }

    private void IssueBook()
    {
        if (_gridBooks.CurrentRow == null || _gridReaders.CurrentRow == null)
        {
            _infoLabel.Text = "Выберите книгу и читателя.";
            return;
        }

        int bookId = (int)_gridBooks.CurrentRow.Cells[0].Value;
        int readerId = (int)_gridReaders.CurrentRow.Cells[0].Value;

        try
        {
            _loanRepository.IssueBook(bookId, readerId, DateTime.Now.AddDays(14));
            _infoLabel.Text = "Книга успешно выдана!";
            LoadBooks();
            LoadReaders();
        }
        catch (Exception ex)
        {
            _infoLabel.Text = $"Ошибка: {ex.Message}";
        }
    }
}