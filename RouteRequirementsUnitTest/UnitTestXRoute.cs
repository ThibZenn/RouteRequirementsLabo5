using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using RouteRequirementsBL.Exceptions;
using RouteRequirementsBL.Models;

namespace RouteRequirementsUnitTest
{
    public class UnitTestXRoute
    {
        private XRoute _route;
        private List<string> _locations = new List<string> {
            "The Shire", "Bree", "Rivendel", "Edoras", "Helm's Deep", "Isengard", "Minas Tirith", "Minas Morgul", "Mount Doom"
        };
        private List <bool> _stops = new List<bool> {
            true, true, true, true, false, true, true, false, true
        };
        private List<double> _distances = new List<double> {
            0,25,60,33,5,30,28,8,38
        };

        public UnitTestXRoute()
        {
            RouteFactory routeFactory = new RouteFactory();
            _route = routeFactory.BuildRoute(_locations,_stops,_distances);
        }

        [Theory]
        [InlineData("TestLocation", 15, true)]
        [InlineData(" ", 15 , true)]
        public void Test_addLocation(string location, double distance, bool isStop)
        {

        }

        [Theory]
        [InlineData("The Shire", 15, true)]
        [InlineData("The Shire", 15, true)]
        public void Test_addLocationInvalid(string location, double distance, bool isStop)
        {
            var ex = Assert.Throws<RouteException>(() => _route.AddLocation(location,distance,isStop)); //testen of er een exception wordt gegooid.
            Assert.Equal("id<0", ex.Message); // checken of de juiste foutboodschap megegeven is.
        }
    }
}
