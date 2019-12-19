using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using adv_of_code_2019.Classes;
using static adv_of_code_2019.painter;

namespace adv_of_code_2019
{
    public static class Day19
    {
        public static async Task Run ()
        {
            var input = (await File.ReadAllTextAsync ("inputs\\19.txt"));
            var mem = IntMachineBase.ParseProgram (input);

            Dictionary<Point, bool> points = new Dictionary<Point, bool> ();

            for (int y = 0; y < 50; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    points.Add (new Point (x, y), IsPulling (mem, x, y));
                }
            }

            Console.WriteLine ("Part 1: " + points.Where (e => e.Value).Count ());

            Console.WriteLine ("Part 2: " + CalcPart2 (mem));

        }

        public static string CalcPart2 (long[] mem)
        {
            int y = 10;
            int xStart = 0;
            while (true)
            {
                y++;
                // if (y % 10 == 0)
                //     Console.Write (y);
                while (!IsPulling (mem, xStart, y))
                {
                    xStart++;
                };
                int x = xStart;
                while (IsPulling (mem, x + 99, y))
                {
                    if (!IsPulling (mem, x, y))
                        throw new Exception ("weird");
                    if (IsPulling (mem, x, y + 99))
                    {
                        return $"{x * 10000 + y}";
                    }
                    x++;
                }
            }
        }

        private static bool IsPulling (long [] memory, int x, int y)
        {
            var intMachine = new SynchronousIntMachine (memory);
            intMachine.InputQueue.Enqueue (x);
            intMachine.InputQueue.Enqueue (y);
            intMachine.RunUntilBlockOrComplete ();
            return intMachine.OutputQueue.Dequeue () == 1;
        }

    }
}