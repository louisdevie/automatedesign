using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Documents
{
    /// <summary>
    /// Un évènement défini par l'utilisateur.
    /// </summary>
    public class EnumEvent : Event
    {
        private int id;
        private string name;
        
        /// <summary>
        /// L'identifiant de l'évènement.
        /// </summary>
        public int Id => this.id;

        /// <summary>
        /// Le nom de l'évènement.
        /// </summary>
        public string Name { get => this.name; set => this.name = value; }

        public override int Order => 0;

        public override string DisplayName => this.Name;

        /// <summary>
        /// Crée un nouvel évènement utilisateur.
        /// </summary>
        /// <param name="id">L'identifiant de l'évènement.</param>
        /// <param name="name">Le nom de l'évènement.</param>
        public EnumEvent(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

    }
}
