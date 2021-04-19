namespace WPF.Win32
{
    public class Constant
    {
		//--- GetWindowLong
		public const int GWL_STYLE = -16;
		public const int GWL_EXSTYLE = -20;
		//--- Windows Style
		public const int WS_EX_CONTEXTHELP = 0x00400;
		public const int WS_MAXIMIZEBOX = 0x10000;
		public const int WS_MINIMIZEBOX = 0x20000;
		public const int WS_SYSMENU = 0x80000;
		//--- Window Messages
		public const int WM_SYSKEYDOWN = 0x0104;
		public const int WM_SYSCOMMAND = 0x0112;
		//--- System Commands
		public const int SC_CONTEXTHELP = 0xF180;
		//--- Keyboard
		public const int VK_F4 = 0x73;
		//--- Constructor
		private Constant() { }
	}
}
