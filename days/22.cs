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
            Part2 (input);

        }

        private static void Part2 (string[] inputs)
        {
            BigInteger size = 119315717514047;
            BigInteger iter = 101741582076661;
            BigInteger position = 2020;
            BigInteger offset_diff = 0;
            BigInteger increment_mul = 1;

            foreach (var line in inputs)
            {
                RunP2 (ref increment_mul, ref offset_diff, size, line);
            }
            
            (BigInteger increment, BigInteger offset) = getseq (iter, increment_mul, offset_diff, size);

            var card = get (offset, increment, 2020, size);

            Console.WriteLine ("Part 2: " + card);
        }

        private static void RunP2 (ref BigInteger inc_mul, ref BigInteger offset_diff, BigInteger size, string line)
        {
            if (line.StartsWith ("cut"))
            {
                offset_diff += Int32.Parse (line.Split (" ").Last ()) * inc_mul;
            }
            else if (line == "deal into new stack")
            {
                inc_mul *= -1;
                offset_diff += inc_mul;
            }
            else
            {
                var num = Int32.Parse (line.Split (" ").Last ());

                inc_mul *= num.TBI ().inv (size);
            }

            inc_mul = inc_mul.mod (size);
            offset_diff = offset_diff.mod (size);
        }

        private static BigInteger mod (this BigInteger x, BigInteger m)
        {
            return (x % m + m) % m;
        }

        private static BigInteger inv (this BigInteger num, BigInteger size)
        {
            return num.mpow (size - 2, size);
        }

        private static BigInteger get (BigInteger offset, BigInteger increment, BigInteger i, BigInteger size)
        {
            return (offset + i * increment) % size;
        }

        private static (BigInteger increment, BigInteger offset) getseq (this BigInteger iterations, BigInteger inc_mul, BigInteger offset_diff, BigInteger size)
        {
            var increment = inc_mul.mpow (iterations, size);

            var offset = offset_diff * (1 - increment) * ((1 - inc_mul) % size).inv (size);

            offset %= size;

            return (increment, offset);
        }

        private static BigInteger TBI (this int num)
        {
            return new BigInteger (num);
        }

        private static BigInteger mpow (this BigInteger bigInteger, BigInteger pow, BigInteger mod)
        {
            return BigInteger.ModPow (bigInteger, pow, mod);
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