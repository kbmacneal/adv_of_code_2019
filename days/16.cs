using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using adv_of_code_2019.Classes;

namespace adv_of_code_2019
{
    public static class Day16
    {

        public static async Task Run ()
        {
            var input = (await File.ReadAllTextAsync ("inputs\\16.txt"));

            var ints = input.Select (e => e.ToString ()).Select (Int32.Parse).ToList ();

            // var ints = input.Split("").Select(Int32.Parse).ToList();

            var phases = new List<List<int>> ();

            var holder = new List<int> ();

            holder.AddRange (ints);

            for (int i = 0; i < 100; i++)
            {
                holder = RunPhase (holder);

                phases.Add (holder);
            }

            Console.WriteLine ("Part 1: " + string.Join ("", phases.Last ().Take (8)));

            var part2list = new List<int> ();

            for (int i = 0; i < 10000; i++)
            {
                part2list.AddRange (ints);
            }

            var offset = Int32.Parse (string.Join ("", ints.Take (7)));

            //5,977,737

            for (int i = 0; i < 100; i++)
            {
                part2list = part2 (part2list);
            }

            Console.WriteLine ("Part 2: " + String.Join ("", part2list.Skip (offset).Take (8)));

        }

        private static List<int> part2 (List<int> input)
        {
            List<int> b = new List<int> ();

            int acc = 0;
            for (int i = 0; i < input.Count; i++)
            {
                int n = input [input.Count - i - 1];
                acc += n;
                b.Add (acc % 10);
            }
            b.Reverse ();
            return b;

        }

        private static List<int> GenerateMask (int position, int length)
        {
            var base_pattern = new List<int> () { 0, 1, 0, -1 };
            var rtn = new List<int> ();
            for (int n = 0; n < base_pattern.Count (); n++)
            {
                for (int i = 0; i < position + 1; i++)
                {
                    rtn.Add (base_pattern [n]);
                }
            }

            if (rtn.Count != length)
            {
                var holder = new List<int> ();

                var length_diff = rtn.Count () - length + 1;

                if (length_diff > 0)
                {
                    //the pattern i have generated is more than i need. lop off the end

                    rtn = rtn.Take (length + 1).ToList ();
                }
                else
                {
                    //the pattern i have generated is less than i need. backfill the end

                    rtn = Backfill (rtn, length + 1);
                }

                return rtn.Skip (1).ToList ();
            }
            else
            {
                return rtn.Skip (1).ToList ();
            }

        }

        private static List<int> Backfill (List<int> mask, int count)
        {
            var filler = new List<int> ();

            for (int i = 0; i < count; i++)
            {
                filler.Add (mask [i % mask.Count]);
            }

            return filler;

        }

        private static List<int> RunPhase (List<int> input)
        {
            var rtn = new List<int> ();

            for (int i = 0; i < input.Count; i++)
            {
                rtn.Add (calc_for_position (input, i));
            }

            return rtn;
        }

        private static int calc_for_position (List<int> inputs, int position)
        {
            var rtn = 0;

            var holder = new List<int> ();

            var mask = GenerateMask (position, inputs.Count);

            for (int i = 0; i < inputs.Count; i++)
            {
                holder.Add ((inputs [i] * mask [i]));
            }

            rtn = holder.Sum ().Abs () % 10;

            return rtn;
        }

    }
}