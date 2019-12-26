using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2019
{
    public class Day8
    {
        private const int black = 0;
        private const int white = 1;
        private const int transparent = 2;

        public static async Task Run ()
        {
            var inputs = await File.ReadAllTextAsync ("inputs\\8.txt");

            var bits = inputs.Select (e => Int32.Parse (e.ToString ())).ToList ();

            int height = 6, width = 25;

            var layer_size = width * height;

            List<List<int>> layers = new List<List<int>> ();

            for (int i = 0; i < bits.Count (); i += layer_size)
            {
                List<int> layer = new List<int> (bits.Skip (i).Take (layer_size));

                layers.Add (layer);
            }

            var min_0s = layers.Select (e => e.Count (e => e == 0)).Min ();

            var min_layer = layers.First (e => e.Count (e => e == 0) == min_0s);

            var part1 = min_layer.Count (e => e == 1) * min_layer.Count (e => e == 2);

            Console.WriteLine ("Part 1: " + part1.ToString ());

            List<int> home = layers [0];

            for (int i = 0; i < home.Count (); i++)
            {
                if (home [i] == transparent)
                {
                    home [i] = FindFirstNonTransparentPixel (i, layers);
                }
            }

            List<List<int>> rows = new List<List<int>> ();

            for (int i = 0; i < home.Count (); i += width)
            {
                rows.Add (home.Skip (i).Take (width).ToList ());
            }

            Bitmap bmp = new Bitmap (width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bmp.SetPixel (x, y, (rows [y]) [x] == black ? Color.Black : Color.White);
                }
            }

            if (!Directory.Exists ("outputs")) Directory.CreateDirectory ("outputs");

            bmp.Save ("outputs\\8_2.png", ImageFormat.Png);
        }

        private static int FindFirstNonTransparentPixel (int position, List<List<int>> bits)
        {
            var rtn = 0;

            List<int> position_bits = bits.Select (e => e.ElementAt (position)).ToList ();

            rtn = position_bits.First (e => e != transparent);

            return rtn;
        }
    }
}