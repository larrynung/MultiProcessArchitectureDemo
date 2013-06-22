using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace FakeChrome
{
	[StructLayout(LayoutKind.Sequential)]
	public struct CopyDataStruct
	{
		public IntPtr dwData;
		public int cbData;
		public IntPtr lpData;
	}

}
