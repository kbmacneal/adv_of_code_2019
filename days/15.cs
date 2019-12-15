using adv_of_code_2019.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static adv_of_code_2019.painter;

namespace adv_of_code_2019
{
    public static class Day15
    {
        private enum Tile { Robot, Empty, Wall, OxygenSystem, Unknown }

        private static Dictionary<Classes.Point, Tile> Map { get; set; }
        private static HashSet<Classes.Point> Visited { get; set; }
        private static List<Classes.Point> PathToOxygenGenerator { get; set; }

        private static SynchronousIntMachine myIntMachine { get; set; }

        private static readonly Dictionary<int, Point> myDirections = new Dictionary<int, Point>
        {
            [1] = new Point(0, -1), // North
            [2] = new Point(0, 1),  // South
            [3] = new Point(-1, 0), // West
            [4] = new Point(1, 0)   // East
        };

        public static async Task Run()
        {
            var input = await File.ReadAllTextAsync("inputs\\15.txt");

            myIntMachine = new SynchronousIntMachine(input);
            Map = new Dictionary<Classes.Point, Tile>() { [new Classes.Point(0, 0)] = Tile.Empty };
            PathToOxygenGenerator = new List<Classes.Point>();
            await Backtrack(new Point(0, 0), null);

            Console.WriteLine("Part 1: " + PathToOxygenGenerator.Count().ToString());

            var o2gen = Map.First(x => x.Value == Tile.OxygenSystem).Key;
            var maxDistance = 0;
            Visited = new HashSet<Point>();
            var queue = new Queue<(Point p, int distance)>(new[] { (o2gen, 0) });
            while (queue.Count > 0)
            {
                var (pos, distance) = queue.Dequeue();
                if (!Visited.Add(pos)) { continue; }
                if (distance > maxDistance) { maxDistance = distance; }

                for (var directionCode = 1; directionCode <= 4; directionCode++)
                {
                    var direction = myDirections[directionCode];
                    var nextPos = pos + direction;
                    if (Visited.Contains(nextPos) || Map[nextPos] == Tile.Wall) { continue; }

                    queue.Enqueue((nextPos, distance + 1));
                }
            }

            Console.WriteLine("Part 2: " + maxDistance.ToString());
        }

        private static async Task<bool> Backtrack(Point pos, Point? backDirection)
        {
            var backDirectionCode = -1;
            var foundPath = false;
            for (var directionCode = 1; directionCode <= 4; directionCode++)
            {
                var direction = myDirections[directionCode];
                if (direction == backDirection) { backDirectionCode = directionCode; continue; }

                var nextPos = pos + direction;
                if (Map.ContainsKey(nextPos)) { continue; }

                long tileCode = Step(directionCode);
                switch (tileCode)
                {
                    case 0:
                        Map[nextPos] = Tile.Wall;
                        break;

                    case 1:
                        Map[nextPos] = Tile.Empty;
                        if (await Backtrack(nextPos, direction * -1))
                        {
                            PathToOxygenGenerator.Add(pos);
                            foundPath = true;
                        }
                        break;

                    case 2:
                        Map[nextPos] = Tile.OxygenSystem;
                        PathToOxygenGenerator.Add(pos);
                        foundPath = true;
                        await Backtrack(nextPos, direction * -1);
                        break;
                }
            }

            if (backDirection != null)
            {
                Step(backDirectionCode);
            }

            return foundPath;
        }

        private static long Step(int directionCode)
        {
            myIntMachine.InputQueue.Enqueue(directionCode);
            myIntMachine.RunUntilBlockOrComplete();
            var tileCode = myIntMachine.OutputQueue.Dequeue();

            return tileCode;
        }
    }
}