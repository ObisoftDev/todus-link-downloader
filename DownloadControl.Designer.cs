namespace TDM
{
    partial class DownloadControl
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
            try
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
            catch { }
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.fileName = new System.Windows.Forms.Label();
            this.fileSize = new System.Windows.Forms.Label();
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.bunifuElipse2 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.Imagen = new System.Windows.Forms.PictureBox();
            this.Progres = new System.Windows.Forms.ProgressBar();
            this.timespan = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Imagen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // fileName
            // 
            this.fileName.AutoSize = true;
            this.fileName.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.fileName.Location = new System.Drawing.Point(37, 3);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(35, 13);
            this.fileName.TabIndex = 1;
            this.fileName.Text = "label1";
            this.fileName.Click += new System.EventHandler(this.fileName_Click);
            // 
            // fileSize
            // 
            this.fileSize.AutoSize = true;
            this.fileSize.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.fileSize.Location = new System.Drawing.Point(36, 19);
            this.fileSize.Name = "fileSize";
            this.fileSize.Size = new System.Drawing.Size(35, 13);
            this.fileSize.TabIndex = 2;
            this.fileSize.Text = "label1";
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 10;
            this.bunifuElipse1.TargetControl = this;
            // 
            // bunifuElipse2
            // 
            this.bunifuElipse2.ElipseRadius = 5;
            this.bunifuElipse2.TargetControl = this;
            // 
            // Imagen
            // 
            this.Imagen.Image = global::TDM.Properties.Resources.link;
            this.Imagen.Location = new System.Drawing.Point(5, 3);
            this.Imagen.Name = "Imagen";
            this.Imagen.Size = new System.Drawing.Size(30, 30);
            this.Imagen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Imagen.TabIndex = 0;
            this.Imagen.TabStop = false;
            // 
            // Progres
            // 
            this.Progres.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Progres.ForeColor = System.Drawing.Color.Orange;
            this.Progres.Location = new System.Drawing.Point(208, 22);
            this.Progres.Name = "Progres";
            this.Progres.Size = new System.Drawing.Size(127, 10);
            this.Progres.TabIndex = 11;
            this.Progres.Visible = false;
            // 
            // timespan
            // 
            this.timespan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timespan.AutoSize = true;
            this.timespan.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.timespan.Location = new System.Drawing.Point(204, 4);
            this.timespan.Name = "timespan";
            this.timespan.Size = new System.Drawing.Size(0, 13);
            this.timespan.TabIndex = 12;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.ErrorImage = global::TDM.Properties.Resources.ic_play_arrow_action_light;
            this.btnClose.Image = global::TDM.Properties.Resources.com_facebook_close;
            this.btnClose.Location = new System.Drawing.Point(315, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(20, 20);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnClose.TabIndex = 10;
            this.btnClose.TabStop = false;
            this.btnClose.Visible = false;
            this.btnClose.WaitOnLoad = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseEnter += new System.EventHandler(this.btnClose_MouseEnter);
            this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
            // 
            // DownloadControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.timespan);
            this.Controls.Add(this.Progres);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.fileSize);
            this.Controls.Add(this.fileName);
            this.Controls.Add(this.Imagen);
            this.Name = "DownloadControl";
            this.Size = new System.Drawing.Size(338, 37);
            this.Load += new System.EventHandler(this.DownloadControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Imagen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Imagen;
        private System.Windows.Forms.Label fileName;
        private System.Windows.Forms.Label fileSize;
        private Bunifu.Framework.UI.BunifuElipse bunifuElipse1;
        private Bunifu.Framework.UI.BunifuElipse bunifuElipse2;
        private System.Windows.Forms.ProgressBar Progres;
        private System.Windows.Forms.Label timespan;
        private System.Windows.Forms.PictureBox btnClose;
    }
}
