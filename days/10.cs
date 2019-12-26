using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using adv_of_code_2019.Classes;

namespace adv_of_code_2019
{
    public class Day10
    {
        private class asteroid
        {
            public int x { get; set; }
            public int y { get; set; }
            public double angle { get; set; }

            public List<asteroid> visible { get; set; } = new List<asteroid> ();

            public void countVisible (List<asteroid> asteroids)
            {
                foreach (asteroid a in asteroids)
                    if (a != this && lineOfSight (a, asteroids))
                        this.visible.Add (a);
            }

            public bool sameAngle (asteroid a, asteroid b)
            {
                if (sign (a.x - this.x) != sign (b.x - this.x)) return false;
                if (sign (a.y - this.y) != sign (b.y - this.y)) return false;
                if (a.x == this.x && b.x == this.x) return true;
                if (a.y == this.y && b.y == this.y) return true;
                return ((a.x - this.x) * (b.y - this.y) == (b.x - this.x) * (a.y - this.y));
            }

            public bool lineOfSight (asteroid to, List<asteroid> asteroids)
            {
                int dx = (to.x - this.x).Abs ();
                int dy = (to.y - this.y).Abs ();
                foreach (asteroid a in asteroids)
                {
                    if (a != this && a != to)
                    {
                        int dxa = a.x - this.x;
                        int dya = a.y - this.y;
                        if (dxa.Abs () <= dx && dya.Abs () <= dy && sameAngle (to, a))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            public static int sign (int x)
            {
                return (x > 0) ? 1 : (x < 0) ? -1 : 0;
            }
        }

        public static async Task Run ()
        {
            var inputs = await File.ReadAllLinesAsync ("inputs\\10.txt");

            List<asteroid> asteroids = new List<asteroid> ();

            for (int y = 0; y < inputs.Length; y++)
            {
                for (int x = 0; x < inputs [y].Length; x++)
                {
                    if ((inputs [y]) [x].ToString () == "#")
                    {
                        asteroids.Add (new asteroid ()
                        {
                            x = x,
                                y = y
                        });
                    }
                }
            }

            asteroids.ForEach (e => e.countVisible (asteroids));

            var best = asteroids.First (e => e.visible.Count == asteroids.Max (e => e.visible.Count));

            Console.WriteLine ("Part 1: " + asteroids.Max (e => e.visible.Count).ToString ());

            int goal = 200;

            foreach (asteroid a in best.visible)
            {
                a.angle = -Math.Atan2 (a.x - best.x, a.y - best.y);
            }
            best.visible.Sort ((a, b) => a.angle.CompareTo (b.angle));
            asteroid target = best.visible [goal - 1];
            Console.WriteLine ("Part 2: " + (target.x * 100 + target.y).ToString ());
        }
    }
}