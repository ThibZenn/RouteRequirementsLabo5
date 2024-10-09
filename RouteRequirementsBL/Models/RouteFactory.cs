using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteRequirementsBL.Models
{
    internal class RouteFactory
    {
        public XRoute BuildRouteFromFile(string fileName) 
        {
            throw new NotImplementedException(); //TODO streamreader implementeren?
        }
        public XRoute BuildRoute(List<string> locations, List<bool> stops, List<double> distances) 
        {
            throw new NotImplementedException(); //TODO method implementeren
        }
        public XRoute ReverseRoute(XRoute route) 
        {
            throw new NotImplementedException(); //TODO method implementeren
        }
    }
}
