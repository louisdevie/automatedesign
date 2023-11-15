using System.Linq;
using System.Windows.Controls;

namespace AutomateDesign.Client.View.Helpers
{
    /// <summary>
    /// Classe qui peut être attachée à une <see cref="TextBox"/> pour compléter automatiquement les e-mails.
    /// </summary>
    public class EmailInputHelper
    {
        private TextBox input;
        private bool isHandlingTextChanged;

        public delegate void AutocompletionEventHandler();

        /// <summary>
        /// Déclenché quand l'email à été complété.
        /// </summary>
        public event AutocompletionEventHandler? AfterAutocompletion;

        private EmailInputHelper(TextBox emailInput)
        {
            this.input = emailInput;
            this.isHandlingTextChanged = false;

            this.input.TextChanged += this.EmailAutocompletion;
        }

        /// <summary>
        /// Attache une instance de cette classe à une <see cref="TextBox"/>.
        /// </summary>
        /// <param name="emailInput"></param>
        public static void AttachTo(TextBox emailInput) => new EmailInputHelper(emailInput);

        private void EmailAutocompletion(object sender, TextChangedEventArgs e)
        {
            if (sender != this.input) return;

            if (this.input.Text.Length > 0)
            {
                // Évitez de traiter l'événement lorsqu'il est déjà en cours de traitement.
                if (this.isHandlingTextChanged) return;
                this.isHandlingTextChanged = true;

                if (e.Changes.Count == 1 // une seule modification...
                    && e.Changes.First().AddedLength == 1 // ...un caractère ajouté...
                    && e.Changes.First().Offset == this.input.Text.Length - 1 // ...à la fin...
                    && this.input.Text[^1] == '@' // ...qui est un @
                )
                {
                    // on ajoute le nom de domaine et on déplace le curseur à la fin
                    this.input.Text += "iut-dijon.u-bourgogne.fr";
                    this.input.Focus();
                    this.input.CaretIndex = this.input.Text.Length;

                    this.AfterAutocompletion?.Invoke();
                }

                this.isHandlingTextChanged = false;
            }
        }
    }
}
