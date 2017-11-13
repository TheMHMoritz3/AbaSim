﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler.Lexing
{
	[Serializable]
	public class LexerException : CompilerException
	{
		public LexerException() { }
		public LexerException(string message) : base(message) { }
		public LexerException(string message, Exception inner) : base(message, inner) { }
		protected LexerException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
