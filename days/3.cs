using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using adv_of_code_2019.Classes;

namespace adv_of_code_2019
{
    public class Day3
    {
        private class intersection
        {
            public Point wire_1_coord { get; set; }
            public Point wire_2_coord { get; set; }
            public int wire_1_stepcount { get; set; }
            public int wire_2_stepcount { get; set; }
        }

        public static async Task Run ()
        {
            List<string> inputs = (await File.ReadAllLinesAsync ("inputs/3.txt")).ToList ();

            List<string> wire1 = inputs [0].Split (",").ToList ();

            List<string> wire2 = inputs [1].Split (",").ToList ();

            List<Point> wire1_traveled = new List<Point> ();
            List<Point> wire2_traveled = new List<Point> ();

            Point location = new Point (0, 0);

            foreach (var inst in wire1)
            {
                List<Point> traveled = new List<Point> ();

                char dir = inst [0];
                switch (dir)
                {
                case 'R':
                    traveled = location.moveRight (Int32.Parse (inst.Substring (1)));
                    break;

                case 'L':
                    traveled = location.moveLeft (Int32.Parse (inst.Substring (1)));
                    break;

                case 'U':
                    traveled = location.moveUp (Int32.Parse (inst.Substring (1)));
                    break;

                case 'D':
                    traveled = location.moveDown (Int32.Parse (inst.Substring (1)));
                    break;

                default:
                    break;
                }
                location = traveled.Last ();
                wire1_traveled.AddRange (traveled);
            }

            location = new Point (0, 0);

            foreach (var inst in wire2)
            {
                List<Point> traveled = new List<Point> ();

                char dir = inst [0];
                switch (dir)
                {
                case 'R':
                    traveled = location.moveRight (Int32.Parse (inst.Substring (1)));
                    break;

                case 'L':
                    traveled = location.moveLeft (Int32.Parse (inst.Substring (1)));
                    break;

                case 'U':
                    traveled = location.moveUp (Int32.Parse (inst.Substring (1)));
                    break;

                case 'D':
                    traveled = location.moveDown (Int32.Parse (inst.Substring (1)));
                    break;

                default:
                    break;
                }
                location = traveled.Last ();
                wire2_traveled.AddRange (traveled);
            }

            List<Point> intersects = wire1_traveled.Where (e => wire2_traveled.Contains (e)).ToList ();

            Console.WriteLine ("Part 1:" + intersects.OrderBy (e => e.ManhDist ()).First ().ManhDist ().ToString ());

            List<intersection> final_list = new List<intersection> ();

            foreach (var intersect in intersects)
            {
                var inter = new intersection ();
                inter.wire_1_coord = intersect;
                inter.wire_2_coord = intersect;

                inter.wire_1_stepcount = wire1_traveled.IndexOf (intersect);
                inter.wire_2_stepcount = wire2_traveled.IndexOf (intersect);

                final_list.Add (inter);
            }

            Console.WriteLine ("Part 2: " + final_list.OrderBy (e => e.wire_1_stepcount + e.wire_2_stepcount).Select (e => e.wire_1_stepcount + e.wire_2_stepcount + 2).First ().ToString ());
        }
    }
}