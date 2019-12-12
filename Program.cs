using System;
using System.Reflection;
using System.Threading.Tasks;

namespace adv_of_code_2019
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Input Day");
            var day = Console.ReadLine();
            // var day = "12";

            if (Int32.TryParse(day, out var day_num))
            {
                // var cls = Type.GetType("Day" + day_num.ToString());

                var cls = Assembly.GetEntryAssembly().GetType("adv_of_code_2019.Day" + day_num.ToString());

                MethodInfo method = cls.GetMethod("Run");

                Task result = (Task)method.Invoke(null, null);

                //result.Start();

                result.Wait();

                result.Dispose();
            }
        }
    }
}