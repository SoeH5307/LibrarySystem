using LibrarySystem.Core.Models;
using LibrarySystem.Data.Repositories;

namespace LibrarySystem.UI.Screens.Panels;

/// <summary>
/// Панель профиля читателя: мои книги, штрафы, история.
/// </summary>
public sealed partial class ProfilePanel : Panel
{
    private readonly IReaderRepository _readerRepository;
    private readonly ILoanRepository _loanRepository;
    private readonly DataGridView _gridMyBooks = new();
    private readonly Label _infoLabel = new();

    public ProfilePanel(IReaderRepository readerRepository, ILoanRepository loanRepository)
    {
        _readerRepository = readerRepository;
        _loanRepository = loanRepository;
        Dock = DockStyle.Fill;
        BuildUI();
    }

    private void BuildUI()
    {
        var lblTitle = new Label { Text = "Мой профиль и мои книги", Location = new System.Drawing.Point(16, 12), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };

        var lblReader = new Label { Text = "Читатель:", Location = new System.Drawing.Point(16, 40), AutoSize = true };
        var txtReaderId = new TextBox { Location = new System.Drawing.Point(90, 36), Size = new System.Drawing.Size(100, 23) };
        var btnLoad = new Button { Text = "Загрузить", Location = new System.Drawing.Point(200, 36), Size = new System.Drawing.Size(80, 23) };

        var lblInfo = new Label { Text = "Мои активные выдачи:", Location = new System.Drawing.Point(16, 70), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };

        _gridMyBooks.Location = new System.Drawing.Point(16, 95);
        _gridMyBooks.Size = new System.Drawing.Size(992, 250);
        _gridMyBooks.ReadOnly = true;
        _gridMyBooks.AllowUserToAddRows = false;
        _gridMyBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _gridMyBooks.Columns.Add("LoanId", "Код выдачи");
        _gridMyBooks.Columns.Add("BookTitle", "Книга");
        _gridMyBooks.Columns.Add("IssueDate", "Дата выдачи");
        _gridMyBooks.Columns.Add("DueDate", "Срок возврата");
        _gridMyBooks.Columns["LoanId"].Visible = false;

        _infoLabel.Location = new System.Drawing.Point(16, 360);
        _infoLabel.AutoSize = true;
        _infoLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);

        btnLoad.Click += (_, _) =>
        {
            if (int.TryParse(txtReaderId.Text, out int readerId))
            {
                var loans = _loanRepository.GetActiveLoansByReader(readerId);
                _gridMyBooks.Rows.Clear();
                foreach (var l in loans)
                    _gridMyBooks.Rows.Add(l.Id, l.BookTitle, l.IssueDate.ToShortDateString(), l.DueDate.ToShortDateString());
                _infoLabel.Text = $"Активных выдач: {loans.Count}";
            }
            else
            {
                _infoLabel.Text = "Введите корректный номер читательского билета.";
            }
        };

        Controls.Add(lblTitle);
        Controls.Add(lblReader);
        Controls.Add(txtReaderId);
        Controls.Add(btnLoad);
        Controls.Add(lblInfo);
        Controls.Add(_gridMyBooks);
        Controls.Add(_infoLabel);
    }
}