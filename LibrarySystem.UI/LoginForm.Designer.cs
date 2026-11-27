namespace LibrarySystem.UI
{
    partial class LoginForm
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
            this.btnLibrarian = new System.Windows.Forms.Button();
            this.btnReader = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(90, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(220, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Добро пожаловать";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(60, 80);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 15);
            this.lblError.TabIndex = 1;
            // 
            // btnLibrarian
            // 
            this.btnLibrarian.Location = new System.Drawing.Point(60, 120);
            this.btnLibrarian.Name = "btnLibrarian";
            this.btnLibrarian.Size = new System.Drawing.Size(260, 45);
            this.btnLibrarian.TabIndex = 2;
            this.btnLibrarian.Text = "Войти как библиотекарь";
            this.btnLibrarian.UseVisualStyleBackColor = true;
            // 
            // btnReader
            // 
            this.btnReader.Location = new System.Drawing.Point(60, 180);
            this.btnReader.Name = "btnReader";
            this.btnReader.Size = new System.Drawing.Size(260, 45);
            this.btnReader.TabIndex = 3;
            this.btnReader.Text = "Войти как читатель";
            this.btnReader.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 260);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnLibrarian);
            this.Controls.Add(this.btnReader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ИС Библиотека — Вход";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnLibrarian;
        private System.Windows.Forms.Button btnReader;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblTitle;
    }
}