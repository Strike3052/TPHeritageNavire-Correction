using Station.Interfaces;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionNavire.Exceptions;
using System.Configuration;

namespace NavireHeritage.ClassesMetier
{
    public class Port : IStationnable
    {
        static int valSuperTanker = Convert.ToInt32(ConfigurationManager.AppSettings["valSuperTanker"]);
        private string nom;
        private string latitude;
        private string longitude;
        private int nbPortique;
        private int nbQuaisTanker;
        private int nbQuaisSuperTanker;
        private int nbQuaisPassager;
        private Dictionary<string, Navire> navireAttendus = new Dictionary<string, Navire>();
        private Dictionary<string, Navire> navireArrives = new Dictionary<string, Navire>();
        private Dictionary<string, Navire> navirePartis = new Dictionary<string, Navire>();
        private Dictionary<string, Navire> navireEnAttente = new Dictionary<string, Navire>();

        public string Nom { get => nom; set => nom = value; }
        public string Latitude { get => latitude;}
        public string Longitude { get => longitude; }
        public int NbPortique { get => nbPortique; set => nbPortique = value; }
        public int NbQuaisTanker { get => nbQuaisTanker; set => nbQuaisTanker = value; }
        public int NbQuaisSuperTanker { get => nbQuaisSuperTanker; set => nbQuaisSuperTanker = value; }
        public int NbQuaisPassager { get => nbQuaisPassager; set => nbQuaisPassager = value; }
        public Dictionary<string, Navire> NavireAttendus { get => navireAttendus; }
        public Dictionary<string, Navire> NavireArrives { get => navireArrives; }
        public Dictionary<string, Navire> NavirePartis { get => navirePartis; }
        public Dictionary<string, Navire> NavireEnAttente { get => navireEnAttente; }

        public Port(string nom, string latitude, string longitude, int nbPortique, int nbQuaisTanker, int nbQuaisSuperTanker, int nbQuaisPassager)
        {
            this.nom = nom;
            this.latitude = latitude;
            this.longitude = longitude;
            this.nbPortique = nbPortique;
            this.nbQuaisTanker = nbQuaisTanker;
            this.nbQuaisSuperTanker = nbQuaisSuperTanker;
            this.nbQuaisPassager = nbQuaisPassager;
        }

        public void enregistrerArriveePrevue(Navire navire)
        {
            if (this.estAttendu(navire.Imo))
            {
                throw new Exception("Le navire " + navire.Imo + " est déja enregistré dans les navires attendus !");
            }
            this.navireAttendus.Add(navire.Imo, navire);
        }

        public void enregistrerArrivee(string imo)
        {
            // Premiere vérification : Le navire est-il attendu ?
            if (!this.navireAttendus.ContainsKey(imo))
            {
                // Le navire n'est pas attendu -> Renvoie une excpetion
                throw new GestionPortExceptions("Le navire " + imo + " n'est pas attendu");
            }
            // Le navire est attendu :
            if (NavireAttendus[imo] is INavCommercable)
            {
                // On gère les tankers et cargos dans une méthode privée -> GestionArrivéCommercable
                this.GestionArriveCommercable(imo);
            }
            else
            {
                // On gère les navires de croisières -> Permuttation du navire de attendu à arrivé
                this.PermuteAttenduArrive(imo);
            }
        }

        /// <summary>
        /// Méthode permettant de permuter le navire de attendu à arrivé
        /// </summary>
        /// <param name="imo">L'imo du navire à permuter</param>
        private void PermuteAttenduArrive(string imo)
        {
            this.navireArrives.Add(imo, this.navireAttendus[imo]);
            this.navireAttendus.Remove(imo);
        }

        /// <summary>
        /// Méthode permettant de permuter le navire de attendu à en attente 
        /// </summary>
        /// <param name="imo">L'imo du navire à permuter</param>
        private void PermuteAttenduEnAttente(string imo)
        {
            this.navireEnAttente.Add(imo, this.navireAttendus[imo]);
            this.navireAttendus.Remove(imo);
        }

        /// <summary>
        /// Méthode permettant de permuter le navire de arrivé a parti.
        /// </summary>
        /// <param name="imo">L'imo du navire a permuter</param>
        private void PermuteArriveParti(string imo)
        {
            this.navirePartis.Add(imo, this.navireArrives[imo]);
            this.navireArrives.Remove(imo);
        }

        /// <summary>
        /// Méthode permettant la gestion des tankers et cargos
        /// </summary>
        /// <param name="imo">Imo du navire</param>
        private void GestionArriveCommercable(string imo)
        {
            if (navireAttendus[imo] is Cargo)
            {
                // Ici, on teste si il reste des portiques disponibles
                GererArriveeAttente(imo, this.nbPortique, this.getNbCargoArrives());
            }
            else if (navireAttendus[imo] is Tanker)
            {
                // Méthode privée pour gérer les tankers
                GererArriveeTanker(imo);
            }
            else
            {
                throw new GestionPortExceptions("Le type du navire " + imo + " n'est pas compatible.");
            }
        }

        private void GererArriveeTanker(string imo)
        {
            if (navireAttendus[imo].TonnageGT <= valSuperTanker)
            {
                // Le navire est un tanker
                GererArriveeAttente(imo, NbQuaisTanker, this.getNbTankerArrives());
            }
            else
            {
                // Le navire est un super tanker
                GererArriveeAttente(imo, nbQuaisSuperTanker, this.getNbSuperTankerArrives());
            }
        }

        private void GererArriveeAttente(string imo, int nbQuais, int NbQuaisUtilises)
        {
            if (nbQuais > NbQuaisUtilises)
            {
                this.PermuteAttenduArrive(imo);
            }
            else
            {
                this.PermuteAttenduEnAttente(imo);
            }
        }

        public void enregistrerDepart(string imo)
        {
            if (!navireArrives.ContainsKey(imo))
            {
                throw new GestionPortExceptions("Le navire " + imo + " n'est pas dans le port");
            }
            this.PermuteArriveParti(imo);
            // Le navire est un navire ICommercable
            if (navirePartis[imo] is INavCommercable)
            {
                // On va gérer l'arrivé d'un navire en attente en remplacement du navire parti
                // On vas parcourir la liste navireEnAttente, jusqu'a trouver un navire en attente du même type que le navire parti
                int i = 0;
                bool trouve = false;
                while (i < this.navireEnAttente.Count && trouve == false)
                {
                    i++;
                    if (this.navireEnAttente.ElementAt(i).Value.GetType().FullName == navirePartis[imo].GetType().FullName)
                    {
                        trouve = true;
                        if (this.navireEnAttente.ElementAt(i).Value.TonnageGT <= valSuperTanker != navirePartis[imo].TonnageGT <= valSuperTanker)
                        {
                            trouve = false;
                        }
                        else if (this.navireEnAttente.ElementAt(i).Value.TonnageGT > valSuperTanker != navirePartis[imo].TonnageGT > valSuperTanker)
                        {
                            trouve = false;
                        }
                    }
                }
                if (trouve)
                {
                    this.navireArrives.Add(this.navireEnAttente.ElementAt(i).Value.Imo, this.navireEnAttente.ElementAt(i).Value);
                    this.navireEnAttente.Remove(this.navireEnAttente.ElementAt(i).Key);
                }
            }
        }

        public bool estAttendu(string imo)
        {
            return navireAttendus.ContainsKey(imo);
        }
        public bool estParti(string imo)
        {
            return navirePartis.ContainsKey(imo);
        }
        public bool estPresent(string imo)
        {
            return navireArrives.ContainsKey(imo);
        }
        public bool estEnAttente(string imo)
        {
            return navireEnAttente.ContainsKey(imo);
        }
        public void dechargement(int id)
        {

        }
        public void chargement(int id)
        {

        }
        public Navire getUnAttendu(string imo)
        {
            if (!this.estAttendu(imo))
            {
                throw new Exception("Le navire " + imo + " n'est pas attendu !");
            }
            return navireAttendus[imo];
        }
        public Navire getUnArrive(string imo)
        {
            if (!this.estPresent(imo))
            {
                throw new Exception("Le navire " + imo + " n'est pas arrivé !");
            }
            return navireArrives[imo];
        }
        public Navire getUnParti(string imo)
        {
            if (!this.estParti(imo))
            {
                throw new Exception("Le navire " + imo + " n'est pas parti !");
            }
            return navirePartis[imo];
        }

        public int getNbTankerArrives()
        {
            int total = 0;
            foreach (var navire in navireArrives.Values)
            {
                if(navire is Tanker && navire.TonnageGT<=valSuperTanker)
                {
                    total += 1;
                }
            }
            return total;
        }

        public int getNbSuperTankerArrives()
        {
            int total = 0;
            foreach (var navire in navireArrives.Values)
            {
                if (navire is Tanker && navire.TonnageGT > valSuperTanker)
                {
                    total += 1;
                }
            }
            return total;
        }

        public int getNbCargoArrives()
        {
            int total = 0;
            foreach (var navire in navireArrives.Values)
            {
                if (navire is Cargo)
                {
                    total += 1;
                }
            }
            return total;
        }

        public int getNbCroisiereArrives()
        {
            int total = 0;
            foreach (var navire in navireArrives.Values)
            {
                if (navire is Croisiere)
                {
                    total += 1;
                }
            }
            return total;
        }

        public double getMilesToArrival(Navire navire)
        {
            GeoCoordinate pointGPS1 = new GeoCoordinate(Convert.ToDouble(navire.Latitude.Split(' ')[0], new CultureInfo("en-US")), Convert.ToDouble(navire.Longitude.Split(' ')[0], new CultureInfo("en-US")));
            GeoCoordinate pointGPS2 = new GeoCoordinate(Convert.ToDouble(this.Latitude.Split(' ')[0], new CultureInfo("en-US")), Convert.ToDouble(this.Longitude.Split(' ')[0], new CultureInfo("en-US")));
            return pointGPS1.GetDistanceTo(pointGPS2);
        }

        private void AjoutNavireEnAttente(Navire navire)
        {

        }

        public override string ToString()
        {
            return "--------------------------------------------------------------------------------------\n" +
                    "Port de " + this.nom + "\n" +
                    "\t" + "Coordonnées GPS : " + this.latitude + " / " + this.longitude + "\n" +
                    "\t" + "Nb portiques : " + this.nbPortique + "\n" +
                    "\t" + "Nb quais croisiere : " + this.nbQuaisPassager + "\n" +
                    "\t" + "Nb quais tankers : " + this.nbQuaisTanker + "\n" +
                    "\t" + "Nb quais super tankers : " + this.nbQuaisSuperTanker + "\n" +
                    "\t" + "Nb Navires à quai : " + this.navireArrives.Count + "\n" +
                    "\t" + "Nb Navires attendus : " + this.navireAttendus.Count + "\n" +
                    "\t" + "Nb Navires partis : " + this.navirePartis.Count + "\n" +
                    "\t" + "Nb Navires en attente : " + this.navireEnAttente.Count + "\n" +
                    "\n" + "Nombre de cargos dans le port : " + this.getNbCargoArrives() +
                    "\n" + "Nombre de tankers dans le port : " + this.getNbTankerArrives() +
                    "\n" + "Nombre de super tankers dans le port : " + this.getNbSuperTankerArrives() + "\n" +
                    "--------------------------------------------------------------------------------------\n";
        }
    }
}
