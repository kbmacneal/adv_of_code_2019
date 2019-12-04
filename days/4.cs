using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2019 {
    public class Day4 {
        #pragma warning disable 1998
        public static async Task Run () {
            int start = 136818;
            int stop = 685979;

            var answer = Enumerable.Range(start,stop-start).ToList().Where(e=>CheckAdjacency(e) && CheckIncreasing(e)).Count();

            Console.WriteLine("Part 1: "+ answer);
            
            var answer2 = Enumerable.Range(start,stop-start).ToList().Where(e=>CheckAdjacency(e) && CheckIncreasing(e) && CheckOnlyTwo(e)).Count();

            Console.WriteLine("Part 2: "+ answer2);
        }
        #pragma warning restore 1998
        private static bool CheckAdjacency(int num)
        {
            List<int> checker = num.ToString().Select(e=>Int32.Parse(e.ToString())).ToList();

            for (int i = 0; i < checker.Count-1; i++)
            {
                if(checker[i]==checker[i+1])
                {
                    return true;
                }
            }

            return false;
        }

        private static bool CheckOnlyTwo(int num)
        {            
            return num.ToString().GroupBy(e=>e).Any(e=>e.Count()==2);
        }
        private static bool CheckIncreasing(int num)
        {
            List<int> checker = num.ToString().Select(e=>Int32.Parse(e.ToString())).ToList();

            for (int i = 0; i < checker.Count-1; i++)
            {
                if(checker[i] > checker[i+1])
                {
                    return false;
                }
            }

            return true;
        }

    }
}