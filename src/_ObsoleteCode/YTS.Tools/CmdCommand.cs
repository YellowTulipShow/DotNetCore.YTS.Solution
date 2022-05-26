using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace YTS.Tools
{
    public class CmdCommand
    {
        public CmdCommand()
        {
        }

        /// <summary>
        /// 执行命令, 返回字符串结果
        /// </summary>
        public string Execute(string str_command)
        {
            str_command = ConvertTool.ObjToString(str_command);
            Process p = new Process();
            // 设定程序名
            p.StartInfo.FileName = "cmd.exe";
            // 关闭Shell的使用
            p.StartInfo.UseShellExecute = false;
            // 重定向标准输入
            p.StartInfo.RedirectStandardInput = true;
            // 重定向标准输出
            p.StartInfo.RedirectStandardOutput = true;
            // 重定向错误输出
            p.StartInfo.RedirectStandardError = true;
            // 设置不显示窗口
            p.StartInfo.CreateNoWindow = true;
            // 上面几个属性的设置是比较关键的一步。
            // 都设置好了启动进程吧
            p.Start();
            // 输入要执行的命令
            p.StandardInput.AutoFlush = true;
            p.StandardInput.WriteLine(string.Empty);
            p.StandardInput.WriteLine(str_command);
            p.StandardInput.WriteLine("exit");
            p.StandardInput.WriteLine(str_command + "&exit");
            // 从输出流获取命令执行结果
            string strOuput = p.StandardOutput.ReadToEnd();
            //等待程序执行完退出进程
            p.WaitForExit();
            p.Close();
            return strOuput;
        }
    }
}
