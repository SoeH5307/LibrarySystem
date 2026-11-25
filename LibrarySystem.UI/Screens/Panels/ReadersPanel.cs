using LibrarySystem.Core.Models;

namespace LibrarySystem.UI.Screens.Panels;

/// <summary>
/// Панель читателей: список читателей и возможности для библиотекаря.
/// </summary>
public sealed partial class ReadersPanel : Panel
{
    private readonly IReaderRepository _readerRepository;
    private readonly DataGridView _grid = new();
    private readonly Label _infoLabel = new();

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
        var searchLabel = new Label { Text = "Поиск читателя (билет/ФИО):", Location = new System.Drawing.Point(16, 12), AutoSize = true };
        var searchBox = new TextBox { Location = new System.Drawing.Point(150, 8), Size = new System.Drawing.Size(250, 23) };
        var btnSearch = new Button { Text = "Найти", Location = new System.Drawing.Point(400, 8), Size = new System.Drawing.Size(80, 23) };
        var btnShowAll = new Button { Text = "Показать всех", Location = new System.Drawing.Point(490, 8), Size = new System.Drawing.Size(90, 23) };

        _grid.Location = new System.Drawing.Point(16, 50);
        _grid.Size = new System.Drawing.Size(992, 300);
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.Columns.Add("Id", "Код билета");
        _grid.Columns.Add("Name", "ФИО");
        _grid.Columns.Add("Phone", "Телефон");
        _grid.Columns.Add("Address", "Адрес");
        _grid.Columns["Id"].Visible = false;

        _infoLabel.Location = new System.Drawing.Point(16, 365);
        _infoLabel.AutoSize = true;
        _infoLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);

        var btnRefresh = new Button { Text = "Обновить", Location = new System.Drawing.Point(16, 410), Size = new System.Drawing.Size(90, 30) };

        btnShowAll.Click += (_, _) => LoadReaders(string.Empty);
        btnSearch.Click += (_, _) => LoadReaders(searchBox.Text);
        searchBox.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadReaders(searchBox.Text); };
        btnRefresh.Click += (_, _) => LoadReaders(string.Empty);

        Controls.Add(searchLabel);
        Controls.Add(searchBox);
        Controls.Add(btnSearch);
        Controls.Add(btnShowAll);
        Controls.Add(_grid);
        Controls.Add(_infoLabel);
        Controls.Add(btnRefresh);

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