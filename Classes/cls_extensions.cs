using System.Collections.Generic;
using System.Linq;

namespace adv_of_code_2019.Classes
{
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values)
        {
            if (values.Count() == 1)
                return new[] { values };
            return values.SelectMany(v => Permutations(values.Where(x => x.Equals(v) == false)), (v, p) => p.Prepend(v));
        }
    }
}