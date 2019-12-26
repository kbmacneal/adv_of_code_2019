using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using adv_of_code_2019.Classes;

namespace adv_of_code_2019
{
    public class Day7
    {
        public static async Task Run ()
        {
            var inputs = await File.ReadAllTextAsync ("inputs\\7.txt");

            day7vm [] vms = new day7vm [5];

            List<IEnumerable<int>> permutations = Enumerable.Range (0, 5).Permutations ().ToList ();

            Dictionary<IEnumerable<int>, int> results = new Dictionary<IEnumerable<int>, int> ();

            for (int i = 0; i < 5; i++)
            {
                vms [i] = new day7vm ()
                {
                    input_instructions = inputs
                };
            }

            foreach (var perm in permutations)
            {
                for (int i = 0; i < 5; i++)
                {
                    vms [i].Reset ();

                    vms [i].input.Enqueue (perm.ToArray () [i]);
                }

                vms [0].input.Enqueue (0);

                await vms [0].Process ();
                vms [1].input.Enqueue (vms [0].outputs.Last ());

                await vms [1].Process ();
                vms [2].input.Enqueue (vms [1].outputs.Last ());

                await vms [2].Process ();
                vms [3].input.Enqueue (vms [2].outputs.Last ());

                await vms [3].Process ();
                vms [4].input.Enqueue (vms [3].outputs.Last ());

                await vms [4].Process ();

                //vms[1].input.Enqueue({ await vms[0].Process(); yield vms[0].outputs.Last() });
                //vms[2].input.Enqueue(await vms[1].Process());
                //vms[3].input.Enqueue(await vms[2].Process());
                //vms[4].input.Enqueue(await vms[3].Process());

                results.Add (perm, vms [4].outputs.Last ());
            }

            Console.WriteLine ("Part 1:" + results.Max (e => e.Value));

            day7vm [] vms2 = new day7vm [5];

            List<IEnumerable<int>> permutations2 = Enumerable.Range (5, 5).Permutations ().ToList ();

            Dictionary<IEnumerable<int>, int> results2 = new Dictionary<IEnumerable<int>, int> ();

            for (int i = 0; i < 5; i++)
            {
                vms2 [i] = new day7vm ()
                {
                    input_instructions = inputs
                };
            }

            foreach (var ordering in permutations2)
            {
                int [] phases = ordering.ToArray ();

                for (var x = 0; x < 5; x++)
                {
                    vms2 [x].Reset ();

                    vms2 [x].input.Enqueue (phases [x]);

                    vms2 [x].name = ((char) (x + 65)).ToString ();
                }

                vms2 [0].input.Enqueue (0);

                Queue<day7vm> vmq = new Queue<day7vm> ();

                for (int i = 0; i < 5; i++)
                {
                    vmq.Enqueue (vms2 [i]);
                }

                while (!vmq.First (e => e.name == "E").stopped)
                {
                    var v = vmq.Dequeue ();
                    await v.Process (vmq.Peek ());
                    vmq.Enqueue (v);
                }

                results2.Add (ordering, vmq.First (e => e.name == "E").outputs.Last ());
            }
            Console.WriteLine ("Part 2:" + results2.Max (e => e.Value).ToString ());
        }

        private class day7vm
        {
            public string name { get; set; }
            public string input_instructions { get; set; }
            public Queue<int> input { get; set; } = new Queue<int> ();
            public int [] state { get; set; }
            public List<int> outputs { get; set; } = new List<int> ();

            public bool stopped { get; private set; } = false;
            public bool paused { get; private set; } = false;

            public int paused_at { get; private set; } = 0;

            private const int ADD = 1;
            private const int MULT = 2;
            private const int INPUT = 3;
            private const int OUTPUT = 4;
            private const int JUMP_IF_TRUE = 5;
            private const int JUMP_IF_FALSE = 6;
            private const int LESS_THAN = 7;
            private const int EQUALS = 8;
            private const int END = 99;

            public void Reset ()
            {
                this.state = input_instructions.Split (",").Select (e => Int32.Parse (e)).ToArray ();

                this.input.Clear ();

                this.outputs.Clear ();

                this.paused = false;

                this.stopped = false;

                this.paused_at = 0;
            }

