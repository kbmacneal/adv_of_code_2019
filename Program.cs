using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommandLine;

namespace adv_of_code_2020
{
    internal class Program
    {
        public class RuntimeOptions
        {
            [Option ('d', "day", HelpText = "[Number of day] Run the solution for the given day.")]
            public int day { get; set; }

            [Option ('l', "latest", HelpText = "Run the solution using the latest day.")]
            public bool latest { get; set; }
        }

        private static void Main (string [] args)
        {
            Parser.Default.ParseArguments<RuntimeOptions> (args)
                .WithParsed<RuntimeOptions> (o =>
                {
                    if (o.day > 0 && o.latest)
                    {
                        Console.WriteLine ("Cannot use -d and -l together.");
                    }

                    if (o.latest)
                    {
                        var max_day = Assembly.GetEntryAssembly ().GetTypes ().Where (e => e.FullName.Contains ("Day") && !e.FullName.Contains("+")).Select (e => e.FullName.Substring(20)).Select (Int32.Parse).Max ();

                        Console.WriteLine("Running " + "adv_of_code_2020.Day" + max_day.ToString ());

                        var cls = Assembly.GetEntryAssembly ().GetType ("adv_of_code_2020.Day" + max_day.ToString ());

                        MethodInfo method = cls.GetMethod ("Run");

                        Task result = (Task) method.Invoke (null, null);

                        result.Wait ();

                        result.Dispose ();
                    }

                    if (o.day > 0)
                    {
                        Console.WriteLine("Running " + "adv_of_code_2020.Day" + o.day.ToString ());

                        var cls = Assembly.GetEntryAssembly ().GetType ("adv_of_code_2020.Day" + o.day.ToString ());

                        MethodInfo method = cls.GetMethod ("Run");

                        Task result = (Task) method.Invoke (null, null);

                        result.Wait ();

                        result.Dispose ();
                    }

                    if(!o.latest && o.day == 0)
                    {
                        Console.WriteLine ("Input Day");

                        if (Int32.TryParse (Console.ReadLine (), out var day_num))
                        {
                            var cls = Assembly.GetEntryAssembly ().GetType ("adv_of_code_2020.Day" + day_num.ToString ());

                            MethodInfo method = cls.GetMethod ("Run");

                            Task result = (Task) method.Invoke (null, null);

                            result.Wait ();

                            result.Dispose ();
                        }
                    }
                });
        }
    }
}