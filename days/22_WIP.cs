using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using adv_of_code_2019.Classes;

namespace adv_of_code_2019
{
    public static class Day22
    {
        public static async Task Run ()
        {
            var input = (await File.ReadAllLinesAsync ("inputs\\22.txt"));
            BigInteger size = 10007;
            //var size = 10;

            var deck = Enumerable.Range (0, Int32.Parse (size.ToString ())).ToList ();

            foreach (var line in input)
            {
                if (line.Contains ("new stack"))
                {
                    deck = deck.DealNewDeck ();
                }
                else if (line.StartsWith ("cut"))
                {
                    var n = Int32.Parse (line.Split (" ").Last ());

                    deck = deck.CutN (n);
                }
                else if (line.StartsWith ("deal with increment"))
                {
                    var n = Int32.Parse (line.Split (" ").Last ());
                    deck = deck.DealwithInc (n);
                }
            }

            Console.WriteLine ("Part 1: " + deck.IndexOf (2019));

            size = 119315717514047;
            BigInteger iter = 101741582076661;
            BigInteger position = 2020;
            BigInteger offset_diff = 0;
            BigInteger increment_mul = 1;

            // var rev = new Queue<string> (input.Reverse ());

            foreach (var line in input)
            {
                if (line.Contains ("cut"))
                {
                    offset_diff += Int32.Parse (line.Split (" ").Last ()) * increment_mul;
                }
                else if (line == "deal into new stack")
                {
                    increment_mul *= -1;
                    offset_diff += increment_mul;
                }
                else
                {
                    increment_mul *= BigInteger.ModPow (Int32.Parse (line.Split (" ").Last ()), size - 2, size);
                }

                increment_mul %= size;
                offset_diff %= size;
            }

            var increment = BigInteger.ModPow (increment_mul, iter, size);
            var offset = offset_diff * (1 - increment) * BigInteger.ModPow ((1 - increment_mul) % size, size - 2, size);

            offset %= size;

            var card = (offset + 2020 * increment) % size;

            Console.WriteLine ("Part 2: " + card);

        }

        //74662303452927

        private static List<int> DealNewDeck (this List<int> deck)
        {
            deck.Reverse ();
            return deck;
        }

        private static List<int> CutN (this List<int> deck, int n)
        {
            if (n > 0)
            {
                var cut = deck.Take (n);

                deck = deck.Skip (n).ToList ();
                deck.AddRange (cut);
            }
            else
            {
                var cut = deck.TakeLast (n.Abs ());

                deck = deck.Take (deck.Count - n.Abs ()).ToList ();

                deck.InsertRange (0, cut);
            }

            return deck;
        }

        private static List<int> DealwithInc (this List<int> deck, int n)
        {
            var newdeck = new int[deck.Count];

            for (int i = 0; i < deck.Count; i++)
            {
                newdeck[(n * i) % deck.Count] = deck[i];
            }

            return newdeck.ToList ();
        }
    }
}