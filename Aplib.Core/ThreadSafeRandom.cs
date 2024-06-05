using System;

namespace Aplib.Core
{
    internal static class ThreadSafeRandom
    {
        [ThreadStatic]
        private static Random? _local;
        private static readonly Random _global = new();

        private static Random Instance
        {
            get
            {
                if (_local is null)
                {
                    int seed;
                    lock (_global)
                    {
                        seed = _global.Next();
                    }

                    _local = new Random(seed);
                }

                return _local;
            }
        }

        public static int Next() => Instance.Next();

        public static int Next(int maxValue) => Instance.Next(maxValue);
    }
}
