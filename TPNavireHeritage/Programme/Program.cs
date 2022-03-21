using NavireHeritage.ClassesMetier;
using NavireHeritage.ClassesTechniques;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavireHeritage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(ConfigurationManager.AppSettings["valSuperTanker"]);

            Port port = new Port("Marseille", "43.2976 N", "5.3471 E", 4, 3, 2, 4);
            Test.ChargementInitial(port);
            Console.WriteLine(port);
            Test.AfficheAttendus(port);
            Test.TestEnregistrerArriveePrevue(port, new Cargo("IMO9780859", "CMA CGM A. LINCOLN", "43.43279 N", "134.76258 W", 140872, 148992, 123000, "marchandise diverses"));

            Test.TestEnregistrerArrivee(port, "IMO9241061");
            Test.TestEnregistrerArrivee(port, "IMO0000000");
            Test.TestEnregistrerArrivee(port, "IMO9241061");

            Test.TestEnregistrerArrivee(port, "IMO9334076");
            Test.TestEnregistrerArrivee(port, "IMO9197832");
            Test.TestEnregistrerArrivee(port, "IMO9220952");
            Test.TestEnregistrerArrivee(port, "IMO9379715");

            /*
             * On essaie de faire partir un navire qui n'est pas arrivé
             */
            Test.TesterEnregistrerDepart(port, "IMO9197822");

            /*
             * On fait partir le navire de croisiere
             * Il y a toujours les super tanker en attente
            */

            Test.TesterEnregistrerDepart(port, "IMO9241061");

            /*
             * On fait partir le tanker
             * Il y a toujours les super tanker en attente
            */

            Test.TesterEnregistrerDepart(port, "IMO9334076");

            /*
             * On fait partir le super tanker, celui en attente doit arriver
            */

            Test.TesterEnregistrerDepart(port, "IMO9197832");

            /*
             * Dans ce test, on enregistrer l'arrivée de 4 cargos
             *  et il y a de la place
             */

            Test.TestEnregistrerArriveePrevue(port, new Cargo("IMO9755933", "MSC DINA", "39.74224 N", "5.99304 E", 193489, 202036, 176000, "Matériel industriel"));
            Test.TestEnregistrerArriveePrevue(port, new Cargo("IMO9204506", "HOLANDIA", "40.74844 N", "6.87008 E", 8737, 9113, 7500, "Marchandises diverses"));
            Test.TestEnregistrerArrivee(port, "IMO9780859");
            Test.TestEnregistrerArrivee(port, "IMO9250098");
            Test.TestEnregistrerArrivee(port, "IMO9502910");
            Test.TestEnregistrerArrivee(port, "IMO9755933");

            /*
             * Dans ce test on va enregistrer l'arrivée d'un cargo qui sera mis en attente
             */

            Test.TestEnregistrerArrivee(port, "IMO9204506");

            /*
             * Dans ce test on va enregistrer le départ d'un super tanker
             * Le cargo devrait rester en attente
             */

            Test.TesterEnregistrerDepart(port, "IMO9220952");

            /*
             * Dans ce test on va enregistrer le départ d'un cargo
             * Le cargo devrait en attente devrait passer dans les navires arrivés
             */

            Test.TesterEnregistrerDepart(port, "IMO9755933");

            Console.WriteLine(port);

            Croisiere lol = new Croisiere("IMO9999999", "RMS QUEEN MARY 2", "43.43279 N", "134.76258 W", 140872, 148992, 123000, '8', 500);
            List<Passager> passagerss = new List<Passager>();
            passagerss.Add(new Passager("1", "Jonathan", "Simon", "Français" ));
            passagerss.Add(new Passager("2", "Ayme", "Pignon", "Français"));
            passagerss.Add(new Passager("3", "Jeremy", "Sarkissian", "Français"));
            lol.embarquer(passagerss);
            lol.embarquer(passagerss);
            lol.debarquer(passagerss);

            Console.WriteLine(port.getMilesToArrival(new Croisiere("IMO9999999", "RMS QUEEN MARY 2", "43.43279 N", "134.76258 W", 140872, 148992, 123000, '8', 500)));

            Console.ReadKey();
        }
    }
}
