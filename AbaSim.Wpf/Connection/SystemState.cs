using AbaSim.Core.Virtualization.Abacus16;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Wpf.Connection
{
    class SystemState
    {
        public Word[] dataMemory
        {
            get; set;
        }

        public Word[] programMemory
        {
            get; set;
        }

        public int programmCounter
        {
            get; set;
        }

        public IRegisterGroup registerGroup
        {
            get;set;
        }
    }
}
