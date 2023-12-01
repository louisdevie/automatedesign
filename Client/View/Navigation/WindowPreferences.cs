using System.Windows;

namespace AutomateDesign.Client.View.Navigation
{
    /// <summary>
    /// Des préférences relatives à un conteneur de navigation.
    /// </summary>
    public class WindowPreferences
    {
        private const double SMALL_WINDOW_WIDTH = 400;
        private const double SMALL_WINDOW_HEIGHT = 600;
        private const double LARGE_WINDOW_DEFAULT_WIDTH = 1200;
        private const double LARGE_WINDOW_DEFAULT_HEIGHT = 700;

        /// <summary>
        /// Tailles de fenêtre possibles.
        /// </summary>
        public enum WindowSize
        {
            /// <summary>Aucune préférence.</summary>
            NoPreference,

            /// <summary>Petite fenêtre verticale.</summary>
            Small,

            /// <summary>Grande fenêtre.</summary>
            Large
        }

        /// <summary>
        /// Modes de fenêtre possibles
        /// </summary>
        public enum ResizeMode
        {
            /// <summary>Aucune préférence.</summary>
            NoPreference,

            /// <summary>La fenêtre peut être redimensionnée.</summary>
            Resizeable,

            /// <summary>La fenêtre peut être réduite, mais pas redimensionnée.</summary>
            MinimizeOnly,

            /// <summary>La fenêtre ne peut ni être réduite, ni redimmensionnée.</summary>
            NoResize
        }

        private WindowSize windowSize;
        private ResizeMode resizeMode;
        private string? windowTitle;

        /// <summary>
        /// La taille de fenêtre préférée.
        /// </summary>
        public WindowSize WindowSizePreference => this.windowSize;

        /// <summary>
        /// Le mode de fenêtre préféré.
        /// </summary>
        public ResizeMode ResizeModePreference => this.resizeMode;

        /// <summary>
        /// Le titre de fenêtre voulu.
        /// </summary>
        public string? WindowTitle => this.windowTitle;

        /// <summary>
        /// Crée des préférence pour le conteneur de navigation.
        /// </summary>
        /// <param name="windowSize">La taille de fenêtre préférée.</param>
        /// <param name="resizeMode">Le mode de fenêtre préféré.</param>
        /// <param name="windowTitle">Le titre de fenêtre voulu.</param>
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
            window.Height = height;
            if (recenter)
            {
                window.Left = (SystemParameters.PrimaryScreenWidth - width) / 2;
                window.Top = (SystemParameters.PrimaryScreenHeight - height) / 2;
            }
        }

        /// <summary>
        /// Applique une préférence de taille de fenêtre.
        /// </summary>
        /// <param name="windowSize">La taille de fenêtre voulue.</param>
        /// <param name="window">La fenêtre à redimensionner.</param>
        public static void ApplySize(WindowSize windowSize, Window window)
        {
            switch (windowSize)
            {
                case WindowSize.Small:
                    ResizeWindow(window, SMALL_WINDOW_WIDTH, SMALL_WINDOW_HEIGHT, true);
                    window.WindowState = WindowState.Normal;
                    break;

                case WindowSize.Large:
                    ResizeWindow(window, LARGE_WINDOW_DEFAULT_WIDTH, LARGE_WINDOW_DEFAULT_HEIGHT, false);
                    window.WindowState = WindowState.Maximized;
                    break;
            }
        }

        /// <summary>
        /// Applique la préférence de taille de fenêtre.
        /// </summary>
        /// <param name="window">La fenêtre à redimensionner.</param>
        public void ApplySize(Window window) => ApplySize(this.windowSize, window);

        /// <summary>
        /// Applique une préférence de mode de fenêtre.
        /// </summary>
        /// <param name="resizeMode">Le mode de fenêtre voulu.</param>
        /// <param name="window">La fenêtre à configurer.</param>
        public static void ApplyResizeMode(ResizeMode resizeMode, Window window)
        {
            switch (resizeMode)
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
        }

        /// <summary>
        /// Applique la préférence de mode de fenêtre.
        /// </summary>
        /// <param name="window">La fenêtre à configurer.</param>
        public void ApplyResizeMode(Window window) => ApplyResizeMode(this.resizeMode, window);

        /// <summary>
        /// Change le titre de la fenêtre pour correspondre à la page.
        /// </summary>
        /// <param name="window">La fenêtre à modifier.</param>
        public void ApplyTitleTo(Window window)
        {
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
