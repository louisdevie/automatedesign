using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Documents
{
    public class EnumEvent : IEvent
    {
        #region Attributs

        private int id;
        private string name;

        #endregion

        #region Properties

        public int Id => this.id;

        public string Name { get => this.name; set => this.name = value; }

        public int Order => 0;

        public string DisplayName => this.Name;

        #endregion

        public EnumEvent(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

    }
}
