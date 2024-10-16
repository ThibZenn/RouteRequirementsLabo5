using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteRequirementsBL.Models
{
    public class SegmentLocatie
    {
        public SegmentLocatie(bool isStop)
        {
            IsStop = isStop;
        }

        public bool IsStop { get; set; }
    }
}
