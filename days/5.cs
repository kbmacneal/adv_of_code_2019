using adv_of_code_2019.Classes;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2019
{
    public class Day5
    {
#pragma warning disable 1998

        public static async Task Run()
        {
            var vm = new painter.SynchronousIntMachine(await File.ReadAllTextAsync("inputs\\5.txt"));

            var c = 0;

            vm.InputQueue.Enqueue(1);

            while (vm.RunUntilBlockOrComplete() != painter.SynchronousIntMachine.ReturnCode.Completed)
            {
                c++;
            }

            Console.WriteLine("Part 1: " + vm.OutputQueue.Last());

            vm = new painter.SynchronousIntMachine(await File.ReadAllTextAsync("inputs\\5.txt"));

            vm.InputQueue.Enqueue(5);

            c = 0;

            while (vm.RunUntilBlockOrComplete() != painter.SynchronousIntMachine.ReturnCode.Completed)
            {
                c++;
            }

            Console.WriteLine("Part 2: " + vm.OutputQueue.Last());
        }
    }
}