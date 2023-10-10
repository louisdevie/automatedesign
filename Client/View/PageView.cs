using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutomateDesign.Client.View
{
    public class PageView : Page
    {
        private MainWindow mainWindow;

        public PageView(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }
    }
}
