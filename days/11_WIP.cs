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

namespace adv_of_code_2019
{

    public static class Day11
    {
        public static async Task Run ()
        {
            painter paint = new painter ();
            paint.initial_color = 0;
            var part1 = await paint.PaintAsync ();

            Console.WriteLine ("Part 1: " + part1);

            var paint2 = new painter ();
            paint2.initial_color = 1;
            var text = await paint2.Paint2Async ();

            if (!Directory.Exists ("outputs")) Directory.CreateDirectory ("outputs");

            text.Save ("outputs\\11.png", ImageFormat.Png);

        }
    }

    public class painter
    {

        private bool is_coloring_mode = true;

        private static List<long> commands { get; set; } = File.ReadAllText ("inputs\\11.txt").Split (",").Select (Int64.Parse).ToList ();

        private Processor processor { get; set; } = new Processor (commands.ToArray ());
        private Dictionary<Point, int> canvas { get; set; } = new Dictionary<Point, int> ();

        private HashSet<Point> PaintedPositions { get; set; } = new HashSet<Point> ();

        private int current_direction { get; set; } = 0;
        private Point current_loc { get; set; } = new Point (0, 0);

        private Point[] directions { get; } = new [] { new Point (-1, 0), new Point (0, 1), new Point (1, 0), new Point (0, -1) };

        public int initial_color { get; set; }

        public async Task<int> PaintAsync ()
        {

            var rtn = 0;

            var color = canvas.GetOrAdd (current_loc, _ => this.initial_color);

            processor.AddInput (color);

            processor.ProgramOutput += OnOutput;

            processor.ProccessProgram ();

            rtn = canvas.DistinctBy (e => e.Key).Count ();

            return rtn;

        }

        public async Task<Bitmap> Paint2Async ()
        {

            var rtn = new Dictionary<Point, int> ();

            var color = canvas.GetOrAdd (current_loc, _ => this.initial_color);

            processor.AddInput (color);

            processor.ProgramOutput += OnOutput;

            processor.ProccessProgram ();

            //rtn = canvas.DistinctBy(e => e.Key).ToDictionary();
            var whitePoints = canvas.Where (x => x.Value == 1).Select (x => x.Key).ToList ();
            var bottomleft = new Point (whitePoints.Min (x => x.X), whitePoints.Min (x => x.Y));
            var topright = new Point (whitePoints.Max (x => x.X), whitePoints.Max (x => x.Y));

            var width = topright.X+1;

            var height = topright.Y+1;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap (height,width);

            

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bmp.SetPixel(y,x,Color.Black);

                }
            }
            
            whitePoints.ForEach(e=>bmp.SetPixel(e.Y,e.X,Color.White));

            return bmp;
        }

        private string Render ()
        {
            var whitePoints = canvas.Where (x => x.Value == 1).Select (x => x.Key).ToList ();
            var topLeft = new Point (whitePoints.Min (x => x.X), whitePoints.Min (x => x.Y));
            var bottomRight = new Point (whitePoints.Max (x => x.X), whitePoints.Max (x => x.Y));
            var resultSb = new StringBuilder ();
            for (var x = topLeft.X; x <= bottomRight.X; x++)
            {
                for (var y = topLeft.Y; y <= bottomRight.Y; y++)
                {
                    if (canvas.TryGetValue (new Point (x, y), out var color))
                    {
                        resultSb.Append (color == 0 ? ' ' : '#');
                    }
                    else
                    {
                        resultSb.Append (' ');
                    }
                }
                resultSb.AppendLine ();
            }

            return resultSb.ToString ();
        }

        private void OnOutput (object sender, OutputEventArgs e)
        {
            if (this.is_coloring_mode)
            {
                long out_val = e.OutputValue;

                canvas[current_loc] = (int) out_val;
                PaintedPositions.Add (current_loc);

                is_coloring_mode = !is_coloring_mode;
            }
            else
            {
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