namespace LibrarySystem.UI.Screens;

/// <summary>
/// Временная форма-заглушка для разделов, которые ещё не реализованы.
/// </summary>
public sealed class PlaceholderForm : Form
{
    public PlaceholderForm(string title)
    {
        Text = $"ИС Библиотека — {title}";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(640, 360);
        Font = new Font("Segoe UI", 10F);

        var header = new Label
        {
            Text = title,
            AutoSize = true,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Location = new Point(16, 16)
        };

        var note = new Label
        {
            Text = "Этот раздел находится в разработке.",
            AutoSize = true,
            Location = new Point(16, 56)
        };

        Controls.Add(header);
        Controls.Add(note);
    }
}
