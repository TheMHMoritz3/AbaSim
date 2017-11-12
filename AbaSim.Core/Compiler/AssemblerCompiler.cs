﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AbaSim.Core.Virtualization.Abacus16;

namespace AbaSim.Core.Compiler
{
	public class AssemblerCompiler
	{
		public AssemblerCompiler() { }

		public string LineSperator { get; set; }

		protected Dictionary<string, int> Labels = new Dictionary<string, int>();

		protected Dictionary<string, InstructionMapping> Mappings;

		public byte[] Compile(string sourceCode)
		{
			Lexing.AssemblerLexer lexer = new Lexing.AssemblerLexer();

			List<Lexing.Instruction> instructions = lexer.Lex(sourceCode).ToList();

			int instructionCounter = 0;
			List<Word> nativeInstructions = new List<Word>();

			foreach (var instruction in instructions)
			{
				if (!string.IsNullOrWhiteSpace(instruction.Label))
				{
					Labels.Add(instruction.Label.Trim(), instructionCounter);
				}
				if (!string.IsNullOrWhiteSpace(instruction.Operation))
				{
					instructionCounter++;
				}
			}

			instructionCounter = 0;
			foreach (var instruction in instructions)
			{
				if (!string.IsNullOrWhiteSpace(instruction.Operation))
				{
					Word nativeInstruction = Word.Empty;

					//TODO: handle unknown instructions
					//TODO: handle pseudo instructions
					InstructionMapping mapping = Mappings[instruction.Operation.Trim()];

					nativeInstruction |= ((Word)mapping.OpCode) << 10;

					switch (mapping.Type)
					{
						case AbaSim.Core.Compiler.Parsing.InstructionType.Register:
						case AbaSim.Core.Compiler.Parsing.InstructionType.VRegister:
							if (instruction.Arguments.Count == 3)
							{
								//rd
								int rd = ParseRawRegister(instruction.Arguments[0]);
								nativeInstruction |= ((((Word)rd) & ((Word)(Bit.S7 + Bit.S8 + Bit.S9))) << 7);

								//rl
								int rl = ParseRawRegister(instruction.Arguments[1]);
								nativeInstruction |= ((((Word)rl) & ((Word)(Bit.S4 + Bit.S5 + Bit.S6))) << 4);

								//rr
								int rr = ParseRawRegister(instruction.Arguments[1]);
								nativeInstruction |= (((Word)rr) & ((Word)(Bit.S1 + Bit.S2 + Bit.S3)));

								if (mapping.Type == Parsing.InstructionType.VRegister)
								{
									nativeInstruction |= ((Word)Bit.S0);
								}
							}
							else
							{
								//TODO: throw
							}
							break;
						case AbaSim.Core.Compiler.Parsing.InstructionType.Store:
							if (instruction.Arguments.Count == 2)
							{
								//c
								int constant = ParseRawConstant(instruction.Arguments[1], instructionCounter);
								nativeInstruction |= (((Word)constant) & ((Word)(Bit.S0 + Bit.S1 + Bit.S2 + Bit.S3 + Bit.S4 + Bit.S5 + Bit.S6)));

								//rd
								int rd = ParseRawRegister(instruction.Arguments[0]);
								nativeInstruction |= ((((Word)rd) & ((Word)(Bit.S7 + Bit.S8 + Bit.S9))) << 7);
							}
							else
							{
								//TODO: throw
							}
							break;
						case AbaSim.Core.Compiler.Parsing.InstructionType.Immediate:
							if (instruction.Arguments.Count == 3)
							{
								//c
								int constant = ParseRawConstant(instruction.Arguments[2], instructionCounter);
								nativeInstruction |= (((Word)constant) & ((Word)(Bit.S0 + Bit.S1 + Bit.S2 + Bit.S3)));

								//rd
								int rd = ParseRawRegister(instruction.Arguments[0]);
								nativeInstruction |= ((((Word)rd) & ((Word)(Bit.S7 + Bit.S8 + Bit.S9))) << 7);

								//rl
								int rl = ParseRawRegister(instruction.Arguments[1]);
								nativeInstruction |= ((((Word)rl) & ((Word)(Bit.S4 + Bit.S5 + Bit.S6))) << 4);
							}
							else
							{
								//TODO: throw
							}
							break;
						case AbaSim.Core.Compiler.Parsing.InstructionType.Jump:
							if (instruction.Arguments.Count == 1)
							{
								int constant = ParseRawConstant(instruction.Arguments[0], instructionCounter);
								nativeInstruction |= (((Word)constant) & ((Word)(Bit.S0 + Bit.S1 + Bit.S2 + Bit.S3 + Bit.S4 + Bit.S5 + Bit.S6 + Bit.S7 + Bit.S8 + Bit.S9)));
							}
							else
							{
								//TODO: throw
							}
							break;
						default:
							//TODO: should not happen
							break;
					}

					nativeInstructions.Add(nativeInstruction);

					instructionCounter++;
				}
			}

			byte[] binary = new byte[nativeInstructions.Count * (Word.Size / 8)];
			for (int i = 0; i < nativeInstructions.Count; i++)
			{
				nativeInstructions[i].RawValue.CopyTo(binary, i * (Word.Size / 8));
			}
			return binary;
		}

		public virtual void LoadMappings()
		{
			Mappings = new Dictionary<string, InstructionMapping>();
			foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
			{
				foreach (var mappingAttribute in type.GetCustomAttributes<Parsing.AssemblyCodeAttribute>())
				{
					Mappings.Add(mappingAttribute.FriendlyName, new InstructionMapping()
					{
						Type = mappingAttribute.Type,
						OpCode = mappingAttribute.OpCode
					});
				}
			}
		}

		protected virtual int ParseRawRegister(string rawRegister)
		{
			rawRegister = rawRegister.Trim();
			if (string.IsNullOrWhiteSpace(rawRegister))
			{
				//TODO: throw
			}
			if (rawRegister.StartsWith("$"))
			{
				rawRegister = rawRegister.Substring(1);
			}
			return int.Parse(rawRegister);
		}

		protected virtual int ParseRawConstant(string rawConstant, int instructionCounter)
		{
			rawConstant = rawConstant.Trim();
			int constant;
			if (!int.TryParse(rawConstant, out constant))
			{
				constant = Labels[rawConstant] - instructionCounter;
				//TODO: handle missing labels
			}
			//TODO: bounds validation
			//BUG: constant is always assumed singed, is that guaranteed?
			return constant;
		}

		protected struct InstructionMapping
		{
			public Parsing.InstructionType Type { get; set; }
			public byte OpCode { get; set; }
		}
	}
}