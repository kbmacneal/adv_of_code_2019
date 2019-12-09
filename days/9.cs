using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using adv_of_code_2019.Classes;

namespace adv_of_code_2019
{
    public class Day9
    {
        public static async Task Run ()
        {
            var inputs = await File.ReadAllTextAsync ("inputs\\9.txt");
            
            List<long> commands = new List<long>();

            foreach (string s in inputs.Split(','))
            {
                
                if (long.TryParse(s, out long parsed))
                {
                    commands.Add(parsed);
                }
                else
                {
                    throw new Exception($"Failed to parse '{s}' to a long");
                }
            }

            Processor pcA = new Processor(commands.ToArray());
            pcA.ProgramOutput += Pc_ProgramOutput;
            pcA.ProgramFinish += Pc_ProgramFinish;
            pcA.AddInput(1);

            Console.WriteLine("Part 1:");
            pcA.ProccessProgram();

            pcA.ResetInputs();
            pcA.AddInput(2);

            Console.WriteLine("Part 2:");
            pcA.ProccessProgram();

        }

        private static void Pc_ProgramFinish(object sender, EventArgs e)
        {
            
        }

        private static void Pc_ProgramOutput(object sender, OutputEventArgs e)
        {
            Console.WriteLine(e.OutputValue);
        }
    }
}