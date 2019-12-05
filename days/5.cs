using adv_of_code_2019.Classes;
using System;
using System.IO;
using System.Threading.Tasks;

namespace adv_of_code_2019
{
    public class Day5
    {
#pragma warning disable 1998

        public static async Task Run()
        {
            var vm = new day5vm();

            vm.input = 1;

            vm.input_instructions = (await File.ReadAllTextAsync("inputs\\5.txt"));

            Console.WriteLine("Part 1: " + await vm.Process());

            vm = new day5vm();

            vm.input = 5;

            vm.input_instructions = (await File.ReadAllTextAsync("inputs\\5.txt"));

            Console.WriteLine("Part 2: " + await vm.Process());
        }
    }
}