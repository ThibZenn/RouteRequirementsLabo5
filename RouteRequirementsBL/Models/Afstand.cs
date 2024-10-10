using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteRequirementsBL.Exceptions;

namespace RouteRequirementsBL.Models
{
    public class Afstand
    {
        private double _afstandTotVorigeStop;

        public double AfstandTotVorigeStop
        {
            get { return _afstandTotVorigeStop; }
            set
            {
                if (value < 0)
                {
                    throw new LocatieException($"AfstandTotVorigeStop = {value} is kleiner dan 0");
                }
                _afstandTotVorigeStop = value;
            }
        }
        public string VorigeStop { get; set; }

        public Afstand(double afstandTotVorigeStop, string vorigeStop)
        {
            AfstandTotVorigeStop = afstandTotVorigeStop;
            VorigeStop = vorigeStop;
        }
    }
}
