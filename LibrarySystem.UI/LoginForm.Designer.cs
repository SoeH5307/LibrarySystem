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
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPasswordHint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(40, 22);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(220, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Добро пожаловать";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(40, 62);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(200, 15);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Пароль (для входа библиотекаря):";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(40, 84);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(300, 23);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPasswordHint
            // 
            this.lblPasswordHint.AutoSize = true;
            this.lblPasswordHint.ForeColor = System.Drawing.Color.Gray;
            this.lblPasswordHint.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic);
            this.lblPasswordHint.Location = new System.Drawing.Point(40, 112);
            this.lblPasswordHint.Name = "lblPasswordHint";
            this.lblPasswordHint.Size = new System.Drawing.Size(300, 13);
            this.lblPasswordHint.TabIndex = 3;
            this.lblPasswordHint.Text = "Читателю пароль не нужен — просто нажмите «Войти как читатель».";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(40, 138);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 15);
            this.lblError.TabIndex = 4;
            // 
            // btnLibrarian
            // 
            this.btnLibrarian.Location = new System.Drawing.Point(40, 170);
            this.btnLibrarian.Name = "btnLibrarian";
            this.btnLibrarian.Size = new System.Drawing.Size(300, 40);
            this.btnLibrarian.TabIndex = 5;
            this.btnLibrarian.Text = "Войти как библиотекарь";
            this.btnLibrarian.UseVisualStyleBackColor = true;
            // 
            // btnReader
            // 
            this.btnReader.Location = new System.Drawing.Point(40, 220);
            this.btnReader.Name = "btnReader";
            this.btnReader.Size = new System.Drawing.Size(300, 40);
            this.btnReader.TabIndex = 6;
            this.btnReader.Text = "Войти как читатель";
            this.btnReader.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnLibrarian;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 291);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPasswordHint);
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
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPasswordHint;
    }
}