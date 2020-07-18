using System;

namespace UACChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            Environment.Exit(UacHelper.IsProcessElevated ? 1 : 0);
        }
    }
}
