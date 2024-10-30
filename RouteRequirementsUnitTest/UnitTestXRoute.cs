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
        private RouteFactory _routeFactory;
        

        public UnitTestXRoute()
        {
            _routeFactory = new RouteFactory();
            
        }

        private void InitializeRoute()
        {
            var _locations = new List<string> {"The Shire", "Bree", "Rivendel", "Edoras", "Helm's Deep", "Isengard", "Minas Tirith", "Minas Morgul", "Mount Doom"};
            var _stops = new List<bool> { true, true, true, true, false, true, true, false, true };
            var _distances = new List<double> { 0.0,25.0,60.0,33.0,5.0,30.0,28.0,8.0,38.0 };
            _route = _routeFactory.BuildRoute(_locations, _stops, _distances);
        }

        [Fact]
        public void Test_addLocation_correctly()
        {
            InitializeRoute();

            _route.AddLocation("TestLocation", 11.0, true);
            _route.AddLocation("TestLocation2", 37.0, false);

            Assert.Equal(10, _route._segmentList.Count);
            Assert.Equal(11.0, _route._segmentList[8].Distance);
            Assert.Equal(37.0, _route._segmentList[9].Distance);
            Assert.False(_route._segmentList[9].StopB.IsStop);
            Assert.True(_route._segmentList[9].StopA.IsStop);

        }

        [Fact]
        public void Test_addLocation_throwsException()
        {
            InitializeRoute();

        }
    }
}
