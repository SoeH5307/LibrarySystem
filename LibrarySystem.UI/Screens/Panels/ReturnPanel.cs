using LibrarySystem.Core.Models;
using LibrarySystem.Data.Repositories;
using System.Windows.Forms;

namespace LibrarySystem.UI.Screens.Panels;

/// <summary>
/// Панель возврата книг.
/// </summary>
public sealed partial class ReturnPanel : Panel
{
    private readonly ILoanRepository _loanRepository;
    private readonly DataGridView _gridLoans = new();
    private readonly Label _infoLabel = new();
    private readonly Button _btnReturn = new();
    private readonly Button _btnRefresh = new();

    public ReturnPanel(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
        Dock = DockStyle.Fill;
        BuildUI();
    }

    private void BuildUI()
    {
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(16),
            AutoSize = false,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

        var lbl = new Label { Text = "Активные выдачи (не возвращённые):", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
        layout.Controls.Add(lbl, 0, 0);

        _gridLoans.Dock = DockStyle.Fill;
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
        layout.Controls.Add(_gridLoans, 0, 1);

        var bottomPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, Padding = new Padding(16, 0, 16, 16) };
        bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
        bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));

        _btnReturn.Text = "Принять возврат";
        _btnReturn.Size = new Size(140, 35);
        _btnReturn.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
        _btnReturn.ForeColor = Color.White;
        bottomPanel.Controls.Add(_btnReturn, 1, 0);

        _btnRefresh.Text = "Обновить";
        _btnRefresh.Size = new Size(100, 35);
        bottomPanel.Controls.Add(_btnRefresh, 2, 0);

        _infoLabel.AutoSize = true;
        _infoLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        bottomPanel.Controls.Add(_infoLabel, 0, 0);
        _infoLabel.Anchor = AnchorStyles.Left;
        _infoLabel.Margin = new Padding(0, 6, 0, 0);

        layout.Controls.Add(bottomPanel, 0, 2);

        Controls.Add(layout);

        _btnReturn.Click += (_, _) => ReturnBook();
        _btnRefresh.Click += (_, _) => LoadLoans();

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