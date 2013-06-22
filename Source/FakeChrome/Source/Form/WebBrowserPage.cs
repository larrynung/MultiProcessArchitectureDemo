using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FakeChrome
{
	public partial class WebBrowserPage : Form
	{
		public WebBrowserPage()
		{
			InitializeComponent();
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			throw new Exception();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			webBrowser1.Navigate(toolStripTextBox1.Text);
		}

		private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
		{
			toolStripTextBox1.Text = e.Url.ToString();
		}

		private void WebBrowserPage_Load(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Maximized;
			webBrowser1.Navigate("http://www.dotblogs.com.tw/larrynung");
		}

		private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			this.Text = webBrowser1.DocumentTitle;
		}
	}
}
