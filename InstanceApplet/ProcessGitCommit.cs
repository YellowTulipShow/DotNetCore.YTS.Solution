
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace InstanceApplet
{
    public class ProcessGitCommit
    {
        public static void OnExcute()
        {
            string workDirectory = @"D:\Work\YTS.ZRQ\PlanNotes.YTSZRQ.StorageArea";
            var git = new GitHelper(workDirectory);
            git.AddALL();
            git.Status();
            git.Commit();
        }

        class GitHelper
        {
            private readonly Encoding encoding = Encoding.UTF8;
            private readonly string WorkingDirectory;

            public GitHelper(string WorkingDirectory)
            {
                this.WorkingDirectory = WorkingDirectory;
            }

            public void AddALL()
            {
                ProcessStartInfo info = new ProcessStartInfo("git", "add .")
                {
                    UseShellExecute = false,
                    WorkingDirectory = WorkingDirectory,
                    RedirectStandardInput = true,
                    //RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = encoding,
                    StandardInputEncoding = encoding,
                };
                Process process = Process.Start(info);
                using (var sr = process.StandardOutput)
                {
                    while (!sr.EndOfStream)
                    {
                        Console.WriteLine($"[Git 消息]: {sr.ReadLine()}");
                    }
                }
                process.WaitForExit();
                process.Close();
            }

            public void Status()
            {
                ProcessStartInfo info = new ProcessStartInfo("git", "status")
                {
                    UseShellExecute = false,
                    WorkingDirectory = WorkingDirectory,
                    RedirectStandardInput = true,
                    //RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = encoding,
                    StandardInputEncoding = encoding,
                };
                Process process = Process.Start(info);
                using (var sr = process.StandardOutput)
                {
                    while (!sr.EndOfStream)
                    {
                        Console.WriteLine($"[Git 消息]: {sr.ReadLine()}");
                    }
                }
                process.WaitForExit();
                process.Close();
            }

            public void Commit()
            {
                ProcessStartInfo info = new ProcessStartInfo("git", "commit")
                {
                    UseShellExecute = false,
                    WorkingDirectory = WorkingDirectory,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = encoding,
                    StandardInputEncoding = encoding,
                };
                Process process = Process.Start(info);
                using (var sr = process.StandardOutput)
                {
                    while (!sr.EndOfStream)
                    {
                        Console.WriteLine($"[Git 提交]: {sr.ReadLine()}");
                    }
                }
                /*
                process.BeginOutputReadLine();
                bool isWriteMessage = false;
                process.OutputDataReceived += (object sender, DataReceivedEventArgs outLine) => {
                    if (!string.IsNullOrEmpty(outLine.Data))
                    {
                        Console.WriteLine($"[Git 消息]: {outLine.Data}");
                    }
                    else if (!isWriteMessage)
                    {
                        using var sw = process.StandardInput;
                        sw.WriteLine("增加新的文件");
                        sw.WriteLine("");
                        sw.WriteLine("# 这里是备注内容");
                        sw.WriteLine("# sldijflad");
                        sw.Close();
                    }
                };
                */
                process.WaitForExit();
                process.Close();
            }
        }
    }
}
