using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public sealed class UniqueNumberProvider : UniqueNumberProviderBase
    {
        public static readonly long TimestampMask = 0x1FFFFFFFFFF;
        public static readonly int TimestampShift = 22;

        public static readonly long ScopeMask = 0x3FF;
        public static readonly int ScopeShift = 12;

        public static readonly long SequenceMask = 0xFFF;
        public static readonly int SequenceShift = 0;

        public UniqueNumberProvider() : this(0) { }
        public UniqueNumberProvider(int scope)
        {
            SetScope(scope);
        }

        public override long GenerateID()
        {
            long counter = CalculateCounter();
            lock (lockObject)
            {
                return CalculateID(counter);
            }
        }
        public override IList<long> GenerateIDs(int count)
        {
            var result = new long[count];

            long counter = CalculateCounter();
            lock (lockObject)
            {
                for (int i = 0; i < count; i++)
                {
                    result[i] = CalculateID(counter);
                }
            }

            return result;
        }

        public void SetScope(int scope)
        {
            long idScope = scope;
            this.scopeBits = (idScope & ScopeMask) << ScopeShift;
        }

        public void Dispose() { }

        private long CalculateID(long currentCounter)
        {
            if (currentCounter > lastCounter)
            {
                this.lastCounter = currentCounter;
                this.currentSequence = 0;
            }
            else
            {
                this.currentSequence = (currentSequence + 1) & SequenceMask;
                if (this.currentSequence == 0)
                {
                    this.lastCounter++;  // rolled over, add 1 millisecond
                }
            }

            return (this.lastCounter << TimestampShift) |
                   (this.scopeBits) |
                   (this.currentSequence << SequenceShift);
        }
        private long CalculateCounter()
        {
            return (long)(DateTime.UtcNow - CommonStatics.CommonEpoch).TotalMilliseconds & TimestampMask;
        }

        private long lastCounter;
        private long currentSequence;

        private long scopeBits;

        private readonly object lockObject = new object();
    }
}
