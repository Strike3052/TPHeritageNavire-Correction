using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavireHeritage.ClassesMetier
{
    class Passager
    {
        private string numPasseport;
        private string nom;
        private string prenom;
        private string nationalite;

        public Passager(string numPasseport, string nom, string prenom, string nationalite)
        {
            this.numPasseport = numPasseport;
            this.nom = nom;
            this.prenom = prenom;
            this.nationalite = nationalite;
        }

        public string NumPasseport { get => numPasseport; }
        public string Nom { get => nom; }
        public string Prenom { get => prenom; }
        public string Nationalite { get => nationalite; }
    }
}