            public async Task Process ()
            {

                int opcode = 0;

                for (int i = paused_at; opcode != END && !this.paused;)
                {
                    int rawOpcode = state [i];
                    opcode = rawOpcode % 100;
                    bool isValueMode1 = (rawOpcode / 100) % 10 > 0;
                    bool isValueMode2 = (rawOpcode / 1000) % 10 > 0;
                    bool isValueMode3 = (rawOpcode / 10000) % 10 > 0;

                    var param1 = 0;
                    var param2 = 0;

                    switch (opcode)
                    {
                    case ADD:
                        param1 = isValueMode1 ? this.state [i + 1] : this.state [this.state [i + 1]];
                        param2 = isValueMode2 ? this.state [i + 2] : this.state [this.state [i + 2]];
                        this.state [this.state [i + 3]] = param1 + param2;
                        i += 4;
                        continue;

                    case MULT:
                        param1 = isValueMode1 ? this.state [i + 1] : this.state [this.state [i + 1]];
                        param2 = isValueMode2 ? this.state [i + 2] : this.state [this.state [i + 2]];
                        this.state [this.state [i + 3]] = param1 * param2;
                        i += 4;
                        continue;

                    case INPUT:
                        if (input.TryDequeue (out var input_dq))
                        {
                            this.state [this.state [i + 1]] = input_dq;
                            i += 2;
                            continue;
                        }
                        else
                        {
                            this.paused = true;
                            this.paused_at = i;
                            break;
                        }

                    case OUTPUT:
                        outputs.Add (this.state [this.state [i + 1]]);
                        i += 2;
                        continue;
                    case END:
                        this.stopped = true;
                        break;

                    case JUMP_IF_TRUE:
                        param1 = isValueMode1 ? this.state [i + 1] : this.state [this.state [i + 1]];
                        param2 = isValueMode2 ? this.state [i + 2] : this.state [this.state [i + 2]];
                        i = (param1 != 0) ? param2 : i + 3;
                        continue;
                    case JUMP_IF_FALSE:
                        param1 = isValueMode1 ? this.state [i + 1] : this.state [this.state [i + 1]];
                        param2 = isValueMode2 ? this.state [i + 2] : this.state [this.state [i + 2]];
                        i = (param1 == 0) ? param2 : i + 3;
                        continue;
                    case LESS_THAN:
                        param1 = isValueMode1 ? this.state [i + 1] : this.state [this.state [i + 1]];
                        param2 = isValueMode2 ? this.state [i + 2] : this.state [this.state [i + 2]];
                        this.state [this.state [i + 3]] = (param1 < param2) ? 1 : 0;
                        i += 4;
                        continue;
                    case EQUALS:
                        param1 = isValueMode1 ? this.state [i + 1] : this.state [this.state [i + 1]];
                        param2 = isValueMode2 ? this.state [i + 2] : this.state [this.state [i + 2]];
                        this.state [this.state [i + 3]] = (param1 == param2) ? 1 : 0;
                        i += 4;
                        continue;

                    default:
                        break;
                    }
                }
            }

            public async Task Process (day7vm nextvm)
            {

                int opcode = 0;

                this.paused = false;

                for (int i = this.paused_at; opcode != END && !this.paused;)
                {
                    int rawOpcode = state [i];
                    opcode = rawOpcode % 100;
                    bool isValueMode1 = (rawOpcode / 100) % 10 > 0;
                    bool isValueMode2 = (rawOpcode / 1000) % 10 > 0;
                    bool isValueMode3 = (rawOpcode / 10000) % 10 > 0;

                    var param1 = 0;
                    var param2 = 0;

                    switch (opcode)
                    {
                    case ADD:
                        param1 = isValueMode1 ? this.state [i + 1] : this.state [this.state [i + 1]];
                        param2 = isValueMode2 ? this.state [i + 2] : this.state [this.state [i + 2]];
                        this.state [this.state [i + 3]] = param1 + param2;
                        i += 4;
                        continue;

                    case MULT:
                        param1 = isValueMode1 ? this.state [i + 1] : this.state [this.state [i + 1]];
                        param2 = isValueMode2 ? this.state [i + 2] : this.state [this.state [i + 2]];
                        this.state [this.state [i + 3]] = param1 * param2;
                        i += 4;
                        continue;

                    case INPUT:
                        if (input.TryDequeue (out var input_dq))
                        {
                            this.state [this.state [i + 1]] = input_dq;
                            i += 2;
                            continue;
                        }
                        else
                        {
                            this.paused = true;
                            this.paused_at = i;
                            break;
                        }

                    case OUTPUT:
                        outputs.Add (this.state [this.state [i + 1]]);
                        nextvm.input.Enqueue (this.state [this.state [i + 1]]);
                        i += 2;
                        continue;
                    case END:
                        this.stopped = true;
                        break;

                    case JUMP_IF_TRUE:
                        param1 = isValueMode1 ? this.state [i + 1] : this.state [this.state [i + 1]];
                        param2 = isValueMode2 ? this.state [i + 2] : this.state [this.state [i + 2]];
                        i = (param1 != 0) ? param2 : i + 3;
                        continue;
                    case JUMP_IF_FALSE:
                        param1 = isValueMode1 ? this.state [i + 1] : this.state [this.state [i + 1]];
                        param2 = isValueMode2 ? this.state [i + 2] : this.state [this.state [i + 2]];
                        i = (param1 == 0) ? param2 : i + 3;
                        continue;
                    case LESS_THAN:
                        param1 = isValueMode1 ? this.state [i + 1] : this.state [this.state [i + 1]];
                        param2 = isValueMode2 ? this.state [i + 2] : this.state [this.state [i + 2]];
                        this.state [this.state [i + 3]] = (param1 < param2) ? 1 : 0;
                        i += 4;
                        continue;
                    case EQUALS:
                        param1 = isValueMode1 ? this.state [i + 1] : this.state [this.state [i + 1]];
                        param2 = isValueMode2 ? this.state [i + 2] : this.state [this.state [i + 2]];
                        this.state [this.state [i + 3]] = (param1 == param2) ? 1 : 0;
                        i += 4;
                        continue;

                    default:
                        break;
                    }
                }
            }

#pragma warning restore 1707
        }
    }
}