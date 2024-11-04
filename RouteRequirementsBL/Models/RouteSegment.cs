using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteRequirementsBL.Exceptions;

namespace RouteRequirementsBL.Models
{
    public class RouteSegment
    {
        private double _distance;

        public double Distance
        {
            get { return _distance; }
            set
            {
                if (value < 0)
                {
                    throw new LocationException($"DistancePreviousStop = {value} is negative");
                }
                _distance = value;
            }
        }
        public LocationSegment StopA { get; set; }
        public LocationSegment StopB { get; set; }

        public RouteSegment(double distance, LocationSegment stopA, LocationSegment stopB)
        {
            Distance = distance;
            StopA = stopA;
            StopB = stopB;
        }
    }
}
