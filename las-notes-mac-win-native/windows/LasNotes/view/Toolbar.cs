using System.Diagnostics;

namespace LasNotes;

public partial class Toolbar : UserControl {
    private readonly TextBoxBase txtBox;

    public Toolbar(TextBoxBase txtBox) {
        InitializeComponent();
        this.txtBox = txtBox;
        SetLike(false);
    }

    internal void SetLike(bool like) {
        btnDonate.Image = Utils.BytesToImage(resources.GetObject(like ? "heart2" : "heart1") as byte[] ?? []);
    }

    private void BtnH1_Click(object sender, EventArgs e) => Prepend("#", "Header");

    private void BtnH2_Click(object sender, EventArgs e) => Prepend("##", "Header");

    private void BtnH3_Click(object sender, EventArgs e) => Prepend("###", "Header");

    private void BtnBold_Click(object sender, EventArgs e) => Surround("**", "text", "**");

    private void BtnItalic_Click(object sender, EventArgs e) => Surround("*", "text", "*");

    private void BtnUnderline_Click(object sender, EventArgs e) => Surround("__", "text", "__");

    private void BtnStrike_Click(object sender, EventArgs e) => Surround("~~", "text", "~~");

    private void BtnTicks_Click(object sender, EventArgs e) => Surround("`", "text", "`");

    private void BtnBulletList_Click(object sender, EventArgs e) => Prepend("*", "item");

    private void BtnNumList_Click(object sender, EventArgs e) => Prepend("1.", "item");

    private void BtnQuote_Click(object sender, EventArgs e) => Prepend(">", "quote");

    private void BtnLink_Click(object sender, EventArgs e) => Surround("[", "Link", "](https://lasnotes.com) ");

    private void BtnCode_Click(object sender, EventArgs e) => Surround("```\n", "Code", "\n```\n", forceNewline: true);

    private void BtnRule_Click(object sender, EventArgs e) => Append("\n---");

    private void BtnTable_Click(object sender, EventArgs e) => Append("|  |  |  |\n|:-|--|-:|\n|  |  |  |\n|  |  |  |");

    private void BtnImage_Click(object sender, EventArgs e) => Append("![](https://lasnotes.com/picture/)");

    private void BtnMore_Click(object sender, EventArgs e) {
        Process.Start("explorer", "https://www.markdownguide.org/cheat-sheet/");
        txtBox.Focus();
    }

    private void BtnDonate_Click(object sender, EventArgs e) {
        Process.Start("explorer", "https://lasnotes.com/donate");
        txtBox.Focus();
    }

    private void Prepend(string str, string placeholder) {
        var txt = txtBox.Text;
        var idx = txtBox.SelectionStart;
        var s = EmptyLine(txt, idx) ? $"{str} {placeholder}\n" : $"{str} ";

        while (idx > 0 && txt.ElementAtOrDefault(idx - 1) != '\n') idx--; // move caret left

        txtBox.Text = txt.Insert(idx, s);
        txtBox.SelectionStart = idx + s.Length;
        txtBox.Focus();
    }

    private void Surround(string str1, string placeholder, string str2, bool forceNewline = false) {
        var txt = txtBox.Text;
        var idx = txtBox.SelectionStart;
        var len = txtBox.SelectionLength;

        while (len > 0 && char.IsWhiteSpace(txt.ElementAtOrDefault(idx + len - 1))) len--; // remove spaces from selection

        if (len == 0) { // no selection => add placeholder
            while (forceNewline && idx < txt.Length && txt.ElementAtOrDefault(idx) != '\n') idx++; // move caret right

            var s = forceNewline && !EmptyLine(txt, idx) ? $"\n{str1}{placeholder}{str2}" : $"{str1}{placeholder}{str2}";
            txtBox.Text = txt.Insert(idx, s);
            idx += s.Length;
        } else {
            var s = forceNewline && !FullLineSelected(txt, idx, len) ? $"\n{str1}" : str1;
            txtBox.Text = txt.Insert(idx, s).Insert(idx + s.Length + len, str2);
            idx += s.Length + len + str2.Length;
        }

        txtBox.SelectionStart = idx;
        txtBox.Focus();
    }

    private void Append(string str) {
        var txt = txtBox.Text;
        var idx = txtBox.SelectionStart;

        while (idx < txt.Length && txt.ElementAtOrDefault(idx) != '\n') idx++; // move caret right
        var s = EmptyLine(txt, idx) ? $"{str}\n" : $"\n{str}\n";

        txtBox.Text = txt.Insert(idx, s);
        txtBox.SelectionStart = idx + s.Length;
        txtBox.Focus();
    }

    private static bool EmptyLine(string txt, int idx) =>
        (idx == txt.Length || txt.ElementAtOrDefault(idx) == '\n') &&
        (idx == 0          || txt.ElementAtOrDefault(idx - 1) == '\n');

    private static bool FullLineSelected(string txt, int idx, int selLength) =>
        selLength > 0 &&
        (idx == 0                      || txt.ElementAtOrDefault(idx - 1) == '\n') &&
        (idx + selLength == txt.Length || txt.ElementAtOrDefault(idx + selLength) == '\n');
}
