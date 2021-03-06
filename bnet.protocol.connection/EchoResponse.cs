using System;
using System.IO;

namespace bnet.protocol.connection
{
	public class EchoResponse : IProtoBuf
	{
		public bool HasTime;

		private ulong _Time;

		public bool HasPayload;

		private byte[] _Payload;

		public ulong Time
		{
			get
			{
				return this._Time;
			}
			set
			{
				this._Time = value;
				this.HasTime = true;
			}
		}

		public byte[] Payload
		{
			get
			{
				return this._Payload;
			}
			set
			{
				this._Payload = value;
				this.HasPayload = (value != null);
			}
		}

		public bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public void Deserialize(Stream stream)
		{
			EchoResponse.Deserialize(stream, this);
		}

		public static EchoResponse Deserialize(Stream stream, EchoResponse instance)
		{
			return EchoResponse.Deserialize(stream, instance, -1L);
		}

		public static EchoResponse DeserializeLengthDelimited(Stream stream)
		{
			EchoResponse echoResponse = new EchoResponse();
			EchoResponse.DeserializeLengthDelimited(stream, echoResponse);
			return echoResponse;
		}

		public static EchoResponse DeserializeLengthDelimited(Stream stream, EchoResponse instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return EchoResponse.Deserialize(stream, instance, num);
		}

		public static EchoResponse Deserialize(Stream stream, EchoResponse instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			while (limit < 0L || stream.get_Position() < limit)
			{
				int num = stream.ReadByte();
				if (num == -1)
				{
					if (limit >= 0L)
					{
						throw new EndOfStreamException();
					}
					return instance;
				}
				else
				{
					int num2 = num;
					if (num2 != 9)
					{
						if (num2 != 18)
						{
							Key key = ProtocolParser.ReadKey((byte)num, stream);
							uint field = key.Field;
							if (field == 0u)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
						else
						{
							instance.Payload = ProtocolParser.ReadBytes(stream);
						}
					}
					else
					{
						instance.Time = binaryReader.ReadUInt64();
					}
				}
			}
			if (stream.get_Position() == limit)
			{
				return instance;
			}
			throw new ProtocolBufferException("Read past max limit");
		}

		public void Serialize(Stream stream)
		{
			EchoResponse.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, EchoResponse instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			if (instance.HasTime)
			{
				stream.WriteByte(9);
				binaryWriter.Write(instance.Time);
			}
			if (instance.HasPayload)
			{
				stream.WriteByte(18);
				ProtocolParser.WriteBytes(stream, instance.Payload);
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			if (this.HasTime)
			{
				num += 1u;
				num += 8u;
			}
			if (this.HasPayload)
			{
				num += 1u;
				num += ProtocolParser.SizeOfUInt32(this.Payload.Length) + (uint)this.Payload.Length;
			}
			return num;
		}

		public void SetTime(ulong val)
		{
			this.Time = val;
		}

		public void SetPayload(byte[] val)
		{
			this.Payload = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			if (this.HasTime)
			{
				num ^= this.Time.GetHashCode();
			}
			if (this.HasPayload)
			{
				num ^= this.Payload.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			EchoResponse echoResponse = obj as EchoResponse;
			return echoResponse != null && this.HasTime == echoResponse.HasTime && (!this.HasTime || this.Time.Equals(echoResponse.Time)) && this.HasPayload == echoResponse.HasPayload && (!this.HasPayload || this.Payload.Equals(echoResponse.Payload));
		}

		public static EchoResponse ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<EchoResponse>(bs, 0, -1);
		}
	}
}
