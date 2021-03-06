﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16
{
	[StructLayout(LayoutKind.Explicit)]
	public struct Word : IWord
	{
		public const int Size = 16;

		public static readonly Word Empty = new Word();

		public static readonly Word False = Empty;

		public static readonly Word True = ~False;

		[FieldOffset(0)]
		public short SignedValue;

		[FieldOffset(0)]
		public ushort UnsignedValue;

		public byte[] RawValue
		{
			get
			{
				//CHCK: should the swap only happen on LEndian? (source is BE)
				return Bit.SwapHighLow(BitConverter.GetBytes(UnsignedValue));
			}
			set
			{
				//CHCK: should the swap only happen on LEndian? (source is BE)
				UnsignedValue = BitConverter.ToUInt16(Bit.SwapHighLow(value), 0);
			}
		}

		public Word SignExtend(int currentValueLength)
		{
			bool isNegative = (Bit.S0 & (UnsignedValue >> currentValueLength - 1)) == Bit.S0;
			ushort result = UnsignedValue;
			if (isNegative)
			{
				for (int i = currentValueLength; i < Size; i++)
				{
					result |= (ushort)(Bit.S0 << i);
				}
			}
			return (Word)result;
		}

		public override string ToString()
		{
			return Convert.ToString(UnsignedValue, 2).PadLeft(Size, '0');
		}

		public static Word operator &(Word a, Word b)
		{
			var result = new Word();
			result.UnsignedValue = (ushort)(a.UnsignedValue & b.UnsignedValue);
			return result;
		}

		public static Word operator |(Word a, Word b)
		{
			var result = new Word();
			result.UnsignedValue = (ushort)(a.UnsignedValue | b.UnsignedValue);
			return result;
		}

		public static Word operator ~(Word a)
		{
			var result = new Word();
			result.UnsignedValue = (ushort)~a.UnsignedValue;
			return result;
		}

		public static Word operator ^(Word a, Word b)
		{
			var result = new Word();
			result.UnsignedValue = (ushort)(a.UnsignedValue ^ b.UnsignedValue);
			return result;
		}

		public static Word operator >>(Word a, int b)
		{
			var result = new Word();
			result.UnsignedValue = (ushort)(a.UnsignedValue >> b);
			return result;
		}

		public static Word operator <<(Word a, int b)
		{
			var result = new Word();
			result.UnsignedValue = (ushort)(a.UnsignedValue << b);
			return result;
		}

		public static explicit operator Word(int source)
		{
			var result = new Word();
			result.SignedValue = (short)source;
			return result;
		}

		public static implicit operator int(Word source)
		{
			return source.SignedValue;
		}

		public static explicit operator Word(uint source)
		{
			var result = new Word();
			result.UnsignedValue = (ushort)source;
			return result;
		}

		public static implicit operator uint(Word source)
		{
			return source.UnsignedValue;
		}

		public static implicit operator Word(ushort source)
		{
			var result = new Word();
			result.UnsignedValue = source;
			return result;
		}

		public static implicit operator ushort(Word source)
		{
			return source.UnsignedValue;
		}

		public static implicit operator Word(short source)
		{
			var result = new Word();
			result.SignedValue = source;
			return result;
		}

		public static implicit operator short(Word source)
		{
			return source.SignedValue;
		}

		public static implicit operator Word(byte source)
		{
			var result = new Word();
			result.UnsignedValue = source;
			return result;
		}

		public static explicit operator byte(Word source)
		{
			return (byte)source.UnsignedValue;
		}

		public static implicit operator Word(sbyte source)
		{
			var result = new Word();
			result.SignedValue = source;
			return result;
		}

		public static explicit operator sbyte(Word source)
		{
			return (sbyte)source.SignedValue;
		}
	}
}
