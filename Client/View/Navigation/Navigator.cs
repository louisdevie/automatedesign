using AutomateDesign.Client.Model;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AutomateDesign.Client.View.Navigation
{
    public class Navigator
    {
        private Window window;
        private Frame frame;
        private Session? session;
        private Stack<INavigable> history;
        private INavigable current;

        private INavigable Current
        {
            get => this.current;
            set
            {
                this.current = value;
                this.frame.Content = value;
            }
        }

        public Session? Session { get => this.session; set => this.session = value; }

#pragma warning disable CS8618 // `current` est bien assigné
        public Navigator(Window window, Frame frame, INavigable initialPage)
        {
            this.window = window;
            this.frame = frame;
            this.session = null;
            this.history = new Stack<INavigable>();

            initialPage.UseNavigator(this);
            this.Current = initialPage;
            this.Current.Preferences.ApplyTo(this.window);
            this.Current.OnNavigatedToThis(true);
        }
#pragma warning restore CS8618

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
            this.Current.Preferences.ApplyTo(this.window);
            nextPage.OnNavigatedToThis(clearAllHistory);
        }

        public void Back(bool ignoreEnd = false)
        {
            if (ignoreEnd && this.history.Count == 0) return;

            try
            {
                this.Current = this.history.Pop();
                this.Current.Preferences.ApplyTo(this.window);
                this.Current.OnWentBackToThis();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Impossible de remonter plus loin dans l'historique.");
            }
        }
    }
}
