using System;

public static class ConsoleExpand
{
    public static void ClearLineContents(int lineCount)
    {
        for (int i = 0; i < lineCount; i++)
        {
            // ClearConsoleLine(i); continue;
            if (i == 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.BufferWidth - 1));
            }
            else
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.BufferWidth - 1));
            }
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }
    public static void ClearConsoleLine(int invertedIndex = 0)
    {
        int currentLineCursor = Console.CursorTop;
        int top = Console.CursorTop - invertedIndex;
        top = top < 0 ? 0 : top;
        Console.SetCursorPosition(0, top);
        Console.Write(new string(' ', Console.WindowWidth - 1));
        Console.SetCursorPosition(0, top);
        // Console.SetCursorPosition(0, currentLineCursor);
    }
}
