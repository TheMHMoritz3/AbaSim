using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization
{
	public class ExecutionCompletedEventArgs : EventArgs
	{
		public ExecutionCompletedEventArgs(Exception reason)
		{
			Reason = reason;
		}

		public Exception Reason { get; private set; }
	}
}
