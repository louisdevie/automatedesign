using AutomateDesign.Client.Model;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace AutomateDesign.Client.View.Helpers
{
    public class Navigator
    {
        private Frame frame;
        private Session? session;
        private Stack<object> history;

        private object Current { get => this.frame.Content; set => this.frame.Content = value; }

        public Session? Session { get => this.session; set => this.session = value; }

        public Navigator(Frame frame, INavigable initialPage)
        {
            this.frame = frame;
            this.session = null;
            initialPage.UseNavigator(this);
            this.Current = initialPage;
            this.history = new Stack<object>();
        }

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
        }

        public void Back(bool ignoreEnd = false)
        {
            if (ignoreEnd && this.history.Count == 0) return;

            try
            {
                this.Current = this.history.Pop();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Impossible de remonter plus loin dans l'historique.");
            }
        }
    }
}
