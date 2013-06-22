using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Control
{
	/// <summary>
	/// 
	/// </summary>
	public class ApplicationHost : UserControl
	{
		#region PInvoke
		[DllImport("user32.dll", SetLastError = true)]
		private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
		private static extern long SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);
		#endregion


		#region Const
		private const int GWL_STYLE = -16;
		private const int WS_VISIBLE = 0x10000000;
		#endregion


		#region Var
		private Boolean _autoLoadProcess = true;
		private Process _process;
		private string _file;
		private string _arguments;
		#endregion


		#region Private Property
		/// <summary>
		/// Gets or sets the m_ process.
		/// </summary>
		/// <value>
		/// The m_ process.
		/// </value>
		private Process m_Process
		{
			get
			{
				return _process;
			}
			set
			{
				if (_process == value)
					return;

				if (value == null)
					UnloadProcess();

				_process = value;
			}
		}
		#endregion


		#region Public Property
		/// <summary>
		/// Gets or sets the auto load process.
		/// </summary>
		/// <value>
		/// The auto load process.
		/// </value>
		public Boolean AutoLoadProcess
		{
			get
			{
				return _autoLoadProcess;
			}
			set
			{
				_autoLoadProcess = value;
			}
		}

		/// <summary>
		/// Gets or sets the hide application title bar.
		/// </summary>
		/// <value>
		/// The hide application title bar.
		/// </value>
		public Boolean HideApplicationTitleBar { get; set; }

		/// <summary>
		/// Gets or sets the file.
		/// </summary>
		/// <value>
		/// The file.
		/// </value>
		public string File
		{
			get
			{
				return _file ?? string.Empty;
			}
			set
			{
				_file = value;
			}
		}

		/// <summary>
		/// Gets or sets the arguments.
		/// </summary>
		/// <value>
		/// The arguments.
		/// </value>
		public string Arguments
		{
			get
			{
				return _arguments ?? string.Empty;
			}
			set
			{
				_arguments = value;
			}
		}

		/// <summary>
		/// Gets the main window handle.
		/// </summary>
		/// <value>
		/// The main window handle.
		/// </value>
		public IntPtr MainWindowHandle 
		{
			get
			{
				return m_Process == null ? IntPtr.Zero : m_Process.MainWindowHandle;
			}
		}

		/// <summary>
		/// Gets the main window title.
		/// </summary>
		/// <value>
		/// The main window title.
		/// </value>
		public string MainWindowTitle
		{
			get
			{
				return m_Process == null ? string.Empty : m_Process.MainWindowTitle;
			}
		}
		#endregion


		#region Constructor & DeConstructor
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationHost" /> class.
		/// </summary>
		public ApplicationHost()
		{
			this.Load += ApplicationHost_Load;
			this.ProcessLoaded += ApplicationHost_ProcessLoaded;
			this.ProcessUnLoaded += ApplicationHost_ProcessUnLoaded;
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="ApplicationHost" /> class.
		/// </summary>
		~ApplicationHost()
		{
			m_Process = null;
		}
		#endregion


		#region Event
		public event EventHandler ProcessLoaded;
		public event EventHandler ProcessUnLoaded;
		#endregion


		#region Protected Method
		/// <summary>
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				m_Process = null;
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Raises the <see cref="E:ProcessLoaded" /> event.
		/// </summary>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		protected void OnProcessLoaded(EventArgs e)
		{
			if (ProcessLoaded == null)
				return;
			ProcessLoaded(this, e);
		}

		/// <summary>
		/// Raises the <see cref="E:ProcessUnLoaded" /> event.
		/// </summary>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		protected void OnProcessUnLoaded(EventArgs e)
		{
			if (ProcessUnLoaded == null)
				return;
			ProcessUnLoaded(this, e);
		}
		#endregion


		#region Public Method
		/// <summary>
		/// Loads the process.
		/// </summary>
		public void LoadProcess()
		{
			if (m_Process != null)
			{
				var startInfo = m_Process.StartInfo;
				if (startInfo.FileName != this.File || startInfo.Arguments != this.Arguments)
					m_Process = null;
				else
					return;
			}

			m_Process = new Process()
			{
				SynchronizingObject = this,
				StartInfo = new ProcessStartInfo()
				{
					FileName = File,
					Arguments = this.Arguments
				}
			};
		
			m_Process.Start();

			m_Process.WaitForInputIdle();
			while (!m_Process.HasExited && m_Process.MainWindowHandle == IntPtr.Zero)
			{
				Application.DoEvents();
				Thread.Sleep(100);
			}

			m_Process.EnableRaisingEvents = true;

			m_Process.Exited += m_Process_Exited;

			var handle = m_Process.MainWindowHandle;

			if (HideApplicationTitleBar)
				SetWindowLong(handle, GWL_STYLE, WS_VISIBLE);

			SetParent(handle, this.Handle);

			MoveWindow(handle, 0, 0, this.Width, this.Height, true);

			OnProcessLoaded(EventArgs.Empty);
		}

		/// <summary>
		/// Unloads the process.
		/// </summary>
		public void UnloadProcess()
		{
			if (m_Process == null)
				return;

			if (m_Process.HasExited)
				return;

			m_Process.CloseMainWindow();
			m_Process.WaitForExit(100);

			if (m_Process != null && !m_Process.HasExited)
				m_Process.Kill();

			OnProcessUnLoaded(EventArgs.Empty);
		}

		/// <summary>
		/// Reloads the process.
		/// </summary>
		public void ReloadProcess()
		{
			UnloadProcess();
			LoadProcess();
		}
		#endregion


		#region Event Process
		/// <summary>
		/// Handles the Load event of the ApplicationHost control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		void ApplicationHost_Load(object sender, EventArgs e)
		{
			if (Process.GetCurrentProcess().ProcessName.Equals("devenv", StringComparison.CurrentCultureIgnoreCase))
				return;

			if (AutoLoadProcess)
				LoadProcess();
		}

		/// <summary>
		/// Handles the Resize event of the ApplicationHost control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		void ApplicationHost_Resize(object sender, EventArgs e)
		{
			var handle = m_Process.MainWindowHandle;

			if (handle != IntPtr.Zero)
				MoveWindow(handle, 0, 0, this.Width, this.Height, true);
		}

		/// <summary>
		/// Handles the ProcessLoaded event of the ApplicationHost control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		void ApplicationHost_ProcessLoaded(object sender, EventArgs e)
		{
			this.Resize += ApplicationHost_Resize;
		}

		/// <summary>
		/// Handles the ProcessUnLoaded event of the ApplicationHost control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		void ApplicationHost_ProcessUnLoaded(object sender, EventArgs e)
		{
			this.Resize -= ApplicationHost_Resize;
		}

		/// <summary>
		/// Handles the Exited event of the m_Process control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		void m_Process_Exited(object sender, EventArgs e)
		{
			m_Process = null;

			OnProcessUnLoaded(EventArgs.Empty);
		}
		#endregion
	}
}
