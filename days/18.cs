using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using adv_of_code_2019.Classes;

namespace adv_of_code_2019
{
    // yh, not gonna do this one. code from https://github.com/mareklinka/aoc-2019/tree/master/18 
    public static class Day18
    {
        private class ReachableKey
        {
            public char Key { get; set; }

            public int Distance { get; set; }

            public int Obstacles { get; set; }
        }

        private struct State
        {
            public PSet Positions { get; set; }

            public int OwnedKeys { get; set; }

            public int Steps { get; set; }
        }

        public static async Task Run ()
        {

            var lines = await File.ReadAllLinesAsync ("inputs\\18.txt");

            var keys = lines.SelectMany (_ => _.Where (char.IsLower)).ToList ();

            var dictionary = new Dictionary<P, List<ReachableKey>> ();

            dictionary [FindPositionOf ('@', lines)] = ReachableKeys (lines, FindPositionOf ('@', lines), string.Empty);

            foreach (var k in keys)
            {
                dictionary [FindPositionOf (k, lines)] = ReachableKeys (lines, FindPositionOf (k, lines), string.Empty);
            }

            var minimumSteps = CollectKeys (lines, dictionary, GetPositions (lines, keys), new [] { '@' });

            Console.WriteLine ("Part 1: " + minimumSteps.ToString ());

            Console.WriteLine("Part 2: " + await Part2());

        }

        private static async Task<int> Part2 ()
        {
            var lines = await File.ReadAllLinesAsync ("inputs\\18_2.txt");

            var keys = lines.SelectMany (_ => _.Where (char.IsLower)).ToList ();

            var dictionary = new Dictionary<P, List<ReachableKey>> ();

            for (var i = '1'; i <= '4'; i++)
            {
                dictionary [FindPositionOf (i, lines)] = ReachableKeys (lines, FindPositionOf (i, lines), string.Empty);
            }

            foreach (var k in keys)
            {
                dictionary [FindPositionOf (k, lines)] = ReachableKeys (lines, FindPositionOf (k, lines), string.Empty);
            }

            var minimumSteps = CollectKeys (lines, dictionary, GetPositions (lines, keys), new [] { '1', '2', '3', '4' });

            return minimumSteps;
        }

        private static Dictionary<char, P> GetPositions (string [] map, List<char> keys)
        {
            var dict = new Dictionary<char, P> ();

            foreach (var k in keys)
            {
                dict.Add (k, FindPositionOf (k, map));
            }

            return dict;
        }

        private static int CollectKeys (string [] map, Dictionary<P, List<ReachableKey>> keyPaths, Dictionary<char, P> positions, char [] robots)
        {
            var pos = robots.Select (c => FindPositionOf (c, map)).ToArray ();
            var currentMinimum = int.MaxValue;

            var startingSet = new PSet ();
            for (var index = 0; index < pos.Length; index++)
            {
                var p = pos [index];
                startingSet [index + 1] = p;
            }

            var q = new Queue<State> ();
            q.Enqueue (new State { Positions = startingSet, OwnedKeys = 0 });

            var visited = new Dictionary < (PSet, int),
                int > ();
            var finishValue = 0;
            for (var i = 0; i < positions.Count; ++i)
            {
                finishValue |= (int) Math.Pow (2, i);
            }

            while (q.Any ())
            {
                var state = q.Dequeue ();

                var valueTuple = (state.Positions, state.OwnedKeys);
                if (visited.TryGetValue (valueTuple, out var steps))
                {
                    if (steps <= state.Steps)
                    {
                        continue;
                    }

                    // this is the crucial bit
                    // if the current state is a better path to a known state, update -
                    // this will cull more future paths, leading to faster convergence
                    visited [valueTuple] = state.Steps;
                }
                else
                {
                    visited.Add (valueTuple, state.Steps);
                }

                if (state.OwnedKeys == finishValue)
                {
                    currentMinimum = Math.Min (currentMinimum, state.Steps);
                    continue;
                }

                for (int i = 1; i <= robots.Length; i++)
                {
                    foreach (var k in keyPaths [state.Positions [i]])
                    {
                        var ki = (int) Math.Pow (2, k.Key - 'a');
                        if ((state.OwnedKeys & ki) == ki || (k.Obstacles & state.OwnedKeys) != k.Obstacles)
                        {
                            continue;
                        }

                        var newOwned = state.OwnedKeys | ki;

                        var newPos = state.Positions.Clone ();
                        newPos [i] = positions [k.Key];
                        q.Enqueue (new State
                        {
                            Positions = newPos,
                                OwnedKeys = newOwned,
                                Steps = state.Steps + k.Distance
                        });
                    }
                }
            }

            return currentMinimum;
        }

        private static P FindPositionOf (char c, string [] map)
        {
            var startingLine = map.Single (_ => _.Contains (c));
            var startingColumn = startingLine.IndexOf (c);
            var startingRow = Array.IndexOf (map, startingLine);

            return new P { X = startingColumn, Y = startingRow };
        }

        private static List<ReachableKey> ReachableKeys (string [] map, P start, string currentKeys)
        {
            var list = new List<ReachableKey> ();
            var visited = new HashSet<P> ();

            var q = new Queue<P> ();
            var s = new Queue<int> ();
            var o = new Queue<int> ();
            q.Enqueue (start);
            s.Enqueue (0);
            o.Enqueue (0);

            while (q.Any ())
            {
                var pos = q.Dequeue ();
                var dist = s.Dequeue ();
                var obst = o.Dequeue ();

                if (visited.Contains (pos))
                {
                    continue;
                }

                visited.Add (pos);

                var c = map [pos.Y] [pos.X];

                if (c == '@' || c == '1' || c == '2' || c == '3' || c == '4')
                {
                    c = '.';
                }

                if (char.IsLower (c))
                {
                    list.Add (new ReachableKey { Distance = dist, Key = c, Obstacles = obst });

                    foreach (var p in pos.Around ())
                    {
                        q.Enqueue (p);
                        s.Enqueue (dist + 1);
                        o.Enqueue (obst);
                    }
                }
                else if (char.IsUpper (c))
                {
                    foreach (var p in pos.Around ())
                    {
                        q.Enqueue (p);
                        s.Enqueue (dist + 1);
                        o.Enqueue (obst |= (int) Math.Pow (2, (char.ToLower (c) - 'a')));
                    }
                }
                else if (c == '.')
                {
                    foreach (var p in pos.Around ())
                    {
                        q.Enqueue (p);
                        s.Enqueue (dist + 1);
                        o.Enqueue (obst);
                    }
                }
            }

            return list;
        }
    }
}