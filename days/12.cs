using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using adv_of_code_2019.Classes;
using VectorAndPoint.ValTypes;

namespace adv_of_code_2019 {
    public class Day12 {
        private class moon : IEquatable<moon> {

            public Point3D Position { get; set; }
            public Vector3D Velocity { get; set; } = new Vector3D (0, 0, 0);
            public int p_energy { get; set; }
            public int k_energy { get; set; }

            public moon (int X, int Y, int Z) {
                this.Position = new Point3D (X, Y, Z);
            }

            public bool Equals (moon m) {
                return this.Position == m.Position && this.Velocity == m.Velocity;
            }
        }

        // <x=17, y=-12, z=13>
        // <x=2, y=1, z=1>
        // <x=-1, y=-17, z=7>
        // <x=12, y=-14, z=18>

        public static async Task Run () {
            List<moon> moons = RefreshMoons ();

            var stepcount = 1000;

            for (int i = 0; i < stepcount; i++) {
                StepVelo (ref moons);
            }
            // A moon's potential energy is the sum of the absolute values of its x, y, and z position coordinates. A moon's kinetic energy is the sum of the absolute values of its velocity coordinates.

            foreach (var moon in moons) {
                moon.p_energy = ((int) moon.Position.X).Abs () + ((int) moon.Position.Y).Abs () + ((int) moon.Position.Z).Abs ();

                moon.k_energy = ((int) moon.Velocity.X).Abs () + ((int) moon.Velocity.Y).Abs () + ((int) moon.Velocity.Z).Abs ();
            }

            var part1 = moons.Select (e => e.p_energy * e.k_energy).Sum ();

            Console.WriteLine ("Part 1: " + part1.ToString ());

            moons = RefreshMoons ();
            var orig_moons = RefreshMoons ();
            var intervals = new int[3];
            var completed = false;
            var c = 0;

            while (!completed) {

                StepVelo (ref moons);
                c++;

                for (var axis = 0; axis < 3; axis++) {
                    if (intervals[axis] == 0 && AreAxisStatesEqual (orig_moons.ToArray(), moons.ToArray(), (Axis) axis)) {
                        intervals[axis] = c;
                        if (intervals.All (x => x > 0)) {
                            completed = true;
                            break;
                        }
                    }
                }
            }
            var lcm = GetLeastCommonMultiple(intervals);
            Console.WriteLine("Part 2: " + lcm.ToString());
        }

        private static List<moon> RefreshMoons () {
            return new List<moon> () {
                new moon (17, -12, 13),
                    new moon (2, 1, 1),
                    new moon (-1, -17, 7),
                    new moon (12, -14, 18)
            };
        }

        private static void StepVelo (ref List<moon> moons) {
            var length = moons.Count;
            for (var indexA = 0; indexA < length; indexA++) {
                var moonA = moons[indexA];
                for (var indexB = indexA + 1; indexB < length; indexB++) {
                    var moonB = moons[indexB];
                    var diffX = moonA.Position.X.CompareTo (moonB.Position.X);
                    var diffY = moonA.Position.Y.CompareTo (moonB.Position.Y);
                    var diffZ = moonA.Position.Z.CompareTo (moonB.Position.Z);

                    var gravity = new Vector3D (diffX, diffY, diffZ);
                    moonA.Velocity += gravity * -1;
                    moonB.Velocity += gravity;
                }
                moonA.Position += moonA.Velocity;
            }
        }

        public static long GetLeastCommonMultiple (int[] elements) {
            long lcm = 1;
            var divisor = 2;
            while (true) {
                var counter = 0;
                var divisible = false;
                for (var i = 0; i < elements.Length; i++) {
                    if (elements[i] == 0) { return 0; } else if (elements[i] < 0) { elements[i] = elements[i] * (-1); }

                    if (elements[i] == 1) { counter++; }
                    if (elements[i] % divisor == 0) {
                        divisible = true;
                        elements[i] = elements[i] / divisor;
                    }
                }

                if (divisible) { lcm = lcm * divisor; } else { divisor++; }

                if (counter == elements.Length) { return lcm; }
            }
        }

        private static bool AreAxisStatesEqual (moon[] original, moon[] current, Axis axis) {
            var length = original.Length;
            for (var i = 0; i < length; i++) {
                bool falseCondition;
                switch (axis) {
                    case Axis.X:
                        falseCondition = original[i].Position.X != current[i].Position.X || original[i].Velocity.X != current[i].Velocity.X;
                        break;
                    case Axis.Y:
                        falseCondition = original[i].Position.Y != current[i].Position.Y || original[i].Velocity.Y != current[i].Velocity.Y;
                        break;
                    case Axis.Z:
                        falseCondition = original[i].Position.Z != current[i].Position.Z || original[i].Velocity.Z != current[i].Velocity.Z;
                        break;
                    default:
                        throw new InvalidOperationException ();
                }
                if (falseCondition) { return false; }
            }
            return true;
        }
        private enum Axis { X = 0, Y = 1, Z = 2 }
    }
}