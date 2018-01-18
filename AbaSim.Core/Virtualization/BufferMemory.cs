﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public class BufferMemory16 : IMemoryProvider<Abacus16.Word>
	{
		public BufferMemory16(byte[] buffer)
		{
			if (buffer.Length % 2 != 0) { throw new ArgumentException("buffer must contain an integer amount of Words"); }

			Buffer = new Abacus16.Word[buffer.Length / 2];
			byte[] tmp = new byte[2];
			for (int i = 0; i < Buffer.Length; i++)
			{
				//if (BitConverter.IsLittleEndian)
				//{
				//	tmp[0] = buffer[i * 2 + 1];
				//	tmp[1] = buffer[i * 2];
				//}
				//else
				//{
				tmp[0] = buffer[i * 2];
				tmp[1] = buffer[i * 2 + 1];
				//}
				//Buffer[i] = BitConverter.ToInt16(tmp, 0);
				Abacus16.Word item = new Abacus16.Word();
				item.RawValue = tmp;
				Buffer[i] = item;
			}
		}
		public BufferMemory16(uint size)
		{
            Buffer = new Abacus16.Word[size];
			Reset();
		}

		public Abacus16.Word[] Buffer { get; private set; }

		public int Size
		{
			get { return Buffer.Length; }
		}

		public void Flush() { }

		public void Reset()
		{
			Buffer = new Abacus16.Word[Size];
		}

		/// <summary>
		/// Gets or sets the value at <paramref name="index"/>.
		/// </summary>
		/// <param name="index">0 based offset</param>
		/// <returns>value at <paramref name="index"/></returns>
		public Abacus16.Word this[int index]
		{
			get
			{
				return Buffer[index];
			}
			set
			{
				Buffer[index] = value;
			}
		}


		public IEnumerable<KeyValuePair<int, Abacus16.Word>> GetDebugDump()
		{
			for (int i = 0; i < Buffer.Length; i++)
			{
				yield return new KeyValuePair<int, Abacus16.Word>(i, Buffer[i]);
			}
		}
	}
}
