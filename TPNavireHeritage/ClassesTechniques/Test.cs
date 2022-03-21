using GestionNavire.Exceptions;
using NavireHeritage.ClassesMetier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavireHeritage.ClassesTechniques
{
    public abstract class Test
    {
        public static void ChargementInitial(Port port)
        {
            try
            {
                port.enregistrerArriveePrevue(new Cargo("IMO9780859", "CMA CGM A. LINCOLN", "43.43279 N", "134.76258 W", 140872, 148992, 123000, "marchandise diverses"));
                port.enregistrerArriveePrevue(new Cargo("IMO9250098", "CONTAINERSHIPS VII", "43.43279 N", "134.76258 W", 140872, 148992, 123000, "marchandise diverses"));
                port.enregistrerArriveePrevue(new Cargo("IMO9502910", "MAERSK EMERALD", "43.43279 N", "134.76258 W", 140872, 148992, 123000, "marchandise diverses"));

                port.enregistrerArriveePrevue(new Croisiere("IMO9241061", "RMS QUEEN MARY 2", "43.43279 N", "134.76258 W", 140872, 148992, 123000, '8', 500));
                port.enregistrerArriveePrevue(new Croisiere("IMO9803613", "MSC GRANDIOSA", "43.43279 N", "134.76258 W", 140872, 148992, 123000, '8', 500));
                port.enregistrerArriveePrevue(new Croisiere("IMO9351476", "CRUISE ROMA", "43.43279 N", "134.76258 W", 140872, 148992, 123000, '8', 500));

                port.enregistrerArriveePrevue(new Tanker("IMO9334076", "EJNAN", "43.43279 N", "134.76258 W", 98000, 100000, 123000, "marchandise diverses"));
                port.enregistrerArriveePrevue(new Tanker("IMO9380374", "CITRUS", "43.43279 N", "134.76258 W", 140872, 148992, 123000, "marchandise diverses"));
                port.enregistrerArriveePrevue(new Tanker("IMO9220952", "HARAD", "43.43279 N", "134.76258 W", 140872, 148992, 123000, "marchandise diverses"));
                port.enregistrerArriveePrevue(new Tanker("IMO9197832", "KALAMOS", "43.43279 N", "134.76258 W", 140872, 148992, 123000, "marchandise diverses"));
                port.enregistrerArriveePrevue(new Tanker("IMO9379715", "NEW DRAGON", "43.43279 N", "134.76258 W", 140872, 148992, 123000, "marchandise diverses"));

            } catch (GestionPortExceptions ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void AfficheAttendus(Port port)
        {
            Console.WriteLine("Liste des bateaux en attente de leurs arrivée : ");
            foreach (Navire navire in port.NavireAttendus.Values)
            {
                Console.WriteLine(navire.Imo + "  " + navire.Nom + " : " + navire.GetType().Name);
            }
        }

        public static void TestEnregistrerArriveePrevue(Port port, Navire navire)
        {
            try
            {
                port.enregistrerArriveePrevue(navire);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void TestEnregistrerArrivee(Port port, String imo)
        {
            try
            {
                port.enregistrerArrivee(imo);
                Console.WriteLine("Navire " + imo + " arrivé");
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void TesterEnregistrerDepart(Port port, String imo)
        {
            try
            {
                port.enregistrerDepart(imo);
                Console.WriteLine("Navire " + imo + " parti");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
