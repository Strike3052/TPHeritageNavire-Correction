using Station.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavireHeritage.ClassesMetier
{
    class Croisiere : Navire, ICroisierable
    {
        private char typeNavireCroisiere;
        private int nbPassagersMaxi;
        private Dictionary<String, Passager> passagers = new Dictionary<string, Passager>();

        public char getTypeNavireCroisiere { get => typeNavireCroisiere; }
        public int getNbPassagersMaxi { get => nbPassagersMaxi; }
        internal Dictionary<string, Passager> getPassagers { get => passagers; }

        public Croisiere(string imo, string nom, string latitude, string longitude, int tonnageActuel, int tonnageGT, int tonnageDWT, char typeNavireCroisiere, int nbPassagersMaxi)
            : base(imo, nom, latitude, longitude, tonnageActuel, tonnageGT, tonnageDWT)
        {
            this.typeNavireCroisiere = typeNavireCroisiere;
            this.nbPassagersMaxi = nbPassagersMaxi;
        }

        public Croisiere(string imo, string nom, string latitude, string longitude, int tonnageActuel, int tonnageGT, int tonnageDWT, char typeNavireCroisiere, int nbPassagersMaxi, List<Passager> passagers)
            : base(imo, nom, latitude, longitude, tonnageActuel, tonnageGT, tonnageDWT)
        {
            this.typeNavireCroisiere = typeNavireCroisiere;
            this.nbPassagersMaxi = nbPassagersMaxi;
            foreach (Passager passager in passagers)
            {
                this.passagers.Add(passager.NumPasseport, passager);
            }
        }

        public void embarquer(List<Passager> listPassagers)
        {
            foreach (Passager passager in listPassagers)
            {
                try
                {
                    this.passagers.Add(passager.NumPasseport, passager);
                }
                catch (Exception)
                {
                    Console.WriteLine("Le passager " + passager.NumPasseport + " est déja embarqué !");
                }
            }
        }

        public void debarquer(List<Passager> listPassagers)
        {
            foreach (Passager passager in listPassagers)
            {
                try
                {
                    this.passagers.Remove(passager.NumPasseport);
                }
                catch (Exception)
                {
                    Console.WriteLine("Le passager " + passager.NumPasseport + " n'est pas embarqué !");
                }
            }
        }
    }
}
