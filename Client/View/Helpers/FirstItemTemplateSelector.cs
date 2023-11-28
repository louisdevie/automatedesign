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

        public DataTemplate DefaultTemplate
        {
            get => this.defaultTemplate ?? throw new NullReferenceException("No default template set.");
            set => this.defaultTemplate = value;
        }

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
