using System;
using System.Diagnostics;
using System.Text;

namespace EasyExceptions.Yaml.Helpers
{
    /// <summary>
    /// Pooling of StringBuilder instances.
    /// </summary>
    [DebuggerStepThrough]
    internal static class StringBuilderPool
    {
        private static readonly ConcurrentObjectPool<StringBuilder> Pool;

        static StringBuilderPool()
        {
            Pool = new ConcurrentObjectPool<StringBuilder>(() => new StringBuilder());
        }

        public static BuilderWrapper Rent()
        {
            var builder = Pool.Allocate();
            Debug.Assert(builder.Length == 0);
            return new BuilderWrapper(builder, Pool);
        }

        internal readonly struct BuilderWrapper : IDisposable
        {
            public readonly StringBuilder Builder;
            private readonly ConcurrentObjectPool<StringBuilder> _pool;

            public BuilderWrapper(StringBuilder builder, ConcurrentObjectPool<StringBuilder> pool)
            {
                Builder = builder;
                _pool = pool;
            }

            public override string ToString()
            {
                return Builder.ToString();
            }

            public void Dispose()
            {
                var builder = Builder;

                // do not store builders that are too large.
                if (builder.Capacity <= 1024)
                {
                    builder.Length = 0;
                    _pool.Free(builder);
                }
            }
        }
    }
}
