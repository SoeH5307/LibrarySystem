using LibrarySystem.Core.Models;

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
        // Секция: выбор книги
        var lblBook = new Label { Text = "Выберите книгу (свободные):", Location = new System.Drawing.Point(16, 12), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
        _gridBooks.Location = new System.Drawing.Point(16, 35);
        _gridBooks.Size = new System.Drawing.Size(480, 200);
        _gridBooks.ReadOnly = true;
        _gridBooks.AllowUserToAddRows = false;
        _gridBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _gridBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _gridBooks.Columns.Add("Id", "Код");
        _gridBooks.Columns.Add("Title", "Название");
        _gridBooks.Columns.Add("Author", "Автор");
        _gridBooks.Columns.Add("Available", "Свободно");
        _gridBooks.Columns["Id"].Visible = false;

        // Секция: выбор читателя
        var lblReader = new Label { Text = "Выберите читателя:", Location = new System.Drawing.Point(520, 12), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
        _gridReaders.Location = new System.Drawing.Point(520, 35);
        _gridReaders.Size = new System.Drawing.Size(480, 200);
        _gridReaders.ReadOnly = true;
        _gridReaders.AllowUserToAddRows = false;
        _gridReaders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _gridReaders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _gridReaders.Columns.Add("Id", "Код билета");
        _gridReaders.Columns.Add("Name", "ФИО");
        _gridReaders.Columns.Add("Phone", "Телефон");
        _gridReaders.Columns["Id"].Visible = false;

        // Кнопка выдачи
        var btnIssue = new Button { Text = "Выдать книгу", Location = new System.Drawing.Point(16, 260), Size = new System.Drawing.Size(140, 35), BackColor = System.Drawing.Color.FromArgb(0, 120, 215), ForeColor = Color.White };
        var btnRefresh = new Button { Text = "Обновить", Location = new System.Drawing.Point(170, 260), Size = new System.Drawing.Size(100, 35) };

        _infoLabel.Location = new System.Drawing.Point(16, 310);
        _infoLabel.AutoSize = true;
        _infoLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);

        btnIssue.Click += (_, _) => IssueBook();
        btnRefresh.Click += (_, _) => { LoadBooks(); LoadReaders(); };

        Controls.Add(lblBook);
        Controls.Add(_gridBooks);
        Controls.Add(lblReader);
        Controls.Add(_gridReaders);
        Controls.Add(btnIssue);
        Controls.Add(btnRefresh);
        Controls.Add(_infoLabel);

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