using System;
using System.IO;
using System.Numerics;
using Assets.Scripts.Extensions;

namespace Assets.Scripts.Serialization
{

    public class BinaryDeserializer
    {
        private readonly Stream _source;

        public BinaryDeserializer(Stream source)
        {
            _source = source;
        }

        private byte ReadByte()
        {
            return (byte)_source.ReadByte();
        }

        public ushort ReadUInt16()
        {
            ushort a = ReadByte();
            ushort b = ReadByte();

            return (ushort)((a << 8) | b);
        }

        public uint ReadUInt32()
        {
            uint a = ReadByte();
            uint b = ReadByte();
            uint c = ReadByte();
            uint d = ReadByte();

            return (a << 24) | (b << 16) | (c << 8) | d;
        }

        public long ReadVariableInt64(long value)
        {
            return ReadVariableUint64().DecodeZigZag();
        }

        public ulong ReadVariableUint64()
        {
            ulong accumulator = 0;
            var iterations = 0;

            byte readByte;
            do
            {
                readByte = ReadByte();
                accumulator |= ((ulong)readByte & 127) << (iterations * 7);
                iterations++;
            } while ((readByte & 128) != 0);

            return accumulator;
        }

        public uint ReadUInt24()
        {
            return ReadByte() | ((uint)ReadByte() << 8) | ((uint)ReadByte() << 16);
        }

        public float ReadVariableFloat()
        {
            var x = (int)ReadVariableUint64().DecodeZigZag();
            var d = ReadNormalizedFloat8();

            return x + d;
        }

        public float ReadNormalizedFloat8()
        {
            return ReadByte() / (float)byte.MaxValue;
        }

        public float ReadRangeLimitedFloat8(float min, float range)
        {
            return ReadNormalizedFloat8() * range + min;
        }

        public float ReadNormalizedFloat16()
        {
            return ReadUInt16() / (float)ushort.MaxValue;
        }

        public float ReadRangeLimitedFloat16(float min, float range)
        {
            return ReadNormalizedFloat16() * range + min;
        }

        public Vector3 ReadNormalizedVector3()
        {
            // http://aras-p.info/texts/CompactNormalStorage.html
            // Method #4: Spheremap Transform

            var x = ReadNormalizedFloat16();
            var y = ReadNormalizedFloat16();

            var enc = new Vector4(x, y, 0, 0);
            var nn = enc * new Vector4(2, 2, 0, 0) + new Vector4(-1, -1, 1, -1);

            var l = Math.Max(0, Vector3.Dot(new Vector3(nn.X, nn.Y, nn.Z), -new Vector3(nn.X, nn.Y, nn.W)));
            var sqrtl = MathF.Sqrt(l);

            nn.Z = l;
            nn.X *= sqrtl;
            nn.Y *= sqrtl;

            var output = new Vector3(nn.X, nn.Y, nn.Z) * 2 + new Vector3(0, 0, -1);

            return Vector3.Normalize(output);
        }

        public Quaternion ReadQuaternion16()
        {
            return new Quaternion(
                ReadRangeLimitedFloat16(-1, 2),
                ReadRangeLimitedFloat16(-1, 2),
                ReadRangeLimitedFloat16(-1, 2),
                ReadRangeLimitedFloat16(-1, 2)
            );
        }

        public Vector3 ReadVector3()
        {
            return new Vector3(
                ReadVariableFloat(),
                ReadVariableFloat(),
                ReadVariableFloat()
            );
        }
    }
}