using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2019
{
    public class Day1
    {
        private static decimal part_2_total = 0M;

        public static async Task Run ()
        {
            List<Decimal> summer = new List<Decimal> ();
            List<int> inputs = (await File.ReadAllLinesAsync ("inputs/1.txt")).Select (Int32.Parse).ToList ();

            inputs.ForEach (e =>
            {
                summer.Add (getfuel (e));
            });

            Console.WriteLine ("Part 1: " + summer.Sum ().ToString ());

            inputs = (await File.ReadAllLinesAsync ("inputs/1_2.txt")).Select (Int32.Parse).ToList ();

            inputs.ForEach (e =>
            {
                getfuelrecurse (e);
            });

            Console.WriteLine ("Part 2: " + part_2_total.ToString ());
        }

        public static decimal getfuel (decimal mass)
        {
            return Math.Floor (mass / 3) - 2;
        }

        public static decimal getfuelrecurse (decimal mass)
        {
            var needed = getfuel (mass);

            if (needed <= 0)
            {
                return needed;
            }
            else
            {
                part_2_total += needed;
                getfuelrecurse (needed);
            }

            return 0M;
        }
    }
}