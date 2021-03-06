using System;
using System.IO;

namespace bnet.protocol.account
{
	public class GameAccountHandle : IProtoBuf
	{
		public uint Id
		{
			get;
			set;
		}

		public uint Program
		{
			get;
			set;
		}

		public uint Region
		{
			get;
			set;
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
			GameAccountHandle.Deserialize(stream, this);
		}

		public static GameAccountHandle Deserialize(Stream stream, GameAccountHandle instance)
		{
			return GameAccountHandle.Deserialize(stream, instance, -1L);
		}

		public static GameAccountHandle DeserializeLengthDelimited(Stream stream)
		{
			GameAccountHandle gameAccountHandle = new GameAccountHandle();
			GameAccountHandle.DeserializeLengthDelimited(stream, gameAccountHandle);
			return gameAccountHandle;
		}

		public static GameAccountHandle DeserializeLengthDelimited(Stream stream, GameAccountHandle instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return GameAccountHandle.Deserialize(stream, instance, num);
		}

		public static GameAccountHandle Deserialize(Stream stream, GameAccountHandle instance, long limit)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			while (limit < 0L || stream.get_Position() < limit)
			{
				int num = stream.ReadByte();
				if (num != -1)
				{
					int num2 = num;
					switch (num2)
					{
					case 21:
						instance.Program = binaryReader.ReadUInt32();
						continue;
					case 22:
					case 23:
					{
						IL_73:
						if (num2 == 13)
						{
							instance.Id = binaryReader.ReadUInt32();
							continue;
						}
						Key key = ProtocolParser.ReadKey((byte)num, stream);
						uint field = key.Field;
						if (field != 0u)
						{
							ProtocolParser.SkipKey(stream, key);
							continue;
						}
						throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
					}
					case 24:
						instance.Region = ProtocolParser.ReadUInt32(stream);
						continue;
					}
					goto IL_73;
				}
				if (limit >= 0L)
				{
					throw new EndOfStreamException();
				}
				return instance;
			}
			if (stream.get_Position() == limit)
			{
				return instance;
			}
			throw new ProtocolBufferException("Read past max limit");
		}

		public void Serialize(Stream stream)
		{
			GameAccountHandle.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, GameAccountHandle instance)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			stream.WriteByte(13);
			binaryWriter.Write(instance.Id);
			stream.WriteByte(21);
			binaryWriter.Write(instance.Program);
			stream.WriteByte(24);
			ProtocolParser.WriteUInt32(stream, instance.Region);
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			num += 4u;
			num += 4u;
			num += ProtocolParser.SizeOfUInt32(this.Region);
			return num + 3u;
		}

		public void SetId(uint val)
		{
			this.Id = val;
		}

		public void SetProgram(uint val)
		{
			this.Program = val;
		}

		public void SetRegion(uint val)
		{
			this.Region = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			num ^= this.Id.GetHashCode();
			num ^= this.Program.GetHashCode();
			return num ^ this.Region.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			GameAccountHandle gameAccountHandle = obj as GameAccountHandle;
			return gameAccountHandle != null && this.Id.Equals(gameAccountHandle.Id) && this.Program.Equals(gameAccountHandle.Program) && this.Region.Equals(gameAccountHandle.Region);
		}

		public static GameAccountHandle ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<GameAccountHandle>(bs, 0, -1);
		}
	}
}
