using LibrarySystem.Logic.Navigation;
using LibrarySystem.Logic.Views;

namespace LibrarySystem.UI;

/// <summary>
/// Главное окно приложения. Содержит меню сверху и панели разделов снизу.
/// Все модули работают внутри одного окна, переключаются через меню.
/// </summary>
public partial class MainForm : Form, IMainView
{
    private UserRole _userRole = UserRole.None;
    private readonly Panel _activeIndicator;

    public event EventHandler? ViewLoaded;
    public event EventHandler<ScreenId>? ScreenRequested;

    public MainForm()
    {
        InitializeComponent();

        // Индикатор активного раздела (полоска под меню)
        _activeIndicator = new Panel
        {
            BackColor = SystemColors.Highlight,
            Height = 3,
            Dock = DockStyle.None,
            Location = new System.Drawing.Point(0, menuStrip.Height),
            Size = new System.Drawing.Size(100, 3),
            Visible = false,
        };
        menuStrip.Controls.Add(_activeIndicator);

        // Подписки на клики по меню
        miCatalog.Click += (_, _) => NavigateTo(ScreenId.Catalog);
        miReaders.Click += (_, _) => NavigateTo(ScreenId.Readers);
        miLoan.Click += (_, _) => NavigateTo(ScreenId.Loan);
        miReturn.Click += (_, _) => NavigateTo(ScreenId.Return);
        miSearch.Click += (_, _) => NavigateTo(ScreenId.Search);
    }

    public string UserName
    {
        set => lblUser.Text = value;
    }

    public string StatusText
    {
        set => lblStatus.Text = value;
    }

    public void ShowMessage(string message)
    {
        MessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void SetUserRole(UserRole role)
    {
        _userRole = role;
        bool isLibrarian = role == UserRole.Librarian;

        // Меню: читатель видит только каталог, поиск и профиль
        miCatalog.Visible = true;
        miReaders.Visible = isLibrarian;
        miLoan.Visible = isLibrarian;
        miReturn.Visible = isLibrarian;
        miSearch.Visible = true;

        // Активный раздел по умолчанию — Каталог
        NavigateTo(ScreenId.Catalog);
    }

    public void NavigateTo(ScreenId screen)
    {
        // Скролл в начало контента панели если нужно
        bool isLibrarian = _userRole == UserRole.Librarian;

        // Сброс подсветки
        ResetMenuHighlight();
        _activeIndicator.Visible = false;

        // Скрываем все pane-панели (они создаются в InitializeComponent)
        // Найти по имени
        foreach (Control c in this.Controls)
        {
            if (c is Panel p && p.Name.StartsWith("pnl"))
                p.Visible = false;
        }

        // Показываем нужную
        Panel? target = screen switch
        {
            ScreenId.Catalog => pnlCatalog,
            ScreenId.Readers => pnlReaders,
            ScreenId.Loan => pnlLoan,
            ScreenId.Return => pnlReturn,
            ScreenId.Search => pnlSearch,
            ScreenId.Profile => pnlProfile,
            _ => pnlCatalog
        };

        if (target != null)
        {
            target.Visible = true;
            target.BringToFront();
            _activeIndicator.Visible = true;

            // Подсветка меню
            ToolStripMenuItem? activeItem = screen switch
            {
                ScreenId.Catalog => miCatalog,
                ScreenId.Readers => miReaders,
                ScreenId.Loan => miLoan,
                ScreenId.Return => miReturn,
                ScreenId.Search => miSearch,
                _ => miCatalog
            };

            if (activeItem != null)
            {
                activeItem.BackColor = SystemColors.Highlight;
                activeItem.ForeColor = Color.White;
                _activeIndicator.Left = activeItem.Left;
                _activeIndicator.Width = activeItem.Width;
            }

            lblStatus.Text = $"Раздел: {ScreenTitles.Get(screen)}";
        }
    }

    private void ResetMenuHighlight()
    {
        _miCatalog.BackColor = SystemColors.Control;
        _miCatalog.ForeColor = SystemColors.ControlText;
        _miReaders.BackColor = SystemColors.Control;
        _miReaders.ForeColor = SystemColors.ControlText;
        _miLoan.BackColor = SystemColors.Control;
        _miLoan.ForeColor = SystemColors.ControlText;
        _miReturn.BackColor = SystemColors.Control;
        _miReturn.ForeColor = SystemColors.ControlText;
        _miSearch.BackColor = SystemColors.Control;
        _miSearch.ForeColor = SystemColors.ControlText;
    }

    // Доступ к элементам меню для проверки роли
    private ToolStripMenuItem _miCatalog => miCatalog;
    private ToolStripMenuItem _miReaders => miReaders;
    private ToolStripMenuItem _miLoan => miLoan;
    private ToolStripMenuItem _miReturn => miReturn;
    private ToolStripMenuItem _miSearch => miSearch;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ViewLoaded?.Invoke(this, EventArgs.Empty);
    }
}
