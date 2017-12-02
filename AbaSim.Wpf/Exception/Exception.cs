using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Wpf.Exception
{
    class Exception : System.Exception
    {
        private Exception()
        { }

        public Exception(ExceptionId id)
        {
            this.id = id;
            detailedText = "";
        }

        public Exception(ExceptionId id, String detailedText)
        {
            this.id = id;
            this.detailedText = detailedText;
        }

        public String DetailedText
        {
            get { return detailedText; }
            set { detailedText = value; }
        }

        public ExceptionId Id
        {
            private set { }
            get { return id; }
        }
            

        private String detailedText;

        private ExceptionId id;
    }
}
