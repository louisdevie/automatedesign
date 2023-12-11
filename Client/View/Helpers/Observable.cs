using System.ComponentModel;

namespace AutomateDesign.Client.View.Helpers
{
    /// <summary>
    /// Permets de rendre n'importe quelle propriété observable.
    /// </summary>
    /// <typeparam name="T">Le type de la valuer à observer.</typeparam>
    public class Observable<T> : INotifyPropertyChanged
    {
        private T value;

        public Observable(T initialValue)
        {
            this.value = initialValue;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

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
