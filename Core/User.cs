using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Core
{
    public class User
    {
        #region Attibuts
        private int id;
        private string email;
        private string password;
        #endregion

        #region Properties
        public int Id { get => id; set => id = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        #endregion
    }
}
