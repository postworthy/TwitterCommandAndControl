﻿namespace ControlCenter
{
    partial class Screen
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ScreenDisplay = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // ScreenDisplay
            // 
            this.ScreenDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScreenDisplay.Location = new System.Drawing.Point(0, 0);
            this.ScreenDisplay.Name = "ScreenDisplay";
            this.ScreenDisplay.Size = new System.Drawing.Size(284, 261);
            this.ScreenDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ScreenDisplay.TabIndex = 0;
            this.ScreenDisplay.TabStop = false;
            // 
            // Screen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ScreenDisplay);
            this.Name = "Screen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Screen";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Screen_FormClosed);
            this.Load += new System.EventHandler(this.Screen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ScreenDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ScreenDisplay;

    }
}