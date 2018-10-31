using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();
            string result = TEST().GetAwaiter().GetResult();
            sw.Stop();
            Console.WriteLine($"{result} - {sw.ElapsedMilliseconds}(ms)");

            Console.ReadKey();
        }

        public static async Task<string> TEST()
        {
            CancellationTokenSource cancelationTokenSource = new CancellationTokenSource();

            // getting the tasks
            var main = Main(2000);
            var timeout = Timeout(3000);

            var contingencyResult = Task.Delay(2000).ContinueWith((x) =>
            {
                if (cancelationTokenSource.IsCancellationRequested)
                    return Task.WhenAny(timeout);

                var contingencyList = new List<Task<string>> {
                    Contingency1(500),
                    Contingency2(900)
                };

                // creating the contingency tasklist
                var taskList = new List<Task<string>>(contingencyList)
                {
                    timeout
                };
                return Task.WhenAny<string>(taskList);
            }, cancelationTokenSource.Token);

            // resolving main result
            var mainResult = await Task.WhenAny<string>(timeout, main).ConfigureAwait(false);
            if (mainResult == main)
            {
                cancelationTokenSource.Cancel();
                return await mainResult;
            }

            //resolving timeout or contingency
            var contingency = await await contingencyResult;
            if (contingency != timeout)
            {
                return await contingency;
            }

            // return timeout
            return await timeout;
        }

        public static async Task<string> Main(int delay)
        {
            Console.WriteLine("s----main");
            await Task.Delay(delay).ConfigureAwait(false);
            Console.WriteLine("e----main");
            return "main";
        }

        public static async Task<string> Timeout(int delay)
        {
            Console.WriteLine("s----timeout");
            await Task.Delay(delay).ConfigureAwait(false);
            Console.WriteLine("e----timeout");
            return "timeout";
        }

        public static async Task<string> Contingency1(int delay)
        {
            Console.WriteLine("s----Contingency1");
            await Task.Delay(delay).ConfigureAwait(false);
            Console.WriteLine("e----Contingency1");
            return "Contingency1";
        }

        public static async Task<string> Contingency2(int delay)
        {
            Console.WriteLine("s----Contingency2");
            await Task.Delay(delay).ConfigureAwait(false);
            Console.WriteLine("e----Contingency2");
            return "Contingency2";
        }

    }
}
