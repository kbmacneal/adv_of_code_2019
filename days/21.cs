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
    public static class Day21
    {
        public static async Task Run ()
        {
            var input = (await File.ReadAllTextAsync ("inputs\\21.txt"));

            var myIntMachine = new SynchronousIntMachine (input);

            myIntMachine.addASCIILine ("OR A J\n");
            myIntMachine.addASCIILine ("AND B J\n");
            myIntMachine.addASCIILine ("AND C J\n");
            myIntMachine.addASCIILine ("NOT J J\n");
            myIntMachine.addASCIILine ("AND D J\n");
            myIntMachine.addASCIILine ("WALK\n");

            var c = 0;

            while (myIntMachine.RunUntilBlockOrComplete () != SynchronousIntMachine.ReturnCode.Completed)
            {
                c++;
            }

            Console.WriteLine ("Part 1: " + myIntMachine.OutputQueue.Last ());

            myIntMachine = new SynchronousIntMachine (input);

            myIntMachine.addASCIILine ("OR A J\n");
            myIntMachine.addASCIILine ("AND B J\n");
            myIntMachine.addASCIILine ("AND C J\n");
            myIntMachine.addASCIILine ("NOT J J\n");
            myIntMachine.addASCIILine ("AND D J\n");
            myIntMachine.addASCIILine ("OR I T\n");
            myIntMachine.addASCIILine ("OR F T\n");
            myIntMachine.addASCIILine ("AND E T\n");
            myIntMachine.addASCIILine ("OR H T\n");
            myIntMachine.addASCIILine ("AND T J\n");
            myIntMachine.addASCIILine ("RUN\n");

            c = 0;

            while (myIntMachine.RunUntilBlockOrComplete () != SynchronousIntMachine.ReturnCode.Completed)
            {
                c++;
            }

            Console.WriteLine ("Part 2: " + myIntMachine.OutputQueue.Last ());

        }

    }
}