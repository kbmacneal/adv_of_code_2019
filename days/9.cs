using adv_of_code_2019.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2019
{
    public class Day9
    {
        public static async Task Run()
        {
            var inputs = await File.ReadAllTextAsync("inputs\\9.txt");

            painter.SynchronousIntMachine vm = new painter.SynchronousIntMachine(inputs);

            vm.InputQueue.Enqueue(1);

            var c = 0;

            while (vm.RunUntilBlockOrComplete() != painter.SynchronousIntMachine.ReturnCode.Completed)
            {
                c++;
            }

            Console.WriteLine("Part 1: " + vm.OutputQueue.Last());

            vm = new painter.SynchronousIntMachine(inputs);

            vm.InputQueue.Enqueue(2);

            c = 0;

            while (vm.RunUntilBlockOrComplete() != painter.SynchronousIntMachine.ReturnCode.Completed)
            {
                c++;
            }

            Console.WriteLine("Part 2: " + vm.OutputQueue.Last());
        }
    }
}