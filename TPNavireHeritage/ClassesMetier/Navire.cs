using GestionNavire.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NavireHeritage.ClassesMetier
{
    public abstract class Navire
    {
        private readonly string imo;
        private readonly string nom;
        private string latitude;
        private string longitude;
        private int tonnageActuel;
        private int tonnageGT;
        private int tonnageDWT;

        public string Imo => imo;
        public string Nom => nom;
        public string Latitude { get => latitude; set => latitude = value; }
        public string Longitude { get => longitude; set => longitude = value; }
        public int TonnageActuel { get => tonnageActuel; set => tonnageActuel = value; }
        public int TonnageGT { get => tonnageGT; }
        public int TonnageDWT { get => tonnageDWT; }

        protected Navire(string imo, string nom, string latitude, string longitude, int tonnageActuel, int tonnageGT, int tonnageDWT)
        {
            if (!Regex.IsMatch(imo, @"^IMO[\d]{7}$"))
            {
                throw new GestionPortExceptions("Erreur : IMO non valide");
            }

            this.imo = imo;
            this.nom = nom;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.TonnageActuel = tonnageActuel;
            this.tonnageGT = tonnageGT;
            this.tonnageDWT = tonnageDWT;
        }
    }
}
