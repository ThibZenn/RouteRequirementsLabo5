﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteRequirementsBL.Models
{
    public class LocationSegment : Location
    {
        public LocationSegment(string name, bool isStop) : base(name)
        {
            IsStop = isStop;
        }
            
        public bool IsStop { get; set; }
    }
}
