﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("sle", OpCode, Compiler.Parsing.InstructionType.Register)]
	class SetLessThanEqualOperationUnit : RegisterOperationUnit
	{
		public const byte OpCode = Bit.B4 + Bit.B1;

		public SetLessThanEqualOperationUnit(RegisterGroup register) : base(register) { }

		protected override void InternalExecute()
		{
			Destination =  (Left.SignedValue <= Right.SignedValue ? Word.True : Word.False);
		}
	}
}
