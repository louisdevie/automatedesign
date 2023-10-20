using System.Windows;

namespace AutomateDesign.Client.View.Navigation
{
    public class WindowPreferences
    {
        private const double SMALL_WINDOW_WIDTH = 400;
        private const double SMALL_WINDOW_HEIGHT = 600;
        private const double LARGE_WINDOW_DEFAULT_WIDTH = 1200;
        private const double LARGE_WINDOW_DEFAULT_HEIGHT = 700;

        public enum WindowSize { NoPreference, Small, FullScreen }

        public enum ResizeMode { NoPreference, Resizeable, MinimizeOnly, NoResize }

        private WindowSize windowSize;
        private ResizeMode resizeMode;
        private string? windowTitle;

        public WindowSize WindowSizePreference => this.windowSize;

        public ResizeMode ResizeModePreference => this.resizeMode;

        public string? WindowTitle => this.windowTitle;

        public WindowPreferences(
            WindowSize windowSize = WindowSize.NoPreference,
            ResizeMode resizeMode = ResizeMode.NoPreference,
            string? windowTitle = null
        )
        {
            this.windowSize = windowSize;
            this.resizeMode = resizeMode;
            this.windowTitle = windowTitle;
        }

        private static void ResizeWindow(Window window, double width, double height, bool recenter)
        {
            window.Width = width;
            window.Height = width;
            if (recenter)
            {
                window.Left = (SystemParameters.PrimaryScreenWidth - width) / 2;
                window.Top = (SystemParameters.PrimaryScreenHeight - height) / 2;
            }
        }

        public void ApplyTo(Window window)
        {
            switch (this.WindowSizePreference)
            {
                case WindowSize.Small:
                    ResizeWindow(window, SMALL_WINDOW_WIDTH, SMALL_WINDOW_HEIGHT, true);
                    window.WindowState = WindowState.Normal;
                    break;

                case WindowSize.FullScreen:
                    ResizeWindow(window, LARGE_WINDOW_DEFAULT_WIDTH, LARGE_WINDOW_DEFAULT_HEIGHT, false);
                    window.WindowState = WindowState.Maximized;
                    break;
            }

            switch (this.ResizeModePreference)
            {
                case ResizeMode.Resizeable:
                    window.ResizeMode = System.Windows.ResizeMode.CanResize;
                    break;

                case ResizeMode.MinimizeOnly:
                    window.ResizeMode = System.Windows.ResizeMode.CanMinimize;
                    break;

                case ResizeMode.NoResize:
                    window.ResizeMode = System.Windows.ResizeMode.NoResize;
                    break;
            }

            if (this.WindowTitle is string title)
            {
                if (this.WindowTitle.Length == 0)
                {
                    window.Title = "AutomateDesign";
                }
                else
                {
                    window.Title = "AutomateDesign – " + title;
                }
            }
        }
    }
}
