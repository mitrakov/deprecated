using System.ComponentModel;

namespace LasNotes {
    partial class CardWidget {
        private IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent() {
            resources = new ComponentResourceManager(typeof(CardWidget));
            image = new PictureBox();
            header = new Label();
            text = new Label();
            rectangle = new Panel();
            ((ISupportInitialize)image).BeginInit();
            SuspendLayout();
            // 
            // image
            // 
            image.Dock = DockStyle.Left;
            image.InitialImage = null;
            image.Location = new Point(4, 4);
            image.Name = "image";
            image.Size = new Size(64, 142);
            image.SizeMode = PictureBoxSizeMode.CenterImage;
            image.TabStop = false;
            // 
            // header
            // 
            header.AutoSize = true;
            header.Font = new Font("Consolas", 14F, FontStyle.Bold);
            header.Location = new Point(80, 10);
            header.Name = "header";
            header.Size = new Size(90, 28);
            header.Text = "Header for Designer";
            // 
            // text
            // 
            text.AutoSize = true;
            text.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            text.Location = new Point(80, 45);
            text.Margin = new Padding(3, 0, 3, 8);
            text.Name = "text";
            text.Size = new Size(33, 20);
            text.Text = "text for Designer";
            // 
            // rectangle
            // 
            rectangle.Location = new Point(0, 0);
            rectangle.Name = "rectangle";
            rectangle.Size = new Size(540, 0);
            // 
            // CardWidget
            // 
            AutoSize = true;
            BackColor = Color.AliceBlue;
            Controls.Add(text);
            Controls.Add(header);
            Controls.Add(image);
            Controls.Add(rectangle);
            Margin = new Padding(20, 8, 4, 6);
            Name = "CardWidget";
            Padding = new Padding(4);
            Size = new Size(1000, 150);
            Click += OnClick;
            Paint += OnPaint;
            MouseLeave += OnMouseLeave;
            MouseMove += OnMouseMove;
            ((ISupportInitialize)image).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComponentResourceManager resources;
        private PictureBox image;
        private Label header;
        private Label text;
        private Panel rectangle;
    }
}
