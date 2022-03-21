using NavireHeritage.ClassesMetier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Station.Interfaces
{
    interface ICroisierable
    {
        void embarquer(List<Passager> objects);
        void debarquer(List<Passager> objects);
    }
}
