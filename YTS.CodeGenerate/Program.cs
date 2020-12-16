using System;

namespace YTS.CodeGenerate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine($"args[{i}]: {args[i]}");
            }
            Console.WriteLine("Hello World!");
        }
    }
}
