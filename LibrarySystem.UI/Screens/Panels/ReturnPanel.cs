using LibrarySystem.Core.Models;

namespace LibrarySystem.UI.Screens.Panels;

/// <summary>
/// Панель возврата книг.
/// </summary>
public sealed partial class ReturnPanel : Panel
{
    private readonly ILoanRepository _loanRepository;
    private readonly DataGridView _gridLoans = new();
    private readonly Label _infoLabel = new();

    public ReturnPanel() : this(new LoanRepository())
    {
    }

    public ReturnPanel(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
        Dock = DockStyle.Fill;
        BuildUI();
    }

    private void BuildUI()
    {
        var lbl = new Label { Text = "Активные выдачи (не возвращённые):", Location = new System.Drawing.Point(16, 12), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
        _gridLoans.Location = new System.Drawing.Point(16, 35);
        _gridLoans.Size = new System.Drawing.Size(992, 350);
        _gridLoans.ReadOnly = true;
        _gridLoans.AllowUserToAddRows = false;
        _gridLoans.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _gridLoans.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _gridLoans.Columns.Add("LoanId", "Код выдачи");
        _gridLoans.Columns.Add("BookTitle", "Книга");
        _gridLoans.Columns.Add("ReaderName", "Читатель");
        _gridLoans.Columns.Add("IssueDate", "Дата выдачи");
        _gridLoans.Columns.Add("DueDate", "Срок возврата");
        _gridLoans.Columns["LoanId"].Visible = false;

        var btnReturn = new Button { Text = "Принять возврат", Location = new System.Drawing.Point(16, 400), Size = new System.Drawing.Size(140, 35), BackColor = System.Drawing.Color.FromArgb(0, 120, 215), ForeColor = Color.White };
        var btnRefresh = new Button { Text = "Обновить", Location = new System.Drawing.Point(170, 400), Size = new System.Drawing.Size(100, 35) };

        _infoLabel.Location = new System.Drawing.Point(16, 450);
        _infoLabel.AutoSize = true;
        _infoLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);

        btnReturn.Click += (_, _) => ReturnBook();
        btnRefresh.Click += (_, _) => LoadLoans();

        Controls.Add(lbl);
        Controls.Add(_gridLoans);
        Controls.Add(btnReturn);
        Controls.Add(btnRefresh);
        Controls.Add(_infoLabel);

        LoadLoans();
    }

    private void LoadLoans()
    {
        _gridLoans.Rows.Clear();
        var loans = _loanRepository.GetActiveLoans();
        foreach (var l in loans)
        {
            _gridLoans.Rows.Add(l.Id, l.BookTitle, l.ReaderName, l.IssueDate.ToShortDateString(), l.DueDate.ToShortDateString());
        }
        _infoLabel.Text = $"Активных выдач: {loans.Count}";
    }

    private void ReturnBook()
    {
        if (_gridLoans.CurrentRow == null)
        {
            _infoLabel.Text = "Выберите выдачу для возврата.";
            return;
        }

        int loanId = (int)_gridLoans.CurrentRow.Cells[0].Value;
        try
        {
            _loanRepository.ReturnBook(loanId, DateTime.Now);
            _infoLabel.Text = "Книга успешно возвращена!";
            LoadLoans();
        }
        catch (Exception ex)
        {
            _infoLabel.Text = $"Ошибка: {ex.Message}";
        }
    }
}