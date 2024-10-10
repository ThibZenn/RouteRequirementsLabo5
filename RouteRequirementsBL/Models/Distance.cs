using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteRequirementsBL.Exceptions;

namespace RouteRequirementsBL.Models
{
    public class Distance
    {
        private double _distancePreviousStop;

        public double DistancePreviousStop
        {
            get { return _distancePreviousStop; }
            set
            {
                if (value < 0)
                {
                    throw new LocationException($"DistancePreviousStop = {value} is negative");
                }
                _distancePreviousStop = value;
            }
        }
        //public string VorigeStop { get; set; }

        public Distance(double distancePreviousStop)
        {
            DistancePreviousStop = distancePreviousStop;
            //VorigeStop = vorigeStop;
        }
    }
}
