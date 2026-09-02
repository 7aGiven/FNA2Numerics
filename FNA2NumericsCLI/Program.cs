using System;

namespace FNA2NumericsCLI
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Usage: Do both command\nFNA2NumericsCLI.exe FNA.dll\nFNA2NumericsCLI.exe game.exe");
                return;
            }
            string path = args[0];
            FNA2Numerics.FNA2Numerics.Process(path);
        }
    }
}