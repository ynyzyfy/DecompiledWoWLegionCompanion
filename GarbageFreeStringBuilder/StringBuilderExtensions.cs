using System;
using System.Globalization;
using System.Text;

namespace GarbageFreeStringBuilder
{
	public static class StringBuilderExtensions
	{
		private static readonly char[] ms_digits = new char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F'
		};

		private static readonly uint ms_default_decimal_places = 5u;

		private static readonly char ms_default_pad_char = '0';

		public static StringBuilder ConcatFormat<A>(this StringBuilder string_builder, string format_string, A arg1) where A : IConvertible
		{
			return string_builder.ConcatFormat(format_string, arg1, 0, 0, 0);
		}

		public static StringBuilder ConcatFormat<A, B>(this StringBuilder string_builder, string format_string, A arg1, B arg2) where A : IConvertible where B : IConvertible
		{
			return string_builder.ConcatFormat(format_string, arg1, arg2, 0, 0);
		}

		public static StringBuilder ConcatFormat<A, B, C>(this StringBuilder string_builder, string format_string, A arg1, B arg2, C arg3) where A : IConvertible where B : IConvertible where C : IConvertible
		{
			return string_builder.ConcatFormat(format_string, arg1, arg2, arg3, 0);
		}

		public static StringBuilder ConcatFormat<A, B, C, D>(this StringBuilder string_builder, string format_string, A arg1, B arg2, C arg3, D arg4) where A : IConvertible where B : IConvertible where C : IConvertible where D : IConvertible
		{
			int num = 0;
			for (int i = 0; i < format_string.get_Length(); i++)
			{
				if (format_string.get_Chars(i) == '{')
				{
					if (num < i)
					{
						string_builder.Append(format_string, num, i - num);
					}
					uint base_value = 10u;
					uint num2 = 0u;
					uint num3 = 5u;
					i++;
					char c = format_string.get_Chars(i);
					if (c == '{')
					{
						string_builder.Append('{');
						i++;
					}
					else
					{
						i++;
						if (format_string.get_Chars(i) == ':')
						{
							i++;
							while (format_string.get_Chars(i) == '0')
							{
								i++;
								num2 += 1u;
							}
							if (format_string.get_Chars(i) == 'X')
							{
								i++;
								base_value = 16u;
								if (format_string.get_Chars(i) >= '0' && format_string.get_Chars(i) <= '9')
								{
									num2 = (uint)(format_string.get_Chars(i) - '0');
									i++;
								}
							}
							else if (format_string.get_Chars(i) == '.')
							{
								i++;
								num3 = 0u;
								while (format_string.get_Chars(i) == '0')
								{
									i++;
									num3 += 1u;
								}
							}
						}
						while (format_string.get_Chars(i) != '}')
						{
							i++;
						}
						switch (c)
						{
						case '0':
							string_builder.ConcatFormatValue(arg1, num2, base_value, num3);
							break;
						case '1':
							string_builder.ConcatFormatValue(arg2, num2, base_value, num3);
							break;
						case '2':
							string_builder.ConcatFormatValue(arg3, num2, base_value, num3);
							break;
						case '3':
							string_builder.ConcatFormatValue(arg4, num2, base_value, num3);
							break;
						}
					}
					num = i + 1;
				}
			}
			if (num < format_string.get_Length())
			{
				string_builder.Append(format_string, num, format_string.get_Length() - num);
			}
			return string_builder;
		}

		private static void ConcatFormatValue<T>(this StringBuilder string_builder, T arg, uint padding, uint base_value, uint decimal_places) where T : IConvertible
		{
			TypeCode typeCode = arg.GetTypeCode();
			switch (typeCode)
			{
			case 9:
				string_builder.Concat(arg.ToInt32(NumberFormatInfo.get_CurrentInfo()), padding, '0', base_value);
				return;
			case 10:
				string_builder.Concat(arg.ToUInt32(NumberFormatInfo.get_CurrentInfo()), padding, '0', base_value);
				return;
			case 11:
			case 12:
				IL_2B:
				if (typeCode != 18)
				{
					return;
				}
				string_builder.Append(Convert.ToString(arg));
				return;
			case 13:
				string_builder.Concat(arg.ToSingle(NumberFormatInfo.get_CurrentInfo()), decimal_places, padding, '0');
				return;
			}
			goto IL_2B;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, uint uint_val, uint pad_amount, char pad_char, uint base_val)
		{
			uint num = 0u;
			uint num2 = uint_val;
			do
			{
				num2 /= base_val;
				num += 1u;
			}
			while (num2 > 0u);
			string_builder.Append(pad_char, (int)Math.Max(pad_amount, num));
			int num3 = string_builder.get_Length();
			while (num > 0u)
			{
				num3--;
				string_builder.set_Chars(num3, StringBuilderExtensions.ms_digits[(int)((UIntPtr)(uint_val % base_val))]);
				uint_val /= base_val;
				num -= 1u;
			}
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, uint uint_val)
		{
			string_builder.Concat(uint_val, 0u, StringBuilderExtensions.ms_default_pad_char, 10u);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, uint uint_val, uint pad_amount)
		{
			string_builder.Concat(uint_val, pad_amount, StringBuilderExtensions.ms_default_pad_char, 10u);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, uint uint_val, uint pad_amount, char pad_char)
		{
			string_builder.Concat(uint_val, pad_amount, pad_char, 10u);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, int int_val, uint pad_amount, char pad_char, uint base_val)
		{
			if (int_val < 0)
			{
				string_builder.Append('-');
				uint uint_val = (uint)(-1 - int_val + 1);
				string_builder.Concat(uint_val, pad_amount, pad_char, base_val);
			}
			else
			{
				string_builder.Concat((uint)int_val, pad_amount, pad_char, base_val);
			}
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, int int_val)
		{
			string_builder.Concat(int_val, 0u, StringBuilderExtensions.ms_default_pad_char, 10u);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, int int_val, uint pad_amount)
		{
			string_builder.Concat(int_val, pad_amount, StringBuilderExtensions.ms_default_pad_char, 10u);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, int int_val, uint pad_amount, char pad_char)
		{
			string_builder.Concat(int_val, pad_amount, pad_char, 10u);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, float float_val, uint decimal_places, uint pad_amount, char pad_char)
		{
			if (decimal_places == 0u)
			{
				int int_val;
				if (float_val >= 0f)
				{
					int_val = (int)(float_val + 0.5f);
				}
				else
				{
					int_val = (int)(float_val - 0.5f);
				}
				string_builder.Concat(int_val, pad_amount, pad_char, 10u);
			}
			else
			{
				int num = (int)float_val;
				string_builder.Concat(num, pad_amount, pad_char, 10u);
				string_builder.Append('.');
				float num2 = Math.Abs(float_val - (float)num);
				do
				{
					num2 *= 10f;
					decimal_places -= 1u;
				}
				while (decimal_places > 0u);
				num2 += 0.5f;
				string_builder.Concat((uint)num2, 0u, '0', 10u);
			}
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, float float_val)
		{
			string_builder.Concat(float_val, StringBuilderExtensions.ms_default_decimal_places, 0u, StringBuilderExtensions.ms_default_pad_char);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, float float_val, uint decimal_places)
		{
			string_builder.Concat(float_val, decimal_places, 0u, StringBuilderExtensions.ms_default_pad_char);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, float float_val, uint decimal_places, uint pad_amount)
		{
			string_builder.Concat(float_val, decimal_places, pad_amount, StringBuilderExtensions.ms_default_pad_char);
			return string_builder;
		}
	}
}
