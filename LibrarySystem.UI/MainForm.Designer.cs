namespace LibrarySystem.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miCatalog = new System.Windows.Forms.ToolStripMenuItem();
            this.miReaders = new System.Windows.Forms.ToolStripMenuItem();
            this.miSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miLoan = new System.Windows.Forms.ToolStripMenuItem();
            this.miReturn = new System.Windows.Forms.ToolStripMenuItem();
            this.miSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlCatalog = new System.Windows.Forms.Panel();
            this.pnlReaders = new System.Windows.Forms.Panel();
            this.pnlLoan = new System.Windows.Forms.Panel();
            this.pnlReturn = new System.Windows.Forms.Panel();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.pnlProfile = new System.Windows.Forms.Panel();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCatalog,
            this.miReaders,
            this.miSeparator1,
            this.miLoan,
            this.miReturn,
            this.miSearch});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1024, 25);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // miCatalog
            // 
            this.miCatalog.Name = "miCatalog";
            this.miCatalog.Size = new System.Drawing.Size(63, 20);
            this.miCatalog.Text = "Каталог";
            // 
            // miReaders
            // 
            this.miReaders.Name = "miReaders";
            this.miReaders.Size = new System.Drawing.Size(68, 20);
            this.miReaders.Text = "Читатели";
            // 
            // miSeparator1
            // 
            this.miSeparator1.Name = "miSeparator1";
            this.miSeparator1.Size = new System.Drawing.Size(6, 20);
            // 
            // miLoan
            // 
            this.miLoan.Name = "miLoan";
            this.miLoan.Size = new System.Drawing.Size(57, 20);
            this.miLoan.Text = "Выдача";
            // 
            // miReturn
            // 
            this.miReturn.Name = "miReturn";
            this.miReturn.Size = new System.Drawing.Size(64, 20);
            this.miReturn.Text = "Возврат";
            // 
            // miSearch
            // 
            this.miSearch.Name = "miSearch";
            this.miSearch.Size = new System.Drawing.Size(48, 20);
            this.miSearch.Text = "Поиск";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblUser});
            this.statusStrip.Location = new System.Drawing.Point(0, 618);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1024, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUser
            // 
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(80, 17);
            this.lblUser.Text = "Пользователь";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(16, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(136, 19);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Visible = false;
            this.lblTitle.Visible = false;
            // 
            // pnlCatalog
            // 
            this.pnlCatalog.Location = new System.Drawing.Point(0, 60);
            this.pnlCatalog.Name = "pnlCatalog";
            this.pnlCatalog.Size = new System.Drawing.Size(900, 480);
            this.pnlCatalog.TabIndex = 5;
            this.pnlCatalog.Visible = false;
            // 
            // pnlReaders
            // 
            this.pnlReaders.Location = new System.Drawing.Point(0, 60);
            this.pnlReaders.Name = "pnlReaders";
            this.pnlReaders.Size = new System.Drawing.Size(900, 480);
            this.pnlReaders.TabIndex = 6;
            this.pnlReaders.Visible = false;
            // 
            // pnlLoan
            // 
            this.pnlLoan.Location = new System.Drawing.Point(0, 60);
            this.pnlLoan.Name = "pnlLoan";
            this.pnlLoan.Size = new System.Drawing.Size(900, 480);
            this.pnlLoan.TabIndex = 7;
            this.pnlLoan.Visible = false;
            // 
            // pnlReturn
            // 
            this.pnlReturn.Location = new System.Drawing.Point(0, 60);
            this.pnlReturn.Name = "pnlReturn";
            this.pnlReturn.Size = new System.Drawing.Size(900, 480);
            this.pnlReturn.TabIndex = 8;
            this.pnlReturn.Visible = false;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Location = new System.Drawing.Point(0, 60);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(900, 480);
            this.pnlSearch.TabIndex = 9;
            this.pnlSearch.Visible = false;
            // 
            // pnlProfile
            // 
            this.pnlProfile.Location = new System.Drawing.Point(0, 60);
            this.pnlProfile.Name = "pnlProfile";
            this.pnlProfile.Size = new System.Drawing.Size(900, 480);
            this.pnlProfile.TabIndex = 10;
            this.pnlProfile.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 560);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlProfile);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlReturn);
            this.Controls.Add(this.pnlLoan);
            this.Controls.Add(this.pnlReaders);
            this.Controls.Add(this.pnlCatalog);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ИС Библиотека";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem miCatalog;
        private System.Windows.Forms.ToolStripMenuItem miReaders;
        private System.Windows.Forms.ToolStripSeparator miSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miLoan;
        private System.Windows.Forms.ToolStripMenuItem miReturn;
        private System.Windows.Forms.ToolStripMenuItem miSearch;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblUser;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlCatalog;
        private System.Windows.Forms.Panel pnlReaders;
        private System.Windows.Forms.Panel pnlLoan;
        private System.Windows.Forms.Panel pnlReturn;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Panel pnlProfile;
    }
}
