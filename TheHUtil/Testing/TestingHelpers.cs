namespace TheHUtil.Testing
{
    using System.Collections.Generic;

    using TheHUtil.Extensions;

    public static class TestingHelpers
    {
        public static string DumpTestSequencesToText<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            return string.Format("Expected:\nActual:\n", expected.PrintContentsToString('\n'), actual.PrintContentsToString('\n'));
        }
    }
}
