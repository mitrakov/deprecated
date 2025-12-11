using System.ComponentModel;
using System.Windows.Forms.Design;

namespace LasNotes;

[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
public partial class ToolStripCheckBox : ToolStripControlHost {
    public ToolStripCheckBox() : base(new CheckBox()) {
        BackColor = Color.Transparent;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsChecked {
        get {
            if (Control is CheckBox box)
                return box.Checked;
            return false;
        }
        set {
            if (Control is CheckBox box)
                box.Checked = value;
        } 
    }
}
