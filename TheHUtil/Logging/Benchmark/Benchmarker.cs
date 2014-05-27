namespace TheHUtil.Logging.Benchmark
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using TheHUtil.Extensions;

    public static class Benchmarker
    {
        /// <summary>
        /// Times the execution time of a single method that does not accept any parameters and does not return any values. If you need to benchmark a method that needs both inputs and output, consider wrapping it.
        /// </summary>
        /// <subjectAsParameter name="method">The method to benchmark.</subjectAsParameter>
        /// <subjectAsParameter name="iterations">The number of times that the benchmark will be performed. All the results will be totaled, averaged and logged via that <see cref="Logger"/>.</subjectAsParameter>
        /// <remarks>Please be careful with how many iterations you decide to take. There are no restraints in place so if you decide to iterate a few million times, be prepared to wait even for a simple test.</remarks>
        public static void Benchmark(Action method, int iterations)
        {
            var watch = new Stopwatch();
            var results = new List<TimeSpan>(iterations);
            for (int iteration = 0; iteration < iterations; iteration++)
            {
                watch.Start();
                method();
                watch.Stop();
                results.Add(watch.Elapsed);
                watch.Reset();
            }

            var builder = new StringBuilder();
            builder.Append("Method benchmarked: ");
            builder.Append(method.Method.ReflectedType);
            builder.Append(".");
            builder.Append(method.Method.Name);
            builder.Append(". Number of iterations: ");
            builder.Append(iterations.ToString());
            builder.Append(". Total time in milliseconds: ");
            builder.Append(results.Sum(t => t.Milliseconds).ToString());
            builder.Append(". Average time in ticks: ");
            builder.Append(results.Average(t => t.Ticks).ToString());
            Logger.AddToQueue("Benchmarker", builder.ToString(), 1);
        }
    }
}
