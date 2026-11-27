using LibrarySystem.Core.Models;
using LibrarySystem.Data.Repositories;
using System.Windows.Forms;

namespace LibrarySystem.UI.Screens.Panels;

/// <summary>
/// Панель читателей: список читателей, поиск, добавление/удаление.
/// </summary>
public sealed partial class ReadersPanel : Panel
{
    private readonly IReaderRepository _readerRepository;
    private readonly ILoanRepository _loanRepository;
    private readonly DataGridView _grid = new();
    private readonly Label _infoLabel = new();
    private readonly TextBox _searchBox = new();
    private readonly Button _btnSearch = new();
    private readonly Button _btnShowAll = new();
    private readonly Button _btnRefresh = new();
    private readonly Button _btnAddReader = new();
    private readonly Button _btnDeleteReader = new();

    public ReadersPanel(IReaderRepository readerRepository, ILoanRepository loanRepository)
    {
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
            ColumnCount = 4,
            RowCount = 3,
            Padding = new Padding(16),
            AutoSize = false,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));

        var lblSearch = new Label { Text = "Поиск (билет/ФИО):", Anchor = AnchorStyles.Left, AutoSize = true };
        _searchBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _searchBox.Size = new Size(200, 23);
        _btnSearch.Text = "Найти";
        _btnSearch.Size = new Size(80, 30);
        _btnSearch.Anchor = AnchorStyles.Left;
        _btnShowAll.Text = "Показать всех";
        _btnShowAll.Size = new Size(120, 30);
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
        _grid.Columns.Add("Id", "Код билета");
        _grid.Columns.Add("FullName", "ФИО");
        _grid.Columns.Add("Phone", "Телефон");
        _grid.Columns.Add("Address", "Адрес");
        _grid.Columns["Id"].Visible = false;
        layout.Controls.Add(_grid, 0, 1);
        layout.SetColumnSpan(_grid, 4);

        var btnPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(0, 8, 0, 0) };
        _btnAddReader.Text = "Зарегистрировать";
        _btnAddReader.Size = new Size(150, 30);
        _btnDeleteReader.Text = "Удалить";
        _btnDeleteReader.Size = new Size(90, 30);
        _btnRefresh.Text = "Обновить";
        _btnRefresh.Size = new Size(90, 30);
        btnPanel.Controls.Add(_btnAddReader);
        btnPanel.Controls.Add(_btnDeleteReader);
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
        layout.SetColumnSpan(bottomPanel, 4);

        Controls.Add(layout);

        _btnShowAll.Click += (_, _) => LoadReaders(string.Empty);
        _btnSearch.Click += (_, _) => LoadReaders(_searchBox.Text);
        _searchBox.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadReaders(_searchBox.Text); };
        _btnRefresh.Click += (_, _) => LoadReaders(string.Empty);
        _btnAddReader.Click += (_, _) => AddReader();
        _btnDeleteReader.Click += (_, _) => DeleteReader();

        LoadReaders(string.Empty);
    }

    private void LoadReaders(string filter)
    {
        try
        {
            var readers = string.IsNullOrWhiteSpace(filter)
                ? _readerRepository.GetAll()
                : _readerRepository.Search(filter);

            _grid.Rows.Clear();
            foreach (var r in readers)
            {
                _grid.Rows.Add(r.Id, r.FullName, r.Phone, r.Address);
            }

            _infoLabel.Text = $"Найдено читателей: {readers.Count}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Ошибка загрузки читателей: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AddReader()
    {
        using var dlg = new Form
        {
            Text = "Регистрация читателя",
            Size = new Size(400, 250),
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false,
        };

        var lblLastName = new Label { Text = "Фамилия:", Location = new Point(20, 20), AutoSize = true };
        var txtLastName = new TextBox { Location = new Point(120, 16), Size = new Size(250, 23) };
        var lblFirstName = new Label { Text = "Имя:", Location = new Point(20, 50), AutoSize = true };
        var txtFirstName = new TextBox { Location = new Point(120, 46), Size = new Size(250, 23) };
        var lblPhone = new Label { Text = "Телефон:", Location = new Point(20, 80), AutoSize = true };
        var txtPhone = new TextBox { Location = new Point(120, 76), Size = new Size(250, 23) };
        var lblAddress = new Label { Text = "Адрес:", Location = new Point(20, 110), AutoSize = true };
        var txtAddress = new TextBox { Location = new Point(120, 106), Size = new Size(250, 23) };
        var btnOk = new Button { Text = "Зарегистрировать", Location = new Point(120, 160), Size = new Size(140, 30), DialogResult = DialogResult.OK };
        var btnCancel = new Button { Text = "Отмена", Location = new Point(270, 160), Size = new Size(100, 30), DialogResult = DialogResult.Cancel };

        dlg.Controls.AddRange(new Control[] { lblLastName, txtLastName, lblFirstName, txtFirstName, lblPhone, txtPhone, lblAddress, txtAddress, btnOk, btnCancel });
        dlg.AcceptButton = btnOk;
        dlg.CancelButton = btnCancel;

        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            try
            {
                var reader = new Reader
                {
                    LastName = txtLastName.Text.Trim(),
                    FirstName = txtFirstName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim()
                };
                _readerRepository.Add(reader);
                LoadReaders(string.Empty);
                _infoLabel.Text = $"Читатель зарегистрирован (билет №{reader.Id}).";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void DeleteReader()
    {
        if (_grid.CurrentRow == null)
        {
            _infoLabel.Text = "Выберите читателя в таблице.";
            return;
        }

        int readerId = (int)_grid.CurrentRow.Cells[0].Value;

        // Проверяем, есть ли невозвращённые книги
        var activeLoans = _loanRepository.GetActiveLoansByReader(readerId);
        if (activeLoans.Count > 0)
        {
            var bookList = string.Join("\n", activeLoans.Select(l => $"• {l.BookTitle} (до {l.DueDate:dd.MM.yyyy})"));
            MessageBox.Show(this,
                $"Нельзя удалить читателя с невозвращёнными книгами:\n\n{bookList}",
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            _infoLabel.Text = "Удаление отменено: есть невозвращённые книги.";
            return;
        }

        var result = MessageBox.Show(this,
            $"Удалить читателя «{_grid.CurrentRow.Cells[1].Value}»?",
            "Подтверждение",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            try
            {
                _readerRepository.Delete(readerId);
                LoadReaders(string.Empty);
                _infoLabel.Text = "Читатель удалён.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}