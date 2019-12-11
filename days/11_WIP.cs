using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using adv_of_code_2019.Classes;
using MoreLinq;
using Point = adv_of_code_2019.Classes.Point;

namespace adv_of_code_2019 {

    public class Day11 {

        private static bool is_coloring_mode = true;

        private static List<long> commands = File.ReadAllText ("inputs\\11.txt").Split (",").Select (Int64.Parse).ToList ();

        private static Processor processor {get;set;} = new Processor (commands.ToArray ());
        private static Dictionary<Point, int> canvas { get; set; } = new Dictionary<Point, int> ();

        private static HashSet<Point> PaintedPositions { get; set; } = new HashSet<Point> ();

        private static int current_direction = 0;
        private static Point current_loc { get; set; } = new Point (0, 0);

        private static Point[] directions { get; } = new [] { new Point (-1, 0), new Point (0, 1), new Point (1, 0), new Point (0, -1) };

        public static async Task Run () {

            var part1 = await PaintAsync ();

            Console.WriteLine ("Part 1: " + part1);

            var text = await Paint2Async ();

            Console.WriteLine ("Part 2:");
            Console.Write (text);
            Console.WriteLine();

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

        private static async Task<string> Paint2Async () {
            processor = new Processor (commands.ToArray ());

            canvas = new Dictionary<Point, int>();

            current_loc = new Point (0, 0);

            current_direction = 0;

            PaintedPositions = new HashSet<Point> ();

            is_coloring_mode = true;

            var rtn = new Dictionary<Point,int>();

            var color = canvas.GetOrAdd (current_loc, _ => 1);

            processor.AddInput (color);

            processor.ProgramOutput += OnOutput;

            processor.ProccessProgram ();

            // rtn = canvas.DistinctBy (e => e.Key).ToDictionary();

            return Render(canvas);
        }

        private static string Render(Dictionary<Point,int> rtn)
        {
            var whitePoints = rtn.Where(x => x.Value == 1).Select(x => x.Key).ToList();
            var topLeft = new Point(whitePoints.Min(x => x.X), whitePoints.Min(x => x.Y));
            var bottomRight = new Point(whitePoints.Max(x => x.X), whitePoints.Max(x => x.Y));
            var resultSb = new StringBuilder();
            for (var x = topLeft.X; x <= bottomRight.X; x++)
            {
                for (var y = topLeft.Y; y <= bottomRight.Y; y++)
                {
                    if (rtn.TryGetValue(new Point(x, y), out var color))
                    {
                        resultSb.Append(color == 0 ? ' ' : '#');
                    }
                    else
                    {
                        resultSb.Append(' ');
                    }
                }
                resultSb.AppendLine();
            }

            return resultSb.ToString();
        }

        private static void CleanColumn(int position, List<string> strings)
        {

            List<char> position_string = strings.Select(e => e.ElementAt(position)).ToList();

            if(!position_string.Contains('#'))
            {
                strings.ForEach(e=>e.Remove(position,1));
            }
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