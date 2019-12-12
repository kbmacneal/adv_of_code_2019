using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace adv_of_code_2019.Classes
{

    public abstract class IntMachineBase
    {
        public static List<string> GetLines(string input)
        {
            return input.Replace("\r", string.Empty).Split('\n').Reverse().SkipWhile(string.IsNullOrEmpty).Reverse().ToList();
        }

        protected IntMachineBase(long[] memory)
        {
            myMemory = memory.Select((v, i) => (v, i)).ToDictionary(x => (long)x.i, x => x.v);
        }

        public static long[] ParseProgram(string input) => GetLines(input).First().Split(new[] { ',' }).Select(x => Convert.ToInt64(x)).ToArray();

        protected static (int OpCode, int[] ParameterModes) ParseInstruction(int instruction)
        {
            var opCode = instruction % 100;
            instruction /= 100;

            if (!ParameterCountsByOpCode.TryGetValue(opCode, out var parameterCount))
            {
                throw new InvalidOperationException($"Unknown operator: {opCode}");
            }
            var parameterModes = new int[parameterCount];
            var index = 0;
            while (instruction > 0)
            {
                parameterModes[index++] = instruction % 10;
                instruction /= 10;
            }

            return (opCode, parameterModes);
        }

        protected void ResolveParams(long opPos, int[] parameterModes, ref long[] rawParams, ref long[] resolvedParams)
        {
            var count = parameterModes.Length;
            for (var i = 0; i < count; i++)
            {
                rawParams[i] = myMemory[opPos + 1 + i];
            }

            for (var i = 0; i < count; i++)
            {
                switch (parameterModes[i])
                {
                    case 0:
                        resolvedParams[i] = myMemory.GetOrAdd(rawParams[i], _ => 0);
                        break;
                    case 1:
                        resolvedParams[i] = rawParams[i];
                        break;
                    case 2:
                        resolvedParams[i] = myMemory.GetOrAdd(rawParams[i] + myRelativeBase, _ => 0);
                        rawParams[i] = rawParams[i] + myRelativeBase;
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown parameter mode: {parameterModes[i]}");
                }
            }
        }

        protected static readonly Dictionary<int, int> ParameterCountsByOpCode = new Dictionary<int, int>
        {
            [1] = 3,
            [2] = 3,
            [3] = 1,
            [4] = 1,
            [5] = 2,
            [6] = 2,
            [7] = 3,
            [8] = 3,
            [9] = 1,
            [99] = 0
        };

        protected long myRelativeBase = 0;
        protected readonly Dictionary<long, long> myMemory;
    }
}
