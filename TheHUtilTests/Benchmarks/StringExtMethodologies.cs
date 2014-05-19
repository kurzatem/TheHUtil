using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheHUtil.Extensions;

namespace TheHUtilTests.Benchmarks
{
    [TestClass]
    public class StringExtMethodologies
    {
        [TestMethod]
        public void CountingStyles()
        {
            var iterations = 100000;
            var data = "hoodoododldsfjiroloidfhjafjldka;gf";
            var foo = new[]
                {
                    "hoo", "dod", ";gf"
                };

            var bar = new[]
                {
                    'o', 'h', ';'
                };
            var timer = new Stopwatch();
            var intersection = "Intersect and count";
            var extStringCount = "Extension count";
            var count = "Count";
            var extCharCount = "Extension char count";
            var results = new Dictionary<string, TimeSpan[]>()
            {
                {intersection, new TimeSpan[iterations]},
                {extStringCount, new TimeSpan[iterations]},
                {count, new TimeSpan[iterations]},
                {extCharCount, new TimeSpan[iterations]}
            };
            // Compare Intersect.Count to Count(Func)
            // Method is to sum all the numbers getting the times of each looping.

            for (int i = 0; i < iterations; i++)
            {
                // Intersect
                timer.Start();
                foreach (var str in foo)
                {
                    data.Intersect(str).Count();
                }

                timer.Stop();
                results[intersection][i] = timer.Elapsed;

                // Ext Count using string
                timer.Restart();
                foreach (var str in foo)
                {
                    data.Count(str);
                }

                timer.Stop();
                results[extStringCount][i] = timer.Elapsed;

                // Count
                timer.Restart();
                foreach (var ch in bar)
                {
                    data.Count(c => c == ch);
                }

                timer.Stop();
                results[count][i] = timer.Elapsed;

                // Ext using char
                timer.Restart();
                data.Count(bar);
                timer.Stop();
                results[extCharCount][i] = timer.Elapsed;
                timer.Reset();
            }

            var averages = new[]
                {
                    results[intersection].Average(t => t.Ticks),
                    results[extStringCount].Average(t => t.Ticks),
                    results[count].Average(t => t.Ticks),
                    results[extCharCount].Average(t => t.Ticks)
                };

            string output = "\nNumber of iterations: " + iterations
                + "\nIntersect technique: " + averages[0]
                + "\nMy Count technique: " + averages[1]
                + "\nCount technique: " + averages[2]
                + "\nOther technique: " + averages[3];

            Assert.IsFalse(true, output);
        }
    }
}
