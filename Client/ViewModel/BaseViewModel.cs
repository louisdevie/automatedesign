using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AutomateDesign.Client.ViewModel
{
    /// <summary>
    /// Le modèle-vue de base qui implémente <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Indique qu'une propriété a changée.
        /// </summary>
        /// <param name="property">
        /// Le nom de la propriété
        /// (par défaut le nom de la méthode/propriété ou est est appelée).
        /// </param>
        protected void NotifyPropertyChanged([CallerMemberName] string property = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
