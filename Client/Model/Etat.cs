using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model
{
    /// <summary>
    /// Structure d'un etat de l'automate
    /// </summary>
    public class Etat : Objet
    {
        #region Attributs
        private int id;
        private string name;
        private List<Transition> transitionsEntrantes;
        private List<Transition> transitionsSortantes;
        #endregion

        #region Properties
        public int Id { get => this.id; }
        public string Name { get => this.name; set => this.name = value; }
        public List<Transition> TransitionsEntrantes { get => this.transitionsEntrantes; set => this.transitionsEntrantes = value; }
        public List<Transition> TransitionsSortantes { get => this.transitionsSortantes; set => this.transitionsSortantes = value; }
        #endregion

        public Etat(int id)
        {
            this.id = id;
            this.name = string.Empty;
            this.transitionsEntrantes = new List<Transition>();
            this.transitionsSortantes = new List<Transition>();
        }
    }
}
