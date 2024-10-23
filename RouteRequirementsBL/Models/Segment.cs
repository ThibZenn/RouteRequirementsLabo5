using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteRequirementsBL.Exceptions;

namespace RouteRequirementsBL.Models
{
    public class Segment
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
        public SegmentLocatie StopA { get; set; }
        public SegmentLocatie StopB { get; set; }

        public Segment(double distance, SegmentLocatie stopA, SegmentLocatie stopB)
        {
            Distance = distance;
            StopA = stopA;
            StopB = stopB;
        }
    }
}
