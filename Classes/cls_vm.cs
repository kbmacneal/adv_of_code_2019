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
            int steps = 0;

            List<int> raw_input = input_instructions.Split(",").Select(e => Int32.Parse(e)).ToList();

            int code = 0;
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
}