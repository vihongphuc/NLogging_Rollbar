using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public class MemoryCopyStream : Stream
    {
        private readonly MemoryStream memStream;
        private readonly Stream sink;

        public MemoryCopyStream(Stream sink)
            : this(sink, 1024)
        {
        }
        public MemoryCopyStream(Stream sink, int capacity)
        {
            this.sink = sink;
            this.memStream = new MemoryStream(capacity);
        }

        public Stream CopyStream
        {
            get
            {
                return memStream;
            }
        }
        public void CloseCopyStream()
        {
            this.memStream.Dispose();
        }

        public override bool CanRead { get { return this.sink.CanRead; } }
        public override bool CanSeek { get { return this.sink.CanSeek; } }
        public override bool CanWrite { get { return this.sink.CanWrite; } }

        public override long Length { get { return this.sink.Length; } }
        public override long Position
        {
            get
            {
                return this.sink.Position;
            }
            set
            {
                this.sink.Position = value;
                this.memStream.Position = value;
            }
        }

        public override void Flush()
        {
            this.sink.Flush();
            this.memStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var result = this.sink.Read(buffer, offset, count);
            memStream.Write(buffer, offset, count);

            return result;
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            var result = this.sink.Seek(offset, origin);
            this.memStream.Seek(offset, origin);

            return result;
        }
        public override void SetLength(long value)
        {
            this.sink.SetLength(value);
            this.memStream.SetLength(value);
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            this.sink.Write(buffer, offset, count);
            memStream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                sink.Dispose();
            }
        }
    }
}
