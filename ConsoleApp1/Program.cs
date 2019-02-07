using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var qs = new QuartzScheduler(() => { Console.WriteLine("---"); }, 18, 52);
            qs.Schedule();
            Console.ReadKey();
        }
    }
}
