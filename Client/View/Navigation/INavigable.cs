using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.View.Navigation
{
    public interface INavigable
    {
        WindowPreferences Preferences { get; }

        void UseNavigator(Navigator navigator);

        void OnNavigatedToThis(bool clearedHistory);

        void OnWentBackToThis();
    }
}
