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
    public static class Day17
    {
        public static async Task Run ()
        {
            var input = (await File.ReadAllTextAsync ("inputs\\17.txt"));

            var myIntMachine = new SynchronousIntMachine (input);

            List<char> lines = new List<char> ();

            if (!File.Exists ("outputs\\17.txt"))
            {
                while (myIntMachine.RunUntilBlockOrComplete () != SynchronousIntMachine.ReturnCode.Completed)
                {
                    var o = myIntMachine.OutputQueue.Dequeue ();

                    lines.Add ((char) o);
                }

                var sb = new StringBuilder ();

                lines.ForEach (e => sb.Append (e));

                if (!Directory.Exists ("outputs")) Directory.CreateDirectory ("outputs");

                await File.WriteAllTextAsync ("outputs\\17.txt", sb.ToString (), Encoding.ASCII);
            }

            var scaffold = (await File.ReadAllLinesAsync ("outputs\\17.txt", Encoding.ASCII)).Where (e => e != "").ToArray ();

            var aparm = 0;

            for (int y = 0; y < scaffold.Length; y++)
            {
                for (int x = 0; x < scaffold [y].Length; x++)
                {
                    aparm += GetIntersect (x, y, scaffold);
                }
            }

            Console.WriteLine ("Part 1: " + aparm.ToString ());

            myIntMachine = new SynchronousIntMachine (input);

            myIntMachine.SetMemoryRegister (0, 2);

            var A = "L,12,L,8,R,10,R,10\n";
            var B = "L,6,L,4,L,12\n";
            var C = "R,10,L,8,L,4,R,10\n";

            var seq = "A,B,A,B,C,B,A,C,B,C\n";

            //use these to debug your input lengths
            //if you are stuck, try outputting the outputqueue after the vm is done running as ascii text
            // var la = A.ToCharArray ().Length;
            // var lb = B.ToCharArray ().Length;
            // var lc = C.ToCharArray ().Length;
            // var ls = seq.ToCharArray ().Length;

            myIntMachine.addASCIILine (seq);
            myIntMachine.addASCIILine (A);
            myIntMachine.addASCIILine (B);
            myIntMachine.addASCIILine (C);
            myIntMachine.addASCIILine ("n\n");

            var c = 0;

            while (myIntMachine.RunUntilBlockOrComplete () != SynchronousIntMachine.ReturnCode.Completed)
            {
                c++;
            }

            var part2 = myIntMachine.OutputQueue.Last ();

            Console.WriteLine ("Part 2: " + part2.ToString ());

        }

        private static int GetIntersect (int x, int y, string [] lines)
        {
            var rtn = 0;

            var itself = lines [y] [x];

            char up, down, left, right;

            if (y >= lines.Length - 1)
            {
                return 0;
            }
            else
            {
                up = lines [y + 1] [x];
            }

            if (y != 0)
            {
                down = lines [y - 1] [x];
            }
            else
            {
                return 0;
            }
            if (x != 0)
            {
                left = lines [y] [x - 1];
            }
            else
            {
                return 0;
            }

            if (x >= lines [y].Length - 1)
            {
                return 0;
            }
            else
            {
                right = lines [y] [x + 1];
            }

            if (up == '#' && down == '#' && left == '#' && right == '#' && itself == '#')
            {
                rtn = x * y;
            }

            return rtn;
        }

    }
}