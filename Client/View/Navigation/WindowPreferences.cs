using System.Windows;

namespace AutomateDesign.Client.View.Navigation
{
    public class WindowPreferences
    {
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

        public void ApplyTo(Window window)
        {
            switch (this.WindowSizePreference)
            {
                case WindowSize.Small:
                    window.Width = 800;
                    window.Height = 600;
                    window.Left = (SystemParameters.PrimaryScreenWidth - 800) / 2;
                    window.Top = (SystemParameters.PrimaryScreenHeight - 600) / 2;
                    break;

                case WindowSize.FullScreen:
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
