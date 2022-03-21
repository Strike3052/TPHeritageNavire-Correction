using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionNavire.Exceptions
{
    /// <summary>
    /// Class GestionPortExcpetion.
    /// </summary>
    class GestionPortExceptions : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GestionPortException"/> class.
        /// </summary>
        /// <param name="message">Message d'erreur.</param>
        public GestionPortExceptions(string message)
            : base("Erreur de : " + System.Environment.UserName + " le " + DateTime.Now.ToLocalTime() + "\n" + message)
        {
        }
    }
}
