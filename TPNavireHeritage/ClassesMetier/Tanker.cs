using Station.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavireHeritage.ClassesMetier
{
    class Tanker : Navire, INavCommercable
    {
        private string typeFluide;

        public string getTypeFluide { get => typeFluide; }

        public Tanker(string imo, string nom, string latitude, string longitude, int tonnageActuel, int tonnageGT, int tonnageDWT, string typeFluide)
            :base(imo, nom, latitude, longitude, tonnageActuel, tonnageGT, tonnageDWT)
        {
            this.typeFluide = typeFluide;
        }

        public void decharger(int pQuantite)
        {
            if (pQuantite < 0)
            {
                throw new Exception("La quantité ne peut être négative");
            }
            else if (this.TonnageActuel - pQuantite < 0)
            {
                throw new Exception("Impossible de destocker autant");
            }
            this.TonnageActuel -= pQuantite;
        }
        public void charger(int pQuantite)
        {
            if (pQuantite < 0)
            {
                throw new Exception("La quantité ne peut être négative");
            }
            else if (pQuantite + this.TonnageActuel > this.TonnageGT)
            {
                throw new Exception("Impossible de stocker autant");
            }
            this.TonnageActuel += pQuantite;
        }
    }
}
