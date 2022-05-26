
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Text;

namespace InstanceApplet
{
    public class ProcessGitBashOpen
    {
        public static void OnExcute()
        {
            string workDirectory = @"C:\Program Files\Git";
            var path = @"/D/Work/YTS.ZRQ/Play";
            path = @"D:\Work\YTS.ZRQ\Play";
            string fileName = @"C:\Program Files\Git\git-bash.exe";
            //fileName = @"git-bash.exe";

            SecureString securePwd = new SecureString();
            string pwd = "yts.zrq.1997";
            for (int i = 0; i < pwd.Length; i++)
            {
                securePwd.AppendChar(pwd[i]);
            }
            ProcessStartInfo info = new ProcessStartInfo(fileName, $"--cd=\"{path}\"")
            //ProcessStartInfo info = new ProcessStartInfo("git", "status")
            {
                UseShellExecute = false,
                WorkingDirectory = workDirectory,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8,
                UserName= "Administrator",
                Password = securePwd,
            };
            Process process = Process.Start(info);
            using (var sr = process.StandardOutput)
            {
                while (!sr.EndOfStream)
                {
                    Console.WriteLine($"[消息]: {sr.ReadLine()}");
                }
            }
            process.WaitForExit();
            process.Close();
        }
    }
}
