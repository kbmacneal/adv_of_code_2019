using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using adv_of_code_2019.Classes;
using MoreLinq;

namespace adv_of_code_2019 {

    public class Day11 {

        private static bool is_coloring_mode = true;

        private static List<long> commands = File.ReadAllText("inputs\\11.txt").Split(",").Select(Int64.Parse).ToList();

        private static Processor processor = new Processor(commands.ToArray());
        private static Dictionary<Point, int> canvas { get; set; } = new Dictionary<Point, int> ();

        private static HashSet<Point> PaintedPositions { get; set; } = new HashSet<Point> ();

        private static int current_direction = 0;
        private static Point current_loc { get; set; } = new Point (0, 0);

        private static Point[] directions { get; } = new [] { new Point (-1, 0), new Point (0, 1), new Point (1, 0), new Point (0, -1) };

        public static async Task Run () {

            var part1 = await PaintAsync ();

            Console.WriteLine ("Part 1: " + part1);

        }

        private static async Task<int> PaintAsync () {

            var rtn = 0;

            var color = canvas.GetOrAdd (current_loc, _ => 0);

            processor.AddInput (color);

            processor.ProgramOutput += OnOutput;

            processor.ProccessProgram ();

            rtn = canvas.DistinctBy (e => e.Key).Count ();

            return rtn;

        }

        private static void OnOutput (object sender, OutputEventArgs e) {
            if (is_coloring_mode) {
                long out_val = e.OutputValue;

                canvas[current_loc] = (int) out_val;
                PaintedPositions.Add (current_loc);

                is_coloring_mode = !is_coloring_mode;
            } else {
                long out_val = e.OutputValue;

                var directionDelta = (int) out_val == 0 ? -1 : 1;

                current_direction = (((int) current_direction + directionDelta + 4) % 4);

                current_loc += directions[current_direction];

                var color = canvas.GetOrAdd (current_loc, _ => 0);

                processor.AddInput (color);

                is_coloring_mode = !is_coloring_mode;
            }

        }
    }
}