using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Wpf.Connection
{
    class ModelConnection
    {
        public ModelConnection()
        {
            Compiler = new Core.Compiler.AssemblerCompiler();
            Compiler.LoadMappings();
        }

        public void setProgrammText(string programmText)
        {
            ProgrammText = ProgrammText;
        }

        public void startCompiling()
        {
            if(ProgrammText.Equals(""))
            {
                //Messagebox Fehler kein Text. Berechnung Beenden
                return;
            }

            byte[] binary;
            try
            {
                binary = Compiler.Compile(ProgrammText);
            }
            catch (Core.Compiler.CompilerException e)
            {
                //ToDo MessageBox Berechnung beenden. Fenster Schließen
                return;
            }
        }

        Core.Compiler.AssemblerCompiler Compiler;
        private string ProgrammText;
    }
}
