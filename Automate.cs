using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NomAutomate.Automate
{
    public class Automate
    {
        private Etat etatCourant;

        public Etat EtatCourant { get => etatCourant; set => etatCourant = value; }

        public void Action()
        {
            throw new NotImplementedException();
        }
    }
}