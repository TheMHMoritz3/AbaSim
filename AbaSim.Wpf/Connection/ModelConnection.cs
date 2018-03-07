using AbaSim.Core.Virtualization;
using AbaSim.Core.Virtualization.Abacus16;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AbaSim.Wpf.Connection
{
    class ModelConnection:INotifyPropertyChanged
    {
        public ModelConnection()
        {
            Compiler = new Core.Compiler.AssemblerCompiler();
            Compiler.LoadMappings();
        }

        public void setProgrammText(string programmText)
        {
            ProgrammText = programmText;
        }

        public void startCompiling()
        {
            if(ProgrammText.Equals(""))
            {
                throw new Wpf.Exception.Exception(ExceptionId.NoProgrammCode, "Programm Code was: \"\"");
            }

            try
            {
                Core.Compiler.AssemblerCompiler compiler = new Core.Compiler.AssemblerCompiler()
                {
                    Dialect = Core.Compiler.Parsing.Dialects.ChDFT
                };
                compiler.LoadMappings();
                var pipeline = Core.Compiler.CompilePipeline
                    .Start(new Core.Compiler.Lexing.AssemblerLexer())
                    .Continue(new Core.Compiler.PseudoInstructionSubstitutor())
                    .Inspect((instructions, log) =>
                    {
                        int i = 0;
                        //Console.WriteLine("Code after substitution:");
                        foreach (var instruction in instructions)
                        {
                            //Console.WriteLine("{0,4}|{2,4}| {1}", i, instruction, instruction.SourceLine);

                            i++;
                        }
                    })
                    .Continue(compiler)
                    .Complete();

                //var result = pipeline.Compile(sourceCode);
                BinaryCode = pipeline.Compile(ProgrammText);
            }
            catch (Core.Compiler.CompilerException e)
            {
                Wpf.Exception.ExceptionHandling.catchException(e);
            }
        }

        public Task startComputation()
        {
            ProgramMemory = new BufferMemory16(BinaryCode.Output);
            DataMemory = new BufferMemory16(65536);

            CPU = new SerialAbacus16Cpu(ProgramMemory,DataMemory);

            VirtualSystem = new Host(CPU);
            VirtualSystem.ExecutionCompleted += onExecutionCompleted;
            VirtualSystem.ClockCycleScheduled += onClockCycleScheduled;

            VirtualSystem.Start();

            return VirtualSystem.WorkerTask;
        }

        private void onClockCycleScheduled(object sender, ClockCycleScheduledEventArgs e)
        {
            NotifyPropertyChanged(nameof(Steps));
        }

        private void onExecutionCompleted(object sender, ExecutionCompletedEventArgs e)
        {
            NotifyPropertyChanged(nameof(Steps));
        }

        public ulong Steps
        {
            get
            {
                if(VirtualSystem!=null)
                    return VirtualSystem.ExecutedClockCycles;
                return 0;
            }
        }

        public List<Command> UpdateProgrammcodeView()
        {
            string[] elements = ProgrammText.Split('\n');
            int i = 0;
            List<Command> commands= new List<Command>();
            foreach (string element in elements)
            {
                Command com = new Command();
                com.Line = ++i;
                com.CommandString = element;
                com.ProgrammCounter = 0;
                commands.Add(com);
                System.Diagnostics.Trace.WriteLine(i);
            }
            return commands;
        }

        Host VirtualSystem = null;
        SerialAbacus16Cpu CPU = null;
        BufferMemory16 ProgramMemory = null;
        BufferMemory16 DataMemory = null;
        Core.Compiler.AssemblerCompiler Compiler;
        private string ProgrammText;
        Core.Compiler.CompileResult<byte[]> BinaryCode;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            if(PropertyChanged!=null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
