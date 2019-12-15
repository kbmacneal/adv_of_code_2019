using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VectorAndPoint.ValTypes;
using static adv_of_code_2019.painter;
using static adv_of_code_2019.painter.SynchronousIntMachine;

namespace adv_of_code_2019
{
    public static class Day13
    {
        private enum tile_type
        {
            Empty = 0,
            Wall = 1,
            Block = 2,
            HorizPaddle = 3,
            Ball = 4
        }

        public static async Task Run()
        {
            var input = await File.ReadAllTextAsync("inputs\\13.txt");

            Dictionary<PointInt, tile_type> tiles = new Dictionary<PointInt, tile_type>();

            var intMachine = new SynchronousIntMachine(input);

            while (intMachine.RunUntilBlockOrComplete() == ReturnCode.WrittenOutput)
            {
                while (intMachine.OutputQueue.Count < 3) { intMachine.RunUntilBlockOrComplete(); }
                var x = (int)intMachine.OutputQueue.Dequeue();
                var y = (int)intMachine.OutputQueue.Dequeue();
                var tile = intMachine.OutputQueue.Dequeue();

                tiles[new PointInt(x, y)] = (tile_type)tile;
            }

            var blocks = tiles.Where(e => e.Value == tile_type.Block).Count();

            Console.WriteLine("Part 1: " + blocks.ToString());

            intMachine = new SynchronousIntMachine(input);

            intMachine.SetMemoryRegister(0, 2);

            long score = 0;
            var blockCount = blocks;
            var ball = Classes.Point.Empty;
            var paddle = Classes.Point.Empty;
            ReturnCode returnCode;
            while (blockCount > -1 && (returnCode = intMachine.RunUntilBlockOrComplete()) != ReturnCode.Completed)
            {
                switch (returnCode)
                {
                    case ReturnCode.WaitingForInput:
                        if (blockCount == 0) { blockCount = -1; break; }
                        var joystickInput = ball.X.CompareTo(paddle.X);
                        intMachine.InputQueue.Enqueue(joystickInput);
                        break;

                    case ReturnCode.WrittenOutput:
                        while (intMachine.OutputQueue.Count < 3) { intMachine.RunUntilBlockOrComplete(); }
                        var x = (int)intMachine.OutputQueue.Dequeue();
                        var y = (int)intMachine.OutputQueue.Dequeue();
                        var t = intMachine.OutputQueue.Dequeue();

                        if (x == -1)
                        {
                            score = t;
                        }
                        else
                        {
                            var tile = (tile_type)t;
                            if (tile != tile_type.Block && tiles[new PointInt(x, y)] == tile_type.Block) { blockCount--; }
                            tiles[new PointInt(x, y)] = tile;

                            if (tile == tile_type.Ball)
                            {
                                ball = new Classes.Point(x, y);
                            }
                            else if (tile == tile_type.HorizPaddle)
                            {
                                paddle = new Classes.Point(x, y);
                            }
                        }
                        break;
                }
            }

            Console.WriteLine("Part 2: " + score);
        }
    }
}