using System.ComponentModel;

namespace AutomateDesign.Client.ViewModel
{
    /// <summary>
    /// Permets de rendre n'importe quelle propriété observable.
    /// </summary>
    /// <typeparam name="T">Le type de la valeur à observer.</typeparam>
    public class Observable<T> : INotifyPropertyChanged
    {
        private T value;

        /// <summary>
        /// Crée une nouvelle propriété observable.
        /// </summary>
        /// <param name="initialValue">La valeur initiale que prends la propriété.</param>
        public Observable(T initialValue)
        {
            this.value = initialValue;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// La valeur observée. Quand elle est modifiée, un évènement PropertyChanged est déclenché.
        /// </summary>
        public T Value
        {
            get => this.value;
            set
            {
                this.value = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }
    }
}
