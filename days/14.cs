using adv_of_code_2019.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2019
{
    public class Day14
    {
        public static async Task Run()
        {

            //from https://github.com/tslater2006/AdventOfCode2019 , this one was hurting my brain
            var refinery = new Refinery(await File.ReadAllLinesAsync("inputs\\14.txt"));

            refinery.ProduceMaterial(new RefineryProduction() { Amount = 1, Chemical = "FUEL" });

            Console.WriteLine("Part 1: " + refinery.OreRequired.ToString());

            refinery = new Refinery(await File.ReadAllLinesAsync("inputs\\14.txt"));
            long fuelCount = 0;
            var productionFactor = 10000;
            Dictionary<string, int> oldSurplus = null;
            long oldOreRequired = 0;
            while (productionFactor >= 1)
            {
                while (refinery.OreRequired < 1000000000000)
                {
                    oldSurplus = new Dictionary<string, int>(refinery.Surplus);
                    oldOreRequired = refinery.OreRequired;
                    refinery.ProduceMaterial(new RefineryProduction() { Amount = productionFactor, Chemical = "FUEL" });
                    fuelCount += productionFactor;
                }

                /* we've gone over... */
                if (productionFactor >= 1)
                {
                    /*reset old state*/
                    refinery.Surplus = new Dictionary<string, int>(oldSurplus);
                    refinery.OreRequired = oldOreRequired;
                    fuelCount -= productionFactor;
                    productionFactor /= 10;
                }
            }
            Console.WriteLine("Part 2: " + fuelCount.ToString());
        }
    }
}