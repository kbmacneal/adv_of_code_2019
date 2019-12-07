using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2019.Classes
{
#pragma warning disable 1707

    public class day5vm
    {
        public string input_instructions { get; set; }
        public int input { get; set; }

        private const int ADD = 1;
        private const int MULT = 2;
        private const int INPUT = 3;
        private const int OUTPUT = 4;
        private const int JUMP_IF_TRUE = 5;
        private const int JUMP_IF_FALSE = 6;
        private const int LESS_THAN = 7;
        private const int EQUALS = 8;
        private const int END = 99;

        public async Task<int> Process()
        {
            List<int> raw_input = input_instructions.Split(",").Select(e => Int32.Parse(e)).ToList();

            int opcode = 0;

            for (int i = 0; opcode != END;)
            {
                int rawOpcode = raw_input[i];
                opcode = rawOpcode % 100;
                bool isValueMode1 = (rawOpcode / 100) % 10 > 0;
                bool isValueMode2 = (rawOpcode / 1000) % 10 > 0;
                bool isValueMode3 = (rawOpcode / 10000) % 10 > 0;

                var param1 = 0;
                var param2 = 0;

                switch (opcode)
                {
                    case ADD:
                        param1 = isValueMode1 ? raw_input[i + 1] : raw_input[raw_input[i + 1]];
                        param2 = isValueMode2 ? raw_input[i + 2] : raw_input[raw_input[i + 2]];
                        raw_input[raw_input[i + 3]] = param1 + param2;
                        i += 4;
                        continue;

                    case MULT:
                        param1 = isValueMode1 ? raw_input[i + 1] : raw_input[raw_input[i + 1]];
                        param2 = isValueMode2 ? raw_input[i + 2] : raw_input[raw_input[i + 2]];
                        raw_input[raw_input[i + 3]] = param1 * param2;
                        i += 4;
                        continue;

                    case INPUT:
                        raw_input[raw_input[i + 1]] = input;
                        i += 2;
                        continue;

                    case OUTPUT:
                        Console.WriteLine(raw_input[raw_input[i + 1]]);
                        i += 2;
                        continue;

                    case JUMP_IF_TRUE:
                        param1 = isValueMode1 ? raw_input[i + 1] : raw_input[raw_input[i + 1]];
                        param2 = isValueMode2 ? raw_input[i + 2] : raw_input[raw_input[i + 2]];
                        i = (param1 != 0) ? param2 : i + 3;
                        continue;
                    case JUMP_IF_FALSE:
                        param1 = isValueMode1 ? raw_input[i + 1] : raw_input[raw_input[i + 1]];
                        param2 = isValueMode2 ? raw_input[i + 2] : raw_input[raw_input[i + 2]];
                        i = (param1 == 0) ? param2 : i + 3;
                        continue;
                    case LESS_THAN:
                        param1 = isValueMode1 ? raw_input[i + 1] : raw_input[raw_input[i + 1]];
                        param2 = isValueMode2 ? raw_input[i + 2] : raw_input[raw_input[i + 2]];
                        raw_input[raw_input[i + 3]] = (param1 < param2) ? 1 : 0;
                        i += 4;
                        continue;
                    case EQUALS:
                        param1 = isValueMode1 ? raw_input[i + 1] : raw_input[raw_input[i + 1]];
                        param2 = isValueMode2 ? raw_input[i + 2] : raw_input[raw_input[i + 2]];
                        raw_input[raw_input[i + 3]] = (param1 == param2) ? 1 : 0;
                        i += 4;
                        continue;

                    default:
                        break;
                }
            }
            return 9999;
        }

#pragma warning restore 1707
    }

    public class day7vm
    {
        public string name { get; set; }
        public string input_instructions { get; set; }
        public Queue<int> input { get; set; } = new Queue<int>();
        public int[] state { get; set; }
        public List<int> outputs { get; set; } = new List<int>();

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

        public void Reset()
        {
            this.state = input_instructions.Split(",").Select(e => Int32.Parse(e)).ToArray();

            this.input.Clear();

            this.outputs.Clear();

            this.paused = false;

            this.stopped = false;

            this.paused_at = 0;
        }

        public async Task Process()
        {
            //List<int> raw_input = input_instructions.Split(",").Select(e => Int32.Parse(e)).ToList();

            //if(this.state == null) this.state = raw_input.ToArray();           

            int opcode = 0;

            for (int i = paused_at; opcode != END && !this.paused;)
            {
                int rawOpcode = state[i];
                opcode = rawOpcode % 100;
                bool isValueMode1 = (rawOpcode / 100) % 10 > 0;
                bool isValueMode2 = (rawOpcode / 1000) % 10 > 0;
                bool isValueMode3 = (rawOpcode / 10000) % 10 > 0;

                var param1 = 0;
                var param2 = 0;

                switch (opcode)
                {
                    case ADD:
                        param1 = isValueMode1 ? this.state[i + 1] : this.state[this.state[i + 1]];
                        param2 = isValueMode2 ? this.state[i + 2] : this.state[this.state[i + 2]];
                        this.state[this.state[i + 3]] = param1 + param2;
                        i += 4;
                        continue;

                    case MULT:
                        param1 = isValueMode1 ? this.state[i + 1] : this.state[this.state[i + 1]];
                        param2 = isValueMode2 ? this.state[i + 2] : this.state[this.state[i + 2]];
                        this.state[this.state[i + 3]] = param1 * param2;
                        i += 4;
                        continue;

                    case INPUT:
                        if(input.TryDequeue(out var input_dq))
                        {
                            this.state[this.state[i + 1]] = input_dq;
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
                        outputs.Add(this.state[this.state[i + 1]]);
                        i += 2;
                        continue;
                    case END:
                        this.stopped = true;
                        break;

                    case JUMP_IF_TRUE:
                        param1 = isValueMode1 ? this.state[i + 1] : this.state[this.state[i + 1]];
                        param2 = isValueMode2 ? this.state[i + 2] : this.state[this.state[i + 2]];
                        i = (param1 != 0) ? param2 : i + 3;
                        continue;
                    case JUMP_IF_FALSE:
                        param1 = isValueMode1 ? this.state[i + 1] : this.state[this.state[i + 1]];
                        param2 = isValueMode2 ? this.state[i + 2] : this.state[this.state[i + 2]];
                        i = (param1 == 0) ? param2 : i + 3;
                        continue;
                    case LESS_THAN:
                        param1 = isValueMode1 ? this.state[i + 1] : this.state[this.state[i + 1]];
                        param2 = isValueMode2 ? this.state[i + 2] : this.state[this.state[i + 2]];
                        this.state[this.state[i + 3]] = (param1 < param2) ? 1 : 0;
                        i += 4;
                        continue;
                    case EQUALS:
                        param1 = isValueMode1 ? this.state[i + 1] : this.state[this.state[i + 1]];
                        param2 = isValueMode2 ? this.state[i + 2] : this.state[this.state[i + 2]];
                        this.state[this.state[i + 3]] = (param1 == param2) ? 1 : 0;
                        i += 4;
                        continue;

                    default:
                        break;
                }
            }
        }

        public async Task Process(day7vm nextvm)
        {
            //List<int> raw_input = input_instructions.Split(",").Select(e => Int32.Parse(e)).ToList();

            //if(this.state == null) this.state = raw_input.ToArray();           

            int opcode = 0;

            this.paused = false;

            for (int i = this.paused_at; opcode != END && !this.paused;)
            {
                int rawOpcode = state[i];
                opcode = rawOpcode % 100;
                bool isValueMode1 = (rawOpcode / 100) % 10 > 0;
                bool isValueMode2 = (rawOpcode / 1000) % 10 > 0;
                bool isValueMode3 = (rawOpcode / 10000) % 10 > 0;

                var param1 = 0;
                var param2 = 0;

                switch (opcode)
                {
                    case ADD:
                        param1 = isValueMode1 ? this.state[i + 1] : this.state[this.state[i + 1]];
                        param2 = isValueMode2 ? this.state[i + 2] : this.state[this.state[i + 2]];
                        this.state[this.state[i + 3]] = param1 + param2;
                        i += 4;
                        continue;

                    case MULT:
                        param1 = isValueMode1 ? this.state[i + 1] : this.state[this.state[i + 1]];
                        param2 = isValueMode2 ? this.state[i + 2] : this.state[this.state[i + 2]];
                        this.state[this.state[i + 3]] = param1 * param2;
                        i += 4;
                        continue;

                    case INPUT:
                        if (input.TryDequeue(out var input_dq))
                        {
                            this.state[this.state[i + 1]] = input_dq;
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
                        outputs.Add(this.state[this.state[i + 1]]);
                        nextvm.input.Enqueue(this.state[this.state[i + 1]]);
                        i += 2;
                        continue;
                    case END:
                        this.stopped = true;
                        break;

                    case JUMP_IF_TRUE:
                        param1 = isValueMode1 ? this.state[i + 1] : this.state[this.state[i + 1]];
                        param2 = isValueMode2 ? this.state[i + 2] : this.state[this.state[i + 2]];
                        i = (param1 != 0) ? param2 : i + 3;
                        continue;
                    case JUMP_IF_FALSE:
                        param1 = isValueMode1 ? this.state[i + 1] : this.state[this.state[i + 1]];
                        param2 = isValueMode2 ? this.state[i + 2] : this.state[this.state[i + 2]];
                        i = (param1 == 0) ? param2 : i + 3;
                        continue;
                    case LESS_THAN:
                        param1 = isValueMode1 ? this.state[i + 1] : this.state[this.state[i + 1]];
                        param2 = isValueMode2 ? this.state[i + 2] : this.state[this.state[i + 2]];
                        this.state[this.state[i + 3]] = (param1 < param2) ? 1 : 0;
                        i += 4;
                        continue;
                    case EQUALS:
                        param1 = isValueMode1 ? this.state[i + 1] : this.state[this.state[i + 1]];
                        param2 = isValueMode2 ? this.state[i + 2] : this.state[this.state[i + 2]];
                        this.state[this.state[i + 3]] = (param1 == param2) ? 1 : 0;
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