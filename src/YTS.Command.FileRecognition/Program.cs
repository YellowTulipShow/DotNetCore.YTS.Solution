using System;
using System.IO;
using System.Text;

using YTS.Log;

namespace YTS.Command.FileRecognition
{
    class Program
    {
        static int Main(string[] args)
        {
            Encoding encoding = Encoding.UTF8;
            try
            {
                FileInfo logFile = ILogExtend.GetLogFilePath("Program");
                ILog log;
                log = new BasicJSONConsolePrintLog();
                // log = new ConsolePrintLog();
                log = new FilePrintLog(logFile, encoding).Connect(log);

                IMain main = new Main(log, encoding);
                CommandArgsParser commandArgsParser = new CommandArgsParser(log, main);
                return commandArgsParser.OnParser(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"程序出错: {ex.Message}");
                Console.WriteLine($"堆栈信息: {ex.StackTrace ?? string.Empty}");
                return 1;
            }
        }
    }
}
