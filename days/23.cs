using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using adv_of_code_2019.Classes;
using MoreLinq;
using static adv_of_code_2019.painter;
using static adv_of_code_2019.painter.SynchronousIntMachine;

namespace adv_of_code_2019
{
    //from https://github.com/sanraith/aoc2019/blob/16f0399740d1df581b150d6829a82646bbfadd84/aoc2019.Puzzles/Solutions/Day23.cs
    public static class Day23
    {
        public static async Task Run ()
        {
            var input = (await File.ReadAllTextAsync ("inputs\\23.txt"));

            var machinecount = 50;
            var addr = 255;

            var memory = IntMachineBase.ParseProgram (input);
            var machines = Enumerable.Range (0, machinecount).Select (_ => new SynchronousIntMachine (memory.ToArray ())).ToArray ();
            var queues = Enumerable.Range (0, machinecount).Select (_ => new Queue<long> ()).ToArray ();
            machines.WithIndex ().ForEach (m => m.Item.InputQueue.Enqueue (m.Index));

            while (true)
            {
                foreach (var machine in machines)
                {
                    foreach (var (address, _, y) in HandleOutgoingPackets (machine, queues, machinecount))
                    {
                        if (address == addr)
                        {
                            Console.WriteLine ("Part 1: " + y.ToString ());
                            goto part2;
                        }
                    }
                }
                HandleIncomingPackets (machines, queues);
            }

            part2:

                machines = Enumerable.Range (0, machinecount).Select (_ => new SynchronousIntMachine (memory.ToArray ())).ToArray ();
            queues = Enumerable.Range (0, machinecount).Select (_ => new Queue<long> ()).ToArray ();
            machines.WithIndex ().ForEach (m => m.Item.InputQueue.Enqueue (m.Index));

            (long X, long Y) natValue = default;
            long? lastNatDeliveredY = null;
            while (true)
            {
                var idleCount = 0;
                foreach (var machine in machines)
                {
                    if (machine.InputQueue.Count == 1 && machine.InputQueue.Single () == -1)
                    {
                        idleCount++;
                    }

                    foreach (var (address, x, y) in HandleOutgoingPackets (machine, queues, machinecount))
                    {
                        if (address == addr)
                        {
                            natValue = (x, y);
                        }
                    }
                }

                if (idleCount == machinecount)
                {
                    if (natValue.Y == lastNatDeliveredY) { break; }
                    queues [0].Enqueue (natValue.X);
                    queues [0].Enqueue (natValue.Y);
                    lastNatDeliveredY = natValue.Y;
                }

                HandleIncomingPackets (machines, queues);
            }

            Console.WriteLine ("Part 2: " + lastNatDeliveredY.ToString ());

        }

        private static IEnumerable < (int Address, long X, long Y) > HandleOutgoingPackets (SynchronousIntMachine machine, Queue<long> [] queues, int MachineCount)
        {
            while (machine.RunUntilBlockOrComplete () == ReturnCode.WrittenOutput)
            {
                var packet = ReadPacket (machine);
                var (address, x, y) = packet;
                if (address < MachineCount)
                {
                    queues [address].Enqueue (x);
                    queues [address].Enqueue (y);
                }
                yield return packet;
            }
        }

        private static void HandleIncomingPackets (SynchronousIntMachine [] machines, Queue<long> [] queues)
        {
            foreach (var (machine, index) in machines.WithIndex ())
            {
                var inputQueue = queues [index];
                if (inputQueue.Count == 0)
                {
                    machine.InputQueue.Enqueue (-1);
                }
                else
                {
                    while (inputQueue.Count > 0) { machine.InputQueue.Enqueue (inputQueue.Dequeue ()); }
                }
            }
        }

        private static (int Address, long X, long Y) ReadPacket (SynchronousIntMachine machine)
        {
            var o = machine.OutputQueue;
            while (o.Count < 3) { machine.RunUntilBlockOrComplete (); }

            return ((int) o.Dequeue (), o.Dequeue (), o.Dequeue ());
        }

        public static IEnumerable < (TItem Item, int Index) > WithIndex<TItem> (this IEnumerable<TItem> sequence) => sequence.Select ((x, i) => (x, i));

    }
}