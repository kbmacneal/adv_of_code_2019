using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using adv_of_code_2019.Classes;
using MoreLinq;

namespace adv_of_code_2019 {

    public class Day11 {

        private static bool is_coloring_mode = true;

        private static Dictionary<Panel, int> canvas = new Dictionary<Panel, int> ();

        private class Panel {
            public int X { get; set; }
            public int Y { get; set; }
            public Panel (int x, int y) {
                this.X = x;
                this.Y = y;
            }
        }

        private static int current_direction = 0;
        private static Panel current_loc { get; set; } = new Panel (0, 0);

        public static async Task Run () {
            var inputs = await File.ReadAllTextAsync ("inputs\\11.txt");

            List<long> commands = new List<long> ();

            foreach (string s in inputs.Split (',')) {

                if (long.TryParse (s, out long parsed)) {
                    commands.Add (parsed);
                } else {
                    throw new Exception ($"Failed to parse '{s}' to a long");
                }
            }

            var part1 = await PaintAsync (new Processor (commands.ToArray ()));

            Console.WriteLine ("Part 1: " + part1);

            // Processor pcA = new Processor (commands.ToArray ());
            // pcA.ProgramOutput += Pc_ProgramOutput;
            // pcA.ProgramFinish += Pc_ProgramFinish;
            // pcA.AddInput (1);

            // Console.WriteLine ("Part 1:");
            // pcA.ProccessProgram ();

            // pcA.ResetInputs ();
            // pcA.AddInput (2);

            // Console.WriteLine ("Part 2:");
            // pcA.ProccessProgram ();

        }

        private static async Task<int> PaintAsync (Processor processor) {

            var rtn = 0;

            canvas.Add (new Panel (0, 0), 0);

            processor.AddInput (0);

            processor.ProgramOutput += OnOutput;

            processor.ProccessProgram ();

            rtn = canvas.DistinctBy (e => e.Key).Count ();

            return rtn;

        }

        private static void OnOutput (object sender, OutputEventArgs e) {
            if (is_coloring_mode) {
                long out_val = e.OutputValue;

                if (canvas.Count (e => e.Key.X == current_loc.X && e.Key.Y == current_loc.Y) > 0) {
                    // canvas.ReplaceValue<Panel,int>(current_loc,(int)out_val);
                    canvas.Where (e => e.Key.X == current_loc.X && e.Key.Y == current_loc.Y).ToList ().ForEach (e => canvas.Remove (e.Key));
                }
                canvas.Add (new Panel (current_loc.X, current_loc.Y), (int) out_val);

                is_coloring_mode = !is_coloring_mode;
            } else {
                long out_val = e.OutputValue;

                var directionDelta = (int) out_val == 0 ? -1 : 1;

                current_direction = (((int) current_direction + directionDelta + 4) % 4);

                switch (current_direction) {
                    case 0:
                        current_loc.Y += 1;
                        break;
                    case 1:
                        current_loc.X += 1;
                        break;
                    case 2:
                        current_loc.Y -= 1;
                        break;
                    case 3:
                        current_loc.X -= 1;
                        break;
                    default:

                        throw new InvalidOperationException ("Attempted to move in invalid direction.");
                }

                if (!canvas.TryGetValue (new Panel (current_loc.X, current_loc.Y), out var color)) {
                    canvas.Add (current_loc, 0);
                }

                (sender as Processor).AddInput (canvas[current_loc]);

                is_coloring_mode = !is_coloring_mode;
            }

        }
    }
}