using System;
using GetPassSecureString;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var password = ConsolePasswordReader.Read();
            Console.Write(password);
            Console.WriteLine();
        }
    }
}
