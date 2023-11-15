using System;
using System.Windows.Controls;

namespace AutomateDesign.Client.ViewModel
{
    /// <summary>
    /// Aide à lire/modifier le contenu des champs de mot de passe.
    /// </summary>
    public class PasswordBoxBinding
    {
        private PasswordBox? box;

        /// <summary>
        /// Le mot de passe saisi.
        /// Si la propriété est utilisée quand aucun <see cref="PasswordBox"/> n'est attaché,
        /// une <see cref="InvalidOperationException"/> sera levée.
        /// </summary>
        public string Password
        {
            get => (this.box ?? throw new InvalidOperationException("No PasswordBox bound to the property.")).Password;
            set => (this.box ?? throw new InvalidOperationException("No PasswordBox bound to the property.")).Password = value;
        }

        public delegate void PasswordChangedEventHandler(PasswordBoxBinding sender);

        /// <summary>
        /// Déclenché quand le mot de passe est modifié.
        /// </summary>
        public event PasswordChangedEventHandler? PasswordChanged;

        private void PropagatePasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            this.PasswordChanged?.Invoke(this);
        }

        /// <summary>
        /// Attache un <see cref="PasswordBox"/> à cette propriété.
        /// Si un autre champ est déjà attaché, il sera détaché.
        /// </summary>
        /// <param name="box">Le champ de mot de passe à attacher.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Bind(PasswordBox box)
        {
            if (this.box is not null) this.Release();

            this.box = box;
            this.box.PasswordChanged += this.PropagatePasswordChanged;
        }

        /// <summary>
        /// Détache la <see cref="PasswordBox"/> de cette propriété.
        /// </summary>
        public void Release()
        {
            if (this.box is not null)
            {
                this.box.PasswordChanged -= this.PropagatePasswordChanged;
                this.box = null;
            }
        }
    }
}
