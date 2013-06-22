using Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FakeChrome
{
	public partial class MainForm : Form
	{
		#region Const
		const int WM_COPYDATA = 0x4A;
		#endregion

		#region Var
		private Dictionary<IntPtr, ApplicationHost> _hostPool;
		#endregion

		#region Private Property
		private Dictionary<IntPtr, ApplicationHost> m_HostPool
		{
			get
			{
				return _hostPool ?? (_hostPool = new Dictionary<IntPtr, ApplicationHost>());
			}
		}
		#endregion

		#region Constructor
		public MainForm()
		{
			InitializeComponent();
		}
		#endregion

		#region Protected Method
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_COPYDATA && m.WParam == (IntPtr)0x401)
			{
				var cds = (CopyDataStruct)Marshal.PtrToStructure(m.LParam, typeof(CopyDataStruct));

				m_HostPool[cds.dwData].Parent.Text = Marshal.PtrToStringAnsi(cds.lpData, cds.cbData);
			}
			base.WndProc(ref m);
		}
		#endregion


		#region Event Process
		private void btnAddTab_Click(object sender, EventArgs e)
		{
			var tabpage = new TabPage();
			tabControl1.TabPages.Add(tabpage);

			var host = new ApplicationHost() 
			{
				File = Application.ExecutablePath,
				Arguments = string.Format("-b -h {0}", this.Handle),
				HideApplicationTitleBar = true,
				Dock = DockStyle.Fill
			};

			host.ProcessLoaded += host_ProcessLoaded;
			host.ProcessUnLoaded += host_ProcessUnLoaded;

			tabpage.Controls.Add(host);
		}

		void host_ProcessLoaded(object sender, EventArgs e)
		{
			var host = sender as ApplicationHost;
			m_HostPool.Add(host.MainWindowHandle, host);
		}

		void host_ProcessUnLoaded(object sender, EventArgs e)
		{
			var host = sender as ApplicationHost;
			var tabpage = host.Parent;

			if (tabpage != null && !tabpage.IsDisposed)
				tabpage.Dispose();
		}
		#endregion
	}
}
