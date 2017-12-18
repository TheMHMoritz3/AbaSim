using AbaSim.Core.Virtualization;
using AbaSim.Core.Virtualization.Abacus16;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                BinaryCode = Compiler.Compile(ProgrammText);
            }
            catch (Core.Compiler.CompilerException e)
            {
                Wpf.Exception.ExceptionHandling.catchException(e);
            }
        }

        public Task startComputation()
        {
            ProgramMemory = new BufferMemory16(BinaryCode);
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

        Host VirtualSystem = null;
        SerialAbacus16Cpu CPU = null;
        BufferMemory16 ProgramMemory = null;
        BufferMemory16 DataMemory = null;
        Core.Compiler.AssemblerCompiler Compiler;
        private string ProgrammText;
        byte[] BinaryCode;

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
