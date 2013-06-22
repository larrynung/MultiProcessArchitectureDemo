using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FakeChrome
{
	static class Program
	{
		[DllImport("User32.dll", EntryPoint = "SendMessage")]
		private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref CopyDataStruct lParam);

		#region Const
		const int WM_COPYDATA = 0x4A;
		#endregion

		public static IntPtr m_ReceiverHandel { get; set; }


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var options = new Options();
			ICommandLineParser parser = new CommandLineParser();
			if (parser.ParseArguments(args, options))
			{
				m_ReceiverHandel = (IntPtr)options.Handle;
				if (options.IsBrowser)
				{
					var browserTab = new WebBrowserPage()
					{
						StartPosition = FormStartPosition.Manual,
						Top = -3200,
						Left = -3200,
						Width = 0,
						Height = 0
					};

					browserTab.TextChanged += browserTab_TextChanged;

					Application.Run(browserTab);
					return;
				}
			}

			Application.Run(new MainForm());
		}

		static void browserTab_TextChanged(object sender, EventArgs e)
		{
			var browserTab = sender as WebBrowserPage;
			var buffer = Encoding.Default.GetBytes(browserTab.Text);

			var cds = new CopyDataStruct();
			cds.cbData = buffer.Length;
			cds.dwData = browserTab.Handle;
			cds.lpData = Marshal.AllocHGlobal(buffer.Length);

			Marshal.Copy(buffer, 0, cds.lpData, buffer.Length);

			SendMessage(m_ReceiverHandel, WM_COPYDATA, 0x401, ref cds);
		}
	}
}
