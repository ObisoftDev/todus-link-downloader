namespace TDM.Dialogs
{
    partial class AuthDialog
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.darkTextBox1 = new DarkUI.Controls.DarkTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ReloadAuth = new DarkUI.Controls.DarkButton();
            this.darkTextBox2 = new DarkUI.Controls.DarkTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.InfoBtn = new DarkUI.Controls.DarkButton();
            this.label3 = new System.Windows.Forms.Label();
            this.InfoTxt = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPakete = new DarkUI.Controls.DarkTextBox();
            this.SuspendLayout();
            // 
            // darkTextBox1
            // 
            this.darkTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.darkTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.darkTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkTextBox1.Location = new System.Drawing.Point(12, 36);
            this.darkTextBox1.Name = "darkTextBox1";
            this.darkTextBox1.Size = new System.Drawing.Size(342, 20);
            this.darkTextBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Autorizacion";
            // 
            // ReloadAuth
            // 
            this.ReloadAuth.Location = new System.Drawing.Point(12, 62);
            this.ReloadAuth.Name = "ReloadAuth";
            this.ReloadAuth.Padding = new System.Windows.Forms.Padding(5);
            this.ReloadAuth.Size = new System.Drawing.Size(75, 23);
            this.ReloadAuth.TabIndex = 4;
            this.ReloadAuth.Text = "Recargar";
            this.ReloadAuth.Click += new System.EventHandler(this.ReloadAuth_Click);
            // 
            // darkTextBox2
            // 
            this.darkTextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.darkTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.darkTextBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkTextBox2.Location = new System.Drawing.Point(300, 67);
            this.darkTextBox2.Name = "darkTextBox2";
            this.darkTextBox2.Size = new System.Drawing.Size(55, 20);
            this.darkTextBox2.TabIndex = 5;
            this.darkTextBox2.Text = "54858181";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(245, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Telefono";
            // 
            // InfoBtn
            // 
            this.InfoBtn.ButtonStyle = DarkUI.Controls.DarkButtonStyle.Flat;
            this.InfoBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.InfoBtn.Image = global::TDM.Properties.Resources.ScrollBarArrowDown;
            this.InfoBtn.ImagePadding = 0;
            this.InfoBtn.Location = new System.Drawing.Point(39, 87);
            this.InfoBtn.Name = "InfoBtn";
            this.InfoBtn.Padding = new System.Windows.Forms.Padding(5);
            this.InfoBtn.Size = new System.Drawing.Size(23, 23);
            this.InfoBtn.TabIndex = 7;
            this.InfoBtn.Click += new System.EventHandler(this.darkButton2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(12, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Info";
            // 
            // InfoTxt
            // 
            this.InfoTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.InfoTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.InfoTxt.Font = new System.Drawing.Font("Myanmar Text", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoTxt.ForeColor = System.Drawing.Color.SpringGreen;
            this.InfoTxt.Location = new System.Drawing.Point(14, 116);
            this.InfoTxt.Name = "InfoTxt";
            this.InfoTxt.Size = new System.Drawing.Size(341, 105);
            this.InfoTxt.TabIndex = 9;
            this.InfoTxt.Text = "Aca les dejo un tuto:\n1-Conecte su movil al pc atraves del usb\n2-Permita el acces" +
    "o a datos \n3-Presionar el Boton de Recargar\n!Enjoy se a recargado su Autorizacio" +
    "n";
            this.InfoTxt.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(92, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Pakete";
            // 
            // txtPakete
            // 
            this.txtPakete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtPakete.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPakete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtPakete.Location = new System.Drawing.Point(134, 65);
            this.txtPakete.Name = "txtPakete";
            this.txtPakete.Size = new System.Drawing.Size(105, 20);
            this.txtPakete.TabIndex = 10;
            this.txtPakete.Text = "cu.todus.android";
            // 
            // AuthDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 159);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPakete);
            this.Controls.Add(this.InfoTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.InfoBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.darkTextBox2);
            this.Controls.Add(this.ReloadAuth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.darkTextBox1);
            this.MinimumSize = new System.Drawing.Size(384, 175);
            this.Name = "AuthDialog";
            this.Text = "Autorizacion";
            this.Load += new System.EventHandler(this.AuthDialog_Load);
            this.Controls.SetChildIndex(this.darkTextBox1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.ReloadAuth, 0);
            this.Controls.SetChildIndex(this.darkTextBox2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.InfoBtn, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.InfoTxt, 0);
            this.Controls.SetChildIndex(this.txtPakete, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DarkUI.Controls.DarkTextBox darkTextBox1;
        private System.Windows.Forms.Label label1;
        private DarkUI.Controls.DarkButton ReloadAuth;
        private DarkUI.Controls.DarkTextBox darkTextBox2;
        private System.Windows.Forms.Label label2;
        private DarkUI.Controls.DarkButton InfoBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox InfoTxt;
        private System.Windows.Forms.Label label4;
        private DarkUI.Controls.DarkTextBox txtPakete;
    }
}
