namespace LasNotes;

internal class InputBox {
    public static string? Show(string? title, string? message, string? value = null, string? placeholder = null) {
        var table = new TableLayoutPanel() {
            RowCount = 3,
            ColumnCount = 1,
            AutoSize = true,
            Dock = DockStyle.Fill,
            BackColor = Color.White,
        };

        var buttons = new TableLayoutPanel() {
            RowCount = 1,
            ColumnCount = 3,
            AutoSize = true,
            Dock = DockStyle.Fill,
            BackColor = SystemColors.Control,
            Margin = new(0),
        };

        var msgLabel = new Label {
            AutoSize = true,
            Text = message,
            Margin = new(20, 16, 20, 16)
        };

        var txtBox = new TextBox {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Text = value,
            PlaceholderText = placeholder,
            Margin = new(20, 3, 20, 10),
        };

        var btnOk = new Button {
            Anchor = AnchorStyles.None,
            AutoSize = true,
            MinimumSize = new(100, 32),
            Text = "OK",
            DialogResult = DialogResult.OK,
            Margin = new(20, 16, 6, 16),
        };

        var btnCancel = new Button {
            AutoSize = true,
            MinimumSize = new(100, 32),
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Margin = new(6, 16, 20, 16),
        };

        var dialog = new Form {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MinimizeBox = false,
            MaximizeBox = false,
            Text = title,
            StartPosition = FormStartPosition.CenterScreen,
            AcceptButton = btnOk,
            CancelButton = btnCancel,
        };

        buttons.Controls.AddRange(new Label(), btnOk, btnCancel);
        table.Controls.AddRange(msgLabel, txtBox, buttons);
        dialog.Controls.Add(table);
        txtBox.SelectAll();

        if (dialog.ShowDialog() == DialogResult.OK)
            return txtBox.Text?.Trim();
        return null;
    }
}
