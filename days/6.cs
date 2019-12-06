using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2019
{
    public class Day6
    {
#pragma warning disable 1998

        private class orbit
        {
            public string name { get; set; }
            public orbit parent { get; set; }
            public List<orbit> child_orbits { get; set; }

            public orbit(string orbitName, string parentName)
            {
                name = orbitName;
                child_orbits = new List<orbit>();

                if (!string.IsNullOrEmpty(parentName))
                {
                    parent = new orbit(parentName, null);
                }
            }

            public void assignallchildren(List<orbit> orbits)
            {
                child_orbits = orbits.Where(x => x.parent.name == this.name).ToList();
                foreach (var child in child_orbits)
                {
                    child.parent = this;
                    child.assignallchildren(orbits);
                }
            }

            public int count_orbits(int parentOrbits = default)
            {
                var count = 0;
                foreach (var childOrbit in this.child_orbits)
                {
                    count += parentOrbits + 1;
                    count += childOrbit.count_orbits(parentOrbits + 1);
                }
                return count;
            }

            public orbit FindChildren(string childName)
            {
                orbit result = null;
                foreach (var child in this.child_orbits)
                {
                    if (child.name == childName)
                    {
                        result = child;
                        break;
                    }
                    else if (child.child_orbits.Any())
                    {
                        result = child.FindChildren(childName);
                    }
                    if (result != null)
                    {
                        return result;
                    }
                }
                return result;
            }
        }

        private class OrbitRelation
        {
            public OrbitRelation(string target, string orbiter)
            {
                Target = target;
                Orbiter = orbiter;
            }

            public string Target { get; set; }

            public string Orbiter { get; set; }
        }

        private static orbit GenerateOrbitList(List<OrbitRelation> relations)
        {
            var allOrbits = new List<orbit>();
            foreach (var relation in relations)
            {
                if (allOrbits.Any(x => x.name == relation.Orbiter))
                {
                    throw new Exception("Orbiter already has a parent");
                }
                allOrbits.Add(new orbit(relation.Orbiter, relation.Target));
            }

            var rootOrbit = new orbit("COM", null);
            rootOrbit.assignallchildren(allOrbits);
            return rootOrbit;
        }

        public static async Task Run()
        {
            var inputs = await File.ReadAllLinesAsync("inputs\\6.txt");

            var relations = inputs.ToList().Select(e => e.Split(")")).Select(e => new OrbitRelation(e[0], e[1]));

            var com = GenerateOrbitList(relations.ToList());

            var count = com.count_orbits();

            Console.WriteLine("Part 1:" + count.ToString());

            var santa = com.FindChildren("SAN");
            var santasWay = new List<string>();
            while (santa.parent != null)
            {
                santa = santa.parent;
                santasWay.Add(santa.name);
            }

            var you = com.FindChildren("YOU");
            var yourWay = new List<string>();
            while (you.parent != null)
            {
                you = you.parent;
                yourWay.Add(you.name);
            }

            for (var i = 0; i < yourWay.Count; i++)
            {
                if (santasWay.Any(x => x == yourWay[i]))
                {
                    Console.WriteLine((i + santasWay.IndexOf(yourWay[i])).ToString());
                    break;
                }
            }
        }
    }
}