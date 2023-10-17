using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model
{
    public class EnumEvent : Event
    {
        #region Attributs
        private int id;
        private string name;
        #endregion

        #region Properties
        public int Id { get => this.id; }
        public string Name { get => this.name; set => this.name = value; }
        #endregion

        public EnumEvent(int id)
        {
            this.id = id;
            this.name = string.Empty;
        }

    }
}
