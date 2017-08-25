using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class Protocol
    {
        private byte[] header = Encoding.ASCII.GetBytes("CHAT");

        public Protocol(byte[] type, byte[] content)
        {
            this.Type = type;
            this.Content = content;
        }

        public byte[] Type
        {
            get;
            private set;
        }

        public byte[] Content
        {
            get;
            private set;
        }

        public byte[] Create()
        {
            byte[] protocol = new byte[header.Length + this.Type.Length + this.Content.Length];

            for (int i = 0; i < header.Length; i++)
            {
                protocol[i] = header[i];
            }

            for (int i = 0; i < this.Type.Length; i++)
            {
                protocol[i + header.Length] = this.Type[i];
            }

            for (int i = 0; i < this.Content.Length; i++)
            {
                protocol[i + header.Length + this.Type.Length] = this.Content[i];
            }

            return protocol;
        }

        public override string ToString()
        {
            return Encoding.ASCII.GetString(this.Create());
        }
    }
}