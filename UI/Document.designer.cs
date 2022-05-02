namespace TDM
{
    partial class Descargas
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
            this.ContentPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // ContentPanel
            // 
            this.ContentPanel.AllowDrop = true;
            this.ContentPanel.AutoScroll = true;
            this.ContentPanel.BackColor = System.Drawing.Color.Black;
            this.ContentPanel.BackgroundImage = global::TDM.Properties.Resources._1_5145639539531841876;
            this.ContentPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentPanel.Location = new System.Drawing.Point(0, 0);
            this.ContentPanel.Name = "ContentPanel";
            this.ContentPanel.Size = new System.Drawing.Size(603, 510);
            this.ContentPanel.TabIndex = 9;
            this.ContentPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.ContentPanel_DragDrop);
            this.ContentPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.ContentPanel_DragEnter);
            this.ContentPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ContentPanel_Paint);
            // 
            // Descargas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.Controls.Add(this.ContentPanel);
            this.DockText = "GameView";
            this.Name = "Descargas";
            this.SerializationKey = "GameView";
            this.Size = new System.Drawing.Size(603, 510);
            this.Load += new System.EventHandler(this.GameView_Load);
            this.Click += new System.EventHandler(this.Descargas_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ContentPanel;
    }
}
