using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.View.Navigation
{
    public interface INavigable
    {
        void UseNavigator(Navigator navigator);

        void OnNavigatedToThis(bool clearedHistory);

        void OnWentBackToThis();

        WindowPreferences Preferences { get; }
    }
}
