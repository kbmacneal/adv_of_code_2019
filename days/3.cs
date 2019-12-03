using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2019 {
    public class Day3 {
        private class coord : IEquatable<coord> {
            public int x { get; set; }
            public int y { get; set; }
            public List<coord> move_right (int spaces) {
                var traveled_spaces = new List<coord> ();

                for (int i = 1; i <= spaces; i++) {
                    traveled_spaces.Add (new coord () {
                        x = this.x + i,
                            y = this.y
                    });
                }

                return traveled_spaces;
            }

            public List<coord> move_left (int spaces) {
                var traveled_spaces = new List<coord> ();

                for (int i = 1; i <= spaces; i++) {
                    traveled_spaces.Add (new coord () {
                        x = this.x - i,
                            y = this.y
                    });
                }

                return traveled_spaces;
            }

            public List<coord> move_up (int spaces) {
                var traveled_spaces = new List<coord> ();

                for (int i = 1; i <= spaces; i++) {
                    traveled_spaces.Add (new coord () {
                        x = this.x,
                            y = this.y + i
                    });
                }

                return traveled_spaces;
            }

            public List<coord> move_down (int spaces) {
                var traveled_spaces = new List<coord> ();

                for (int i = 1; i <= spaces; i++) {
                    traveled_spaces.Add (new coord () {
                        x = this.x,
                            y = this.y - i
                    });
                }

                return traveled_spaces;
            }

            public bool Equals (coord other) {
                return this.x == other.x && this.y == other.y;
            }

            public int calc_manhattan_dist () {
                return Math.Abs (this.x) + Math.Abs (this.y);
            }
        }

        private class intersection{
            public coord wire_1_coord {get;set;}
            public coord wire_2_coord {get;set;}
            public int wire_1_stepcount{get;set;}
            public int wire_2_stepcount{get;set;}
        }
        public static async Task Run () {
            List<string> inputs = (await File.ReadAllLinesAsync ("inputs/3.txt")).ToList ();

            List<string> wire1 = inputs[0].Split (",").ToList ();

            List<string> wire2 = inputs[1].Split (",").ToList ();

            List<coord> wire1_traveled = new List<coord> ();
            List<coord> wire2_traveled = new List<coord> ();

            coord location = new coord ();
            location.y = 0;
            location.x = 0;

            foreach (var inst in wire1) {
                List<coord> traveled = new List<coord> ();

                char dir = inst[0];
                switch (dir) {
                    case 'R':
                        traveled = location.move_right (Int32.Parse (inst.Substring (1)));
                        break;
                    case 'L':
                        traveled = location.move_left (Int32.Parse (inst.Substring (1)));
                        break;
                    case 'U':
                        traveled = location.move_up (Int32.Parse (inst.Substring (1)));
                        break;
                    case 'D':
                        traveled = location.move_down (Int32.Parse (inst.Substring (1)));
                        break;
                    default:
                        break;
                }
                location = traveled.Last ();
                wire1_traveled.AddRange (traveled);
            }

            location = new coord ();
            location.y = 0;
            location.x = 0;

            foreach (var inst in wire2) {
                List<coord> traveled = new List<coord> ();

                char dir = inst[0];
                switch (dir) {
                    case 'R':
                        traveled = location.move_right (Int32.Parse (inst.Substring (1)));
                        break;
                    case 'L':
                        traveled = location.move_left (Int32.Parse (inst.Substring (1)));
                        break;
                    case 'U':
                        traveled = location.move_up (Int32.Parse (inst.Substring (1)));
                        break;
                    case 'D':
                        traveled = location.move_down (Int32.Parse (inst.Substring (1)));
                        break;
                    default:
                        break;
                }
                location = traveled.Last ();
                wire2_traveled.AddRange (traveled);
            }

            List<coord> intersects = wire1_traveled.Where(e=>wire2_traveled.Contains(e)).ToList();

            Console.WriteLine("Part 1:" + intersects.OrderBy(e=>e.calc_manhattan_dist()).First().calc_manhattan_dist().ToString());

            List<intersection> final_list = new List<intersection>();

            foreach (var intersect in intersects)
            {
                var inter = new intersection();
                inter.wire_1_coord = intersect;
                inter.wire_2_coord = intersect;

                inter.wire_1_stepcount = wire1_traveled.IndexOf(intersect);
                inter.wire_2_stepcount = wire2_traveled.IndexOf(intersect);

                final_list.Add(inter);
            }

            Console.WriteLine("Part 2: " + final_list.OrderBy(e=>e.wire_1_stepcount+e.wire_2_stepcount).Select(e=>e.wire_1_stepcount+e.wire_2_stepcount+2).First().ToString());
        }
    }
}