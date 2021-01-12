using System;

namespace YTS.Tools
{
    public class ConsoleCleanManage
    {
        private int SelfCursorTop { get; set; }
        public ConsoleCleanManage()
        {
            RecordPoint();
        }

        public void RecordPoint()
        {
            SelfCursorTop = Console.CursorTop;
        }

        public void CleanALL()
        {
            while (Console.CursorTop >= SelfCursorTop)
            {
                CleanSingleLine();
                int topIndex = Console.CursorTop - 1;
                if (topIndex < 1) break;
                Console.SetCursorPosition(0, topIndex);
            }
            Console.WriteLine(string.Empty);
        }

        public void CleanSingleLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string('#', Console.BufferWidth - 1));
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth - 1));
            Console.SetCursorPosition(0, Console.CursorTop);
        }
    }
}
