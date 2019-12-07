using adv_of_code_2019.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2019
{
    public class Day7
    {
        public static async Task Run()
        {
            var inputs = await File.ReadAllTextAsync("inputs\\7.txt");

            day7vm[] vms = new day7vm[5];

            List<IEnumerable<int>> permutations = Enumerable.Range(0, 5).Permutations().ToList();

            Dictionary<IEnumerable<int>, int> results = new Dictionary<IEnumerable<int>, int>();

            for (int i = 0; i < 5; i++)
            {
                vms[i] = new day7vm()
                {
                    input_instructions = inputs
                };
            }

            foreach (var perm in permutations)
            {
                for (int i = 0; i < 5; i++)
                {
                    vms[i].Reset();

                    vms[i].input.Enqueue(perm.ToArray()[i]);
                }

                vms[0].input.Enqueue(0);

                await vms[0].Process();
                vms[1].input.Enqueue(vms[0].outputs.Last());

                await vms[1].Process();
                vms[2].input.Enqueue(vms[1].outputs.Last());

                await vms[2].Process();
                vms[3].input.Enqueue(vms[2].outputs.Last());

                await vms[3].Process();
                vms[4].input.Enqueue(vms[3].outputs.Last());

                await vms[4].Process();

                //vms[1].input.Enqueue({ await vms[0].Process(); yield vms[0].outputs.Last() });
                //vms[2].input.Enqueue(await vms[1].Process());
                //vms[3].input.Enqueue(await vms[2].Process());
                //vms[4].input.Enqueue(await vms[3].Process());

                results.Add(perm, vms[4].outputs.Last());
            }

            Console.WriteLine("Part 1:" + results.Max(e => e.Value));

            day7vm[] vms2 = new day7vm[5];

            List<IEnumerable<int>> permutations2 = Enumerable.Range(5, 5).Permutations().ToList();

            Dictionary<IEnumerable<int>, int> results2 = new Dictionary<IEnumerable<int>, int>();

            for (int i = 0; i < 5; i++)
            {
                vms2[i] = new day7vm()
                {
                    input_instructions = inputs
                };
            }

            foreach (var ordering in permutations2)
            {
                int[] phases = ordering.ToArray();

                for (var x = 0; x < 5; x++)
                {
                    vms2[x].Reset();

                    vms2[x].input.Enqueue(phases[x]);

                    vms2[x].name = ((char)(x + 65)).ToString();
                }

                vms2[0].input.Enqueue(0);

                Queue<day7vm> vmq = new Queue<day7vm>();

                for (int i = 0; i < 5; i++)
                {
                    vmq.Enqueue(vms2[i]);
                }

                while (!vmq.First(e => e.name == "E").stopped)
                {
                    var v = vmq.Dequeue();
                    await v.Process(vmq.Peek());
                    vmq.Enqueue(v);
                }

                results2.Add(ordering, vmq.First(e => e.name == "E").outputs.Last());
            }
            Console.WriteLine("Part 2:" + results2.Max(e => e.Value).ToString());
        }
    }
}