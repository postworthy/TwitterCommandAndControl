using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlCenter
{
    public partial class Screen : Form
    {
        public bool IsStreaming { get; set; }
        StreamReader reader = null;
        public TcpClient Client { get; set; }

        public Screen()
        {
            InitializeComponent();
        }

        ~Screen()
        {
            if (reader != null) reader.Dispose();
            if (Client != null) Client.Close();
        }

        private void Screen_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                while (Client == null)
                    Thread.Sleep(200);
                if (Client != null && Client.Connected)
                {
                    if (reader == null)
                        reader = new StreamReader(new GZipStream(Client.GetStream(), CompressionMode.Decompress));
                    while (Client.Connected)
                    {
                        try
                        {
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
                                catch { }
                            }
                        }
                        catch { }
                    }
                }
            });

        }

        private void Screen_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (reader != null) reader.Dispose();
            if (Client != null) Client.Close();
        }
    }
}
