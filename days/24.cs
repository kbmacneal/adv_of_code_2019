using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using adv_of_code_2019.Classes;
using MoreLinq;

namespace adv_of_code_2019
{
    public static class Day24
    {
        private static int Part2MinuteCount { get; set; } = 200;

        private static bool ShouldPrintSummary { get; set; } = false;

        public static async Task Run ()
        {
            var input = await File.ReadAllTextAsync ("inputs\\24.txt");

            var map = ParseMap (input);
            var mapAfter = new int [myMapSize];

            var ratings = new HashSet<int> ();
            while (ratings.Add (GetBiodiversityRating (map)))
            {
                for (var pos = 0; pos < myMapSize; pos++)
                {
                    var neighbourCount = 0;
                    var point = GetPoint (pos);
                    foreach (var direction in myDirections)
                    {
                        var nPoint = point + direction;
                        if (IsWithinBounds (nPoint) && map [GetPos (nPoint)] == 1) { neighbourCount++; }
                    }
                    mapAfter [pos] = GetNextPositionState (map, pos, neighbourCount);
                }
                (map, mapAfter) = (mapAfter, map);
            }

            Console.WriteLine ("Part 1: " + GetBiodiversityRating (map).ToString ());
            Console.WriteLine ("Part 2: " + await Part2Async (input));
        }

        public static async Task<string> Part2Async (string input)
        {
            var levels = new Dictionary<int, int []> {
                    [0] = ParseMap (input) };
            var levelsAfter = new Dictionary<int, int []> ();

            for (var minute = 0; minute < Part2MinuteCount; minute++)
            {
                // Add new levels if needed
                if (GetBiodiversityRating (levels [levels.Keys.Min ()]) != 0) { levels.Add (levels.Keys.Min () - 1, new int [myMapSize]); }
                if (GetBiodiversityRating (levels [levels.Keys.Max ()]) != 0) { levels.Add (levels.Keys.Max () + 1, new int [myMapSize]); }

                // Add temporary level containers
                levels.Keys.Except (levelsAfter.Keys).ForEach (k => levelsAfter [k] = new int [myMapSize]);

                // Calculate all levels
                foreach (var (levelIndex, map) in levels)
                {

                    var mapAfter = levelsAfter [levelIndex];
                    CalculatePositionsForLevel (levels, map, mapAfter, levelIndex);
                }

                (levels, levelsAfter) = (levelsAfter, levels);
            }

            var bugCount = levels.Values.SelectMany (x => x).Sum ();

            if (ShouldPrintSummary)
            {
                Console.WriteLine ();
                foreach (var (levelIndex, level) in levels.OrderBy (x => x.Key))
                {
                    Console.WriteLine ($"Depth {levelIndex}:");
                    Console.WriteLine (PrintMap (level));
                }
            }

            return bugCount.ToString ();
        }

        private static string PrintMap (int [] map)
        {
            var sb = new StringBuilder ();
            for (var y = 0; y < myHeight; y++)
            {
                for (var x = 0; x < myWidth; x++)
                {
                    if (x == 2 && y == 2 && map [GetPos (x, y)] == 0) { sb.Append ('?'); }
                    else { sb.Append (map [GetPos (x, y)] == 1 ? '#' : '.'); }
                }
                sb.AppendLine ();
            }
            return sb.ToString ();
        }

        private static void CalculatePositionsForLevel (Dictionary<int, int []> levels, int [] map, int [] mapAfter, int levelIndex)
        {
            for (var pos = 0; pos < myMapSize; pos++)
            {
                if (pos == myMiddlePos) { continue; } // Do not calculate a single tile for the inner level.

                int neighbourCount = CountRecursiveNeighbours (levels, map, levelIndex, pos);
                mapAfter [pos] = GetNextPositionState (map, pos, neighbourCount);
            }
        }

        private static int CountRecursiveNeighbours (Dictionary<int, int []> levels, int [] map, int levelIndex, int pos)
        {
            var neighbourCount = 0;
            var point = GetPoint (pos);
            foreach (var direction in myDirections)
            {
                var nPoint = point + direction;
                if (nPoint == myMiddlePoint)
                {
                    // Add inner level neighbours
                    if (levels.TryGetValue (levelIndex + 1, out var innerLevel))
                    {
                        var xMin = direction.X == 0 ? 0 : (direction.X < 0 ? myWidth - 1 : 0);
                        var xExclusive = direction.X == 0 ? myWidth : (direction.X < 0 ? myWidth : 1);
                        var yMin = direction.Y == 0 ? 0 : (direction.Y < 0 ? myHeight - 1 : 0);
                        var yExclusive = direction.Y == 0 ? myHeight : (direction.Y < 0 ? myHeight : 1);
                        for (var x = xMin; x < xExclusive; x++)
                        {
                            for (var y = yMin; y < yExclusive; y++)
                            {
                                neighbourCount += innerLevel [GetPos (x, y)] == 1 ? 1 : 0;
                            }
                        }
                    }
                }
                else if (!IsWithinBounds (nPoint))
                {
                    // Add outer level neighbour
                    if (levels.TryGetValue (levelIndex - 1, out var outerLevel))
                    {
                        var outerNeighbourPos = GetPos (myMiddlePoint + direction);
                        neighbourCount += outerLevel [outerNeighbourPos] == 1 ? 1 : 0;
                    }
                }
                else
                {
                    neighbourCount += map [GetPos (nPoint)] == 1 ? 1 : 0;
                }
            }

            return neighbourCount;
        }

        private static int GetNextPositionState (int [] map, int pos, int neighbourCount)
        {
            if (map [pos] == 1)
            {
                // A bug dies (becoming an empty space) unless there is exactly one bug adjacent to it.
                if (neighbourCount != 1) { return 0; }
                else { return 1; }
            }
            else
            {
                // An empty space becomes infested with a bug if exactly one or two bugs are adjacent to it.
                if (neighbourCount == 1 || neighbourCount == 2) { return 1; }
                else { return 0; }
            }
        }

        private static int GetBiodiversityRating (int [] map)
        {
            var result = 0;
            for (var i = 0; i < myMapSize; i++)
            {
                result += map [i] << i;
            }
            return result;
        }

        private static int [] ParseMap (string input)
        {
            var lines = GetLines (input);
            var map = lines.SelectMany (l => l).Select (c => c == '#' ? 1 : 0).ToArray ();

            myWidth = lines.First ().Length;
            myHeight = lines.Count;
            myMapSize = myWidth * myHeight;
            myMiddlePoint = new Point (myWidth / 2, myHeight / 2);
            myMiddlePos = GetPos (myMiddlePoint);

            return map;
        }

        public static List<string> GetLines (string input)
        {
            return input.Replace ("\r", string.Empty).Split ('\n').Reverse ().SkipWhile (string.IsNullOrEmpty).Reverse ().ToList ();
        }

        private static int GetPos (Point point) => GetPos (point.X, point.Y);
        private static int GetPos (int x, int y) => x + y * myWidth;
        private static Point GetPoint (int pos) => new Point (pos % myWidth, pos / myWidth);
        private static bool IsWithinBounds (Point p) => p.X >= 0 && p.X < myWidth && p.Y >= 0 && p.Y < myHeight;

        private static int myWidth;
        private static int myHeight;
        private static int myMapSize;
        private static Point myMiddlePoint;
        private static int myMiddlePos;
        private static readonly Point [] myDirections = new [] { new Point (0, -1), new Point (1, 0), new Point (0, 1), new Point (-1, 0) };
    }
}