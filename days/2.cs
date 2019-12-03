using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2019
{
    public class Day2
    {
        public static async Task Run ()
        {
            List<int> inputs = (await File.ReadAllTextAsync ("inputs/2.txt")).Split (",").Select (e => Int32.Parse (e)).ToList ();

            inputs[1] = 12;
            inputs[2] = 2;

            for (int i = 0; i < inputs.Count; i++)
            {
                if (inputs[i] == 99) goto break_out;

                int[] slicer = inputs.GetRange (i, 4).ToArray ();

                int a = slicer[1];
                int b = slicer[2];
                int c = slicer[3];

                switch (slicer[0])
                {
                    case 1:
                        inputs[c] = inputs[a] + inputs[b];
                        break;
                    case 2:
                        inputs[c] = inputs[a] * inputs[b];
                        break;
                    case 99:
                        goto break_out;
                    default:
                        break;
                }

                if (i + 4 > inputs.Count - 1) goto break_out;

                i += 3;
            }

            break_out:

                Console.WriteLine ("Part 1: " + inputs[0].ToString ());

            //brute-forcing it

            for (int g = 0; g < 99; g++)
            {
                for (int j = 0; j < 99; j++)
                {
                    var inputs2 = (await File.ReadAllTextAsync ("inputs/2.txt")).Split (",").Select (e => Int32.Parse (e)).ToList ();

                    inputs2[1] = j;
                    inputs2[2] = g;

                    for (int i = 0; i < inputs.Count; i++)
                    {
                        if (inputs[i] == 99) goto break_out_2;

                        int[] slicer = inputs2.GetRange (i, 4).ToArray ();

                        int a = slicer[1];
                        int b = slicer[2];
                        int c = slicer[3];

                        switch (slicer[0])
                        {
                            case 1:
                                inputs2[c] = inputs2[a] + inputs2[b];
                                break;
                            case 2:
                                inputs2[c] = inputs2[a] * inputs2[b];
                                break;
                            case 99:
                                goto break_out_2;
                            default:
                                break;
                        }

                        if (i + 4 > inputs2.Count - 1) goto break_out;

                        i += 3;
                    }

                    break_out_2:

                        if (inputs2[0] == 19690720)
                        {
                            Console.WriteLine (string.Format ("f({0},{1}) = {2}", j, g, inputs2[0]));
                        }
                }
            }

        }
    }
}