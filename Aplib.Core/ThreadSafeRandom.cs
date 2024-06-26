// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// Copyright Utrecht University (Department of Information and Computing Sciences)

namespace Aplib.Core
{
    internal static class ThreadSafeRandom
    {
        private static readonly System.Random _global = new();

        [System.ThreadStatic]
        private static System.Random? _local;

        private static System.Random Instance
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

                    _local = new System.Random(seed);
                }

                return _local;
            }
        }

        public static int Next() => Instance.Next();

        public static int Next(int maxValue) => Instance.Next(maxValue);
    }
}
