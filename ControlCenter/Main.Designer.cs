﻿namespace ControlCenter
{
    partial class Main
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
            this.ShellPrefix = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ShellButton = new System.Windows.Forms.Button();
            this.ShellOutput = new System.Windows.Forms.TextBox();
            this.CommandText = new System.Windows.Forms.TextBox();
            this.CommandButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ScreenCapture = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ShellPrefix
            // 
            this.ShellPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ShellPrefix.Location = new System.Drawing.Point(52, 9);
            this.ShellPrefix.Name = "ShellPrefix";
            this.ShellPrefix.Size = new System.Drawing.Size(306, 20);
            this.ShellPrefix.TabIndex = 0;
            this.ShellPrefix.Text = "#TwitterCommandAndControl";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Prefix:";
            // 
            // ShellButton
            // 
            this.ShellButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ShellButton.Location = new System.Drawing.Point(364, 7);
            this.ShellButton.Name = "ShellButton";
            this.ShellButton.Size = new System.Drawing.Size(75, 23);
            this.ShellButton.TabIndex = 2;
            this.ShellButton.Text = "Start Shell";
            this.ShellButton.UseVisualStyleBackColor = true;
            this.ShellButton.Click += new System.EventHandler(this.ShellButton_Click);
            // 
            // ShellOutput
            // 
            this.ShellOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ShellOutput.Location = new System.Drawing.Point(12, 35);
            this.ShellOutput.Multiline = true;
            this.ShellOutput.Name = "ShellOutput";
            this.ShellOutput.Size = new System.Drawing.Size(548, 289);
            this.ShellOutput.TabIndex = 3;
            // 
            // CommandText
            // 
            this.CommandText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommandText.Location = new System.Drawing.Point(49, 332);
            this.CommandText.Name = "CommandText";
            this.CommandText.Size = new System.Drawing.Size(430, 20);
            this.CommandText.TabIndex = 4;
            // 
            // CommandButton
            // 
            this.CommandButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CommandButton.Location = new System.Drawing.Point(485, 330);
            this.CommandButton.Name = "CommandButton";
            this.CommandButton.Size = new System.Drawing.Size(75, 23);
            this.CommandButton.TabIndex = 5;
            this.CommandButton.Text = "Send";
            this.CommandButton.UseVisualStyleBackColor = true;
            this.CommandButton.Click += new System.EventHandler(this.CommandButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 335);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Cmd:";
            // 
            // ScreenCapture
            // 
            this.ScreenCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScreenCapture.Location = new System.Drawing.Point(445, 7);
            this.ScreenCapture.Name = "ScreenCapture";
            this.ScreenCapture.Size = new System.Drawing.Size(115, 23);
            this.ScreenCapture.TabIndex = 7;
            this.ScreenCapture.Text = "Screen Capture";
            this.ScreenCapture.UseVisualStyleBackColor = true;
            this.ScreenCapture.Click += new System.EventHandler(this.ScreenCapture_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 364);
            this.Controls.Add(this.ScreenCapture);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CommandButton);
            this.Controls.Add(this.CommandText);
            this.Controls.Add(this.ShellOutput);
            this.Controls.Add(this.ShellButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ShellPrefix);
            this.Name = "Main";
            this.Text = "Control Center";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ShellPrefix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ShellButton;
        private System.Windows.Forms.TextBox ShellOutput;
        private System.Windows.Forms.TextBox CommandText;
        private System.Windows.Forms.Button CommandButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ScreenCapture;
    }
}
