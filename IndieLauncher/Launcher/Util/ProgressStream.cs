﻿using System;
using System.IO;

namespace Dan200.Launcher.Util
{
    public delegate void ProgressDelegate( int percentage );

    public class ProgressStream : Stream
    {
        private Stream m_innerStream;
        private ProgressDelegate m_listener;
        private long m_position;
        private int m_lastProgress;

        public override bool CanRead
        {
            get
            {
                return m_innerStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return m_innerStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override long Length
        {
            get
            {
                return m_innerStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return m_position;
            }
            set
            {
                m_innerStream.Position = value;
                m_position = value;
                EmitProgress();
            }
        }

        public ProgressStream( Stream innerStream, ProgressDelegate listener )
        {
            m_innerStream = innerStream;
            m_listener = listener;
            m_position = 0;
            m_lastProgress = -1;
            EmitProgress();
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                m_innerStream.Dispose();
            }
        }

        public override void Close()
        {
            m_innerStream.Close();
        }

        public override void Flush()
        {
            m_innerStream.Flush();
        }

        public override int Read( byte[] buffer, int offset, int count )
        {
            var result = m_innerStream.Read( buffer, offset, count );
            m_position += result;
            EmitProgress();
            return result;
        }

        public override long Seek( long offset, SeekOrigin origin )
        {
            var result = m_innerStream.Seek( offset, origin );
            m_position = result;
            EmitProgress();
            return result;
        }

        public override void SetLength( long value )
        {
            throw new InvalidOperationException();
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
            throw new InvalidOperationException();
        }

        private void EmitProgress()
        {
            int percentage = (int)((m_position * 100) / Length);
            if( percentage != m_lastProgress )
            {
                m_listener.Invoke( percentage );
                m_lastProgress = percentage;
            }
        }
    }
}
