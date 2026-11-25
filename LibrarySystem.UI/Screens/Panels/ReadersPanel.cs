using LibrarySystem.Core.Models;
using LibrarySystem.Data.Repositories;
using System.Windows.Forms;

namespace LibrarySystem.UI.Screens.Panels;

/// <summary>
/// Панель читателей: список читателей и возможности для библиотекаря.
/// </summary>
public sealed partial class ReadersPanel : Panel
{
    private readonly IReaderRepository _readerRepository;
    private readonly DataGridView _grid = new();
    private readonly Label _infoLabel = new();
    private readonly TextBox _searchBox = new();
    private readonly Button _btnSearch = new();
    private readonly Button _btnShowAll = new();
    private readonly Button _btnRefresh = new();

    public ReadersPanel() : this(new ReaderRepository())
    {
    }

    public ReadersPanel(IReaderRepository readerRepository)
    {
        _readerRepository = readerRepository;
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
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

        var lblSearch = new Label { Text = "Поиск (билет/ФИО):", Anchor = AnchorStyles.Left, AutoSize = true };
        _searchBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _searchBox.Size = new Size(200, 23);
        _btnSearch.Text = "Найти";
        _btnSearch.Size = new Size(80, 23);
        _btnSearch.Anchor = AnchorStyles.Left;
        _btnShowAll.Text = "Показать всех";
        _btnShowAll.Size = new Size(100, 23);
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

        var bottomLayout = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(0, 8, 0, 0) };
        _infoLabel.AutoSize = true;
        _infoLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        _infoLabel.Anchor = AnchorStyles.Left;
        _infoLabel.Margin = new Padding(0, 6, 0, 0);
        _btnRefresh.Text = "Обновить";
        _btnRefresh.Size = new Size(90, 30);
        bottomLayout.Controls.Add(_infoLabel);
        bottomLayout.Controls.Add(_btnRefresh);
        layout.Controls.Add(bottomLayout, 0, 2);
        layout.SetColumnSpan(bottomLayout, 4);

        Controls.Add(layout);

        _btnShowAll.Click += (_, _) => LoadReaders(string.Empty);
        _btnSearch.Click += (_, _) => LoadReaders(_searchBox.Text);
        _searchBox.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadReaders(_searchBox.Text); };
        _btnRefresh.Click += (_, _) => LoadReaders(string.Empty);

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
}