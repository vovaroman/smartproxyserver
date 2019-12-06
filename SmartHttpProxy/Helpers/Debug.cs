using System;

namespace SmartHttpProxy
{
    public static class Debug
    {
        private static Boolean Status = true;
        public static void Print(object obj){
            Console.ForegroundColor 
            = ConsoleColor.Red;
            if(Status) Console.WriteLine($"# {obj}");
            Console.ResetColor();

        }

        public static void PrintInformation(object obj){
            Console.ForegroundColor 
            = ConsoleColor.Green;
            Console.WriteLine($"# {obj}");
            Console.ResetColor();
        }
    }
}
