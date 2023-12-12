using AutomateDesign.Client.Model.Logic;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AutomateDesign.Client.View.Navigation
{
    /// <summary>
    /// Gère la navigation au sein d'un conteneur.
    /// </summary>
    public class Navigator
    {
        private Session? session;
        private INavigationContainer container;
        private Stack<INavigable> history;
        private INavigable current;

        private INavigable Current
        {
            get => this.current;
            set
            {
                this.current = value;
                this.container.ChangeContent(value);
                this.container.ApplyPreferences(this.Current.Preferences);
            }
        }

        /// <summary>
        /// La session actuelle.
        /// </summary>
        public Session? Session { get => this.session; set => this.session = value; }

        /// <summary>
        /// La fenêtre parente.
        /// </summary>
        public Window ParentWindow => this.container.ParentWindow;

        /// <summary>
        /// Crée un navigateur.
        /// </summary>
        /// <param name="container">Le conteneur qui va afficher les pages.</param>
        /// <param name="initialPage">La première page à afficher.</param>
#pragma warning disable CS8618 // `current` est bien assigné
        public Navigator(INavigationContainer container, INavigable initialPage)
        {
            this.container = container;
            this.session = null;
            this.history = new Stack<INavigable>();

            initialPage.UseNavigator(this);
            this.Current = initialPage;
            this.Current.OnNavigatedToThis(true);
        }
#pragma warning restore CS8618

        /// <summary>
        /// Navigue vers une autre vue.
        /// </summary>
        /// <param name="nextPage">La nouvelle vue.</param>
        /// <param name="clearAllHistory">Si <see langword="true"/>, l'historique sera effacé avant de passer à la vue suivante.</param>
        public void Go(INavigable nextPage, bool clearAllHistory = false)
        {
            if (clearAllHistory)
            {
                this.history.Clear();
            }
            else
            {
                this.history.Push(this.Current);
            }

            nextPage.UseNavigator(this);
            this.Current = nextPage;
            nextPage.OnNavigatedToThis(clearAllHistory);
        }

        /// <summary>
        /// Retourne à la page précédente.
        /// </summary>
        /// <param name="ignoreEnd">Si <see langword="true"/>, le cas ou il n'y a aucune vue dans l'historique est ignoré silencieusement.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Back(bool ignoreEnd = false)
        {
            if (ignoreEnd && this.history.Count == 0) return;

            try
            {
                this.Current = this.history.Pop();
                this.Current.OnWentBackToThis();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Impossible de remonter plus loin dans l'historique.");
            }
        }
    }
}
