using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class IDGenerator
    {
        private static UniqueNumberProviderBase provider = new UniqueNumberProvider();

        public static void SetProvider(UniqueNumberProviderBase provider)
        {
            IDGenerator.provider = provider;
        }

        public static long GenerateID()
        {
            return IDGenerator.provider.GenerateID();
        }
        public static IList<long> GenerateIDs(int count)
        {
            return IDGenerator.provider.GenerateIDs(count);
        }

    }
}
