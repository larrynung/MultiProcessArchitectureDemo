using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeChrome
{
    public class MessageEventArgs : EventArgs
    {
        #region Property
        public uint Message { get; set; }
        public IntPtr wParam { get; set; }
        public IntPtr lParam { get; set; }
        #endregion

        #region Constructor
        public MessageEventArgs(uint message, IntPtr wparam, IntPtr lparam)
        {
            Message = message;
            wParam = wparam;
            lParam = lparam;
        }
        #endregion
    }
}
