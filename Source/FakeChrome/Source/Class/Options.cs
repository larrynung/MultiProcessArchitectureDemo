using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeChrome
{
	public class Options
	{
		[Option("h", "handle", Required = true)]
		public int Handle { get; set; }

		[Option("b", "browser")]
		public Boolean IsBrowser { get; set; }
	}

}
