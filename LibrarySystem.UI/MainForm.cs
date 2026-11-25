using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using LibrarySystem.Logic.Navigation;
using LibrarySystem.Logic.Views;
using LibrarySystem.UI.Screens;

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
        this.Controls.Add(_activeIndicator);
        _activeIndicator.BringToFront();

        // Подписки на клики по меню
        miCatalog.Click += (_, _) => NavigateTo(ScreenId.Catalog);
        miReaders.Click += (_, _) => NavigateTo(ScreenId.Readers);
        miLoan.Click += (_, _) => NavigateTo(ScreenId.Loan);
        miReturn.Click += (_, _) => NavigateTo(ScreenId.Return);
        miSearch.Click += (_, _) => NavigateTo(ScreenId.Search);

        // Загрузка панелей (UserControl'ы) в контейнеры
        LoadPanelContent(pnlCatalog, ScreenId.Catalog);
        LoadPanelContent(pnlReaders, ScreenId.Readers);
        LoadPanelContent(pnlLoan, ScreenId.Loan);
        LoadPanelContent(pnlReturn, ScreenId.Return);
        LoadPanelContent(pnlSearch, ScreenId.Search);
        LoadPanelContent(pnlProfile, ScreenId.Profile);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string UserName
    {
        set => lblUser.Text = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        // Сброс подсветки
        ResetMenuHighlight();
        _activeIndicator.Visible = false;

        // Скрываем все pane-панели
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
            ToolStripItem? activeItem = screen switch
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
                
                // Индикатор теперь в Controls формы, нужно преобразовать координаты
                if (activeItem.Owner != null)
                {
                    Point itemScreenPoint = activeItem.Owner.PointToScreen(activeItem.Bounds.Location);
                    Point itemClientPoint = this.PointToClient(itemScreenPoint);
                    _activeIndicator.Left = itemClientPoint.X;
                    _activeIndicator.Top = menuStrip.Bottom;
                    _activeIndicator.Width = activeItem.Bounds.Width;
                }
                else
                {
                    _activeIndicator.Left = menuStrip.Left + activeItem.Bounds.Left;
                    _activeIndicator.Top = menuStrip.Bottom;
                    _activeIndicator.Width = activeItem.Bounds.Width;
                }
            }

            lblStatus.Text = $"Раздел: {ScreenTitles.Get(screen)}";
        }
    }

    private void LoadPanelContent(Panel container, ScreenId screen)
    {
        var factory = new ScreenFactory();
        var content = factory.Create(screen);
        content.Dock = DockStyle.Fill;
        container.Controls.Add(content);
    }

    private void ResetMenuHighlight()
    {
        foreach (ToolStripItem item in menuStrip.Items)
        {
            item.BackColor = SystemColors.Control;
            item.ForeColor = SystemColors.ControlText;
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ViewLoaded?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        // Обновляем позицию индикатора при изменении размера формы
        if (_activeIndicator != null && _activeIndicator.Visible)
        {
            UpdateIndicatorPosition();
        }
    }

    private void UpdateIndicatorPosition()
    {
        // Определяем текущий активный экран по видимой панели
        ScreenId current = ScreenId.Catalog;
        foreach (Control c in this.Controls)
        {
            if (c is Panel p && p.Name.StartsWith("pnl") && p.Visible)
            {
                current = p.Name switch
                {
                    "pnlCatalog" => ScreenId.Catalog,
                    "pnlReaders" => ScreenId.Readers,
                    "pnlLoan" => ScreenId.Loan,
                    "pnlReturn" => ScreenId.Return,
                    "pnlSearch" => ScreenId.Search,
                    "pnlProfile" => ScreenId.Profile,
                    _ => ScreenId.Catalog
                };
                break;
            }
        }

        ToolStripItem? activeItem = current switch
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
            if (activeItem.Owner != null)
            {
                Point itemScreenPoint = activeItem.Owner.PointToScreen(activeItem.Bounds.Location);
                Point itemClientPoint = this.PointToClient(itemScreenPoint);
                _activeIndicator.Left = itemClientPoint.X;
                _activeIndicator.Top = menuStrip.Bottom;
                _activeIndicator.Width = activeItem.Bounds.Width;
            }
            else
            {
                _activeIndicator.Left = menuStrip.Left + activeItem.Bounds.Left;
                _activeIndicator.Top = menuStrip.Bottom;
                _activeIndicator.Width = activeItem.Bounds.Width;
            }
        }
    }
}