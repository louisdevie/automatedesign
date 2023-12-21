namespace AutomateDesign.Client.View.Helpers;

public static class WpfExtensions
{
    public static System.Windows.Forms.IWin32Window GetIWin32Window(this System.Windows.Media.Visual visual)
    {
        var source = System.Windows.PresentationSource.FromVisual(visual) as System.Windows.Interop.HwndSource;
        System.Windows.Forms.IWin32Window win = new OldWindow(source.Handle);
        return win;
    }

    private class OldWindow : System.Windows.Forms.IWin32Window
    {
        private readonly System.IntPtr handle;
        public OldWindow(System.IntPtr handle)
        {
            handle = handle;
        }

        System.IntPtr System.Windows.Forms.IWin32Window.Handle
        {
            get { return handle; }
        }
    }
}