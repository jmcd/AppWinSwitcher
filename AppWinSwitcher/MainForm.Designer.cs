namespace AppWinSwitcher
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);

        //    Cleanup();
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblShortcutText = new Label();
            btnDefineShortcut = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // lblShortcutText
            // 
            lblShortcutText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblShortcutText.AutoSize = true;
            lblShortcutText.Location = new Point(12, 9);
            lblShortcutText.Name = "lblShortcutText";
            lblShortcutText.Size = new Size(38, 15);
            lblShortcutText.TabIndex = 0;
            lblShortcutText.Text = "label1";
            // 
            // btnDefineShortcut
            // 
            btnDefineShortcut.Location = new Point(12, 27);
            btnDefineShortcut.Name = "btnDefineShortcut";
            btnDefineShortcut.Size = new Size(152, 23);
            btnDefineShortcut.TabIndex = 1;
            btnDefineShortcut.Text = "Define Shortcut";
            btnDefineShortcut.UseVisualStyleBackColor = true;
            btnDefineShortcut.Click += btnDefineShortcut_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(130, 120);
            label1.Name = "label1";
            label1.RightToLeft = RightToLeft.Yes;
            label1.Size = new Size(219, 15);
            label1.TabIndex = 2;
            label1.Text = "This window will minimize to the systray";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(361, 144);
            Controls.Add(label1);
            Controls.Add(btnDefineShortcut);
            Controls.Add(lblShortcutText);
            Margin = new Padding(2);
            Name = "MainForm";
            Text = "AppWinSwitcher";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblShortcutText;
        private Button btnDefineShortcut;
        private Label label1;
    }
}
