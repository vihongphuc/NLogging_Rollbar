using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class CommonStatics
    {
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);

        public static readonly DateTime CommonEpoch = new DateTime(2015, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static readonly string DateSerializationFormat = "yyyy-MM-dd";

        public static readonly decimal PointExchangeRate = 100;
    }
}
