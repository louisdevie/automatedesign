using System;
using System.Windows;
using System.Windows.Controls;

namespace AutomateDesign.Client.View.Helpers
{
    /// <summary>
    /// Permet de changer le modèle du premier élément d'un conteneur.
    /// </summary>
    public class FirstItemTemplateSelector : DataTemplateSelector
    {
        private DataTemplate? defaultTemplate, firstItemTemplate;

        /// <summary>
        /// Le modèle à utiliser pour tous les éléments àprès le premier.
        /// </summary>
        public DataTemplate DefaultTemplate
        {
            get => this.defaultTemplate ?? throw new NullReferenceException("No default template set.");
            set => this.defaultTemplate = value;
        }

        /// <summary>
        /// Le modèle à utiliser pour le premier élément.
        /// </summary>
        public DataTemplate FirstItemTemplate
        {
            get => this.firstItemTemplate ?? throw new NullReferenceException("No first item template set.");
            set => this.firstItemTemplate = value;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(container);

            if (item == itemsControl.Items[0])
            {
                return FirstItemTemplate;
            }
            return DefaultTemplate;
        }
    }

}
