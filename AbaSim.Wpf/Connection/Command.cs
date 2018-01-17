using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Wpf.Connection
{
    public class Command
    {
        public int Line
        {
            get;
            internal set;
        }

        public String CommandString
        {
            get;
            internal set;
        }

        //public int ProgrammCounter
        //{
        //    get;
        //    internal set;
        //}

        //public byte[] CommandCache
        //{
        //    get;
        //    internal set;
        //}

        //public byte[] CommandMainMemory
        //{
        //    get;
        //    internal set;
        //}
    }
}
