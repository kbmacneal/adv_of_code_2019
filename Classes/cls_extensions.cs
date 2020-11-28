using System;
using System.Collections.Generic;
using System.Linq;

namespace adv_of_code_.Classes
{
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Permutations<T> (this IEnumerable<T> values)
        {
            if (values.Count () == 1)
                return new [] { values };
            return values.SelectMany (v => Permutations (values.Where (x => x.Equals (v) == false)), (v, p) => p.Prepend (v));
        }

        public static Int32 Abs (this Int32 num)
        {
            return Math.Abs (num);
        }

        public static void ReplaceKey<T, U> (this Dictionary<T, U> source, T key, T newKey)
        {
            if (!source.TryGetValue (key, out var value))
                throw new ArgumentException ("Key does not exist", nameof (key));
            source.Remove (key);
            source.Add (newKey, value);
        }

        public static void ReplaceValue<T, U> (this Dictionary<T, U> source, T key, U newVal)
        {
            source.Remove (key);
            source.Add (key, newVal);
        }

        public static TValue GetOrAdd<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueGenerator)
        {
            if (!dictionary.TryGetValue (key, out var value))
            {
                value = valueGenerator (key);
                dictionary.Add (key, value);
            }

            return value;
        }

        public static IEnumerable<int> CumulativeSum (this IEnumerable<int> sequence)
        {
            int sum = 0;
            foreach (var item in sequence)
            {
                sum += item;
                yield return sum;
            }
        }

        public static int Floor (this Decimal d)
        {
            return (int) Math.Floor (d);
        }

        public static int Floor (this Double d)
        {
            return (int) Math.Floor (d);
        }

        public static int ManhDist (this Point p)
        {
            return p.X.Abs () + p.Y.Abs ();
        }

        //from https://www.geeksforgeeks.org/lcm-of-given-array-elements/
        public static long GetLCM (this int [] element_array)
        {
            long lcm_of_array_elements = 1;
            int divisor = 2;

            while (true)
            {
                int counter = 0;
                bool divisible = false;
                for (int i = 0; i < element_array.Length; i++)
                {
                    // lcm_of_array_elements (n1, n2, ... 0) = 0.
                    // For negative number we convert into
                    // positive and calculate lcm_of_array_elements.
                    if (element_array [i] == 0)
                    {
                        return 0;
                    }
                    else if (element_array [i] < 0)
                    {
                        element_array [i] = element_array [i] * (-1);
                    }
                    if (element_array [i] == 1)
                    {
                        counter++;
                    }

                    // Divide element_array by devisor if complete
                    // division i.e. without remainder then replace
                    // number with quotient; used for find next factor
                    if (element_array [i] % divisor == 0)
                    {
                        divisible = true;
                        element_array [i] = element_array [i] / divisor;
                    }
                }

                // If divisor able to completely divide any number
                // from array multiply with lcm_of_array_elements
                // and store into lcm_of_array_elements and continue
                // to same divisor for next factor finding.
                // else increment divisor
                if (divisible)
                {
                    lcm_of_array_elements = lcm_of_array_elements * divisor;
                }
                else
                {
                    divisor++;
                }

                // Check if all element_array is 1 indicate
                // we found all factors and terminate while loop.
                if (counter == element_array.Length)
                {
                    return lcm_of_array_elements;
                }
            }
        }
    }
}