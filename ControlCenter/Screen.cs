using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlCenter
{
    public partial class Screen : Form
    {
        StreamWriter writer = null;
        StreamReader reader = null;
        private Timer UpdateTimer = new Timer() { Interval = 1000 };
        public TcpClient Client { get; set; }

        public Screen()
        {
            InitializeComponent();
        }

        ~Screen()
        {
            UpdateTimer.Stop();
        }

        private void Screen_Load(object sender, EventArgs e)
        {
            UpdateTimer.Tick += (s, a) =>
            {
                if (Client != null && Client.Connected)
                {
                    if (writer == null)
                        writer = new StreamWriter(Client.GetStream());
                    if (reader == null)
                        reader = new StreamReader(Client.GetStream());
                    writer.WriteLine("X");
                    writer.Flush();
                    var base64 = reader.ReadLine();
                    using (var memoryStream = new MemoryStream(System.Convert.FromBase64String(base64)))
                    {
                        var img = Bitmap.FromStream(memoryStream);
                        try
                        {
                            ScreenDisplay.Invoke(new MethodInvoker(() =>
                            {

                                var old = ScreenDisplay.Image;
                                ScreenDisplay.Image = img;
                                if (old != null)
                                    old.Dispose();

                            }));
                        }
                        catch { UpdateTimer.Stop(); }
                    }
                }
            };
            UpdateTimer.Start();
        }
    }
}
