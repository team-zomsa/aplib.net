namespace Aplib.Core
{
    internal static class ThreadSafeRandom
    {
        [System.ThreadStatic]
        private static System.Random? _local;
        private static readonly System.Random _global = new();

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
