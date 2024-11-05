using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
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
        public void Test_AddLocation_ThrowsException()
        {
            InitializeRoute();

            string location = "Mount Doom";
            double distance = 38;
            bool isStop = true;

            //check if the location excist that the program throws an exception
            var ex = Assert.Throws<RouteException>(() => _route.AddLocation(location, distance, isStop));
            Assert.Equal($"{location} bestaat al", ex.Message);
        }
        
        [Fact]
        public void Test_AddLocation_ThrowsException_When_NoCapital()
        {
            InitializeRoute();

            string location = "mount Doom";
            double distance = 38;
            bool isStop = true;

            //check if the location excist that the program throws an exception
            var ex = Assert.Throws<RouteException>(() => _route.AddLocation(location, distance, isStop));
            Assert.Equal($"{location} begint niet met een hoofdletter", ex.Message);
        }

        [Fact]
        public void Test_GetDistance_correctly()
        {
            InitializeRoute();

            double totalDistance = _route.GetDistance();

            Assert.Equal(227, totalDistance);
        }

        [Fact]
        public void Test_GetDistance_Between_Locations_Correctly()
        {
            InitializeRoute();

            double distanceBetween = _route.GetDistance("Rivendel", "Edoras");
            double distanceBetween2 = _route.GetDistance("Rivendel", "Helm's Deep");

            Assert.Equal(33, distanceBetween);
            Assert.Equal(38, distanceBetween2);
        }

        [Fact]
        public void Test_GetDistance_Between_ThrowsException()
        {
            InitializeRoute();

            var ex = Assert.Throws<RouteException>(() => _route.GetDistance("Ivendel", "Edoras"));
            Assert.Equal("-1 or 2 doesn't excist in the current route.", ex.Message);
        }

        [Fact]
        public void Test_HasStop_Correctly() // Last stop niet laten meetellen? 
        {
            InitializeRoute();

            bool hasStop = _route.HasStop("Rivendel");
            bool hasStop2 = _route.HasStop("Helm's Deep");
            //bool hasStop3 = _route.HasStop("Mount Doom");

            Assert.True(hasStop);
            Assert.False(hasStop2);
            //Assert.True(hasStop3);
        }

        [Fact]
        public void Test_HasLocation_Correctly()
        {
            InitializeRoute();

            Assert.False(_route.HasLocation("FakeLocation"));
            Assert.True(_route.HasLocation("Edoras"));
        }

        [Fact]
        public void Test_SetDistance_Correctly()
        {
            InitializeRoute();

            _route.SetDistance(20.0, "Helm's Deep","Isengard");

            Assert.Equal(20.0, _route._segmentList[4].Distance);
        }

        [Fact]
        public void Test_ShowFullRoute_Correctly()
        {
            InitializeRoute();
            var (start, fullRoute) = _route.ShowFullRoute();

            Assert.Equal("The Shire", start);
            Assert.Equal("Rivendel", fullRoute[1].location);
            Assert.Equal(60.0 , fullRoute[1].distance);
            Assert.Equal("Minas Morgul" , fullRoute[6].location);
            Assert.Equal(8.0 , fullRoute[6].distance);
            Assert.Equal(8, fullRoute.Count);
        }

        [Fact]
        public void Test_ShowFullRoute_Between_Distances_Correctly()
        {
            InitializeRoute();
            var (start, fullRoute) = _route.ShowFullRoute("Rivendel", "Minas Tirith");

            Assert.Equal("Rivendel", start);
            Assert.Equal("Edoras", fullRoute[1].location);
            Assert.Equal(33.0, fullRoute[1].distance);
            Assert.Equal("Isengard", fullRoute[3].location);
            Assert.Equal(30.0, fullRoute[3].distance);
            Assert.Equal (4, fullRoute.Count);
        }

        [Fact]
        public void Test_ShowLocations_Correctly()
        {
            InitializeRoute();
            List<string> locations = _route.ShowLocations();

            Assert.Equal(9, locations.Count);
            Assert.Equal("Bree", locations[1]);
            Assert.Equal("Minas Morgul", locations[7]);
        }

        [Fact]
        public void Test_ShowRoute_Correctly()
        {
            InitializeRoute();
            var (start, route) = _route.ShowRoute();

            Assert.Equal(6, route.Count);
            Assert.Equal("The Shire", start);
            Assert.Equal("Edoras", route[2].location);
            Assert.Equal(35, route[3].distance);
            Assert.Equal("Mount Doom", route[5].location);
        }

        [Fact]
        public void Test_ShowRoute_Between_Given_Locations_Correctly()
        {
            InitializeRoute();
            var (start, route) = _route.ShowRoute("Rivendel", "Minas Tirith");

            Assert.Equal(3, route.Count);
            Assert.Equal("Rivendel", start);
            Assert.Equal("Minas Tirith", route[2].location);
            Assert.Equal(35, route[1].distance);
        }

        [Fact]
        public void Test_ShowRoutes_ThrowsException()
        {
            InitializeRoute();

            var ex = Assert.Throws<RouteException>(() => _route.ShowRoute("Divendel", "Minas Tirith"));
            Assert.Equal("Divendel or Minas Tirith doesn't excist in the current route.", ex.Message);

            var ex1 = Assert.Throws<RouteException>(() => _route.ShowRoute("Minas Tirith", "Rivendel"));
            Assert.Equal("End location must come after start location.", ex1.Message);

            var ex2 = Assert.Throws<RouteException>(() => _route.ShowRoute("Helm's Deep", "Minas Tirith"));
            Assert.Equal("Helm's Deep or Minas Tirith isn't a stop in the current route.", ex2.Message);
        }

        [Fact]
        public void Test_ShowStops_Correctly()
        {
            InitializeRoute();

            List<string> stops = _route.ShowStops();

            Assert.Equal(7, stops.Count);
            Assert.Equal("The Shire", stops[0]);
            Assert.Equal("Isengard", stops[4]);
            Assert.Equal("Minas Tirith", stops[5]);
        }

        [Fact]
        public void Test_UpdateLocation_Correctly()
        {
            // Test als het de eerste is
            InitializeRoute();
            _route.UpdateLocation("The Shire", "Shire", true);

            Assert.Equal("Shire", _route._segmentList[0].StopA.Name);
            Assert.True(_route._segmentList[0].StopA.IsStop);

            //test bij normaal gebruik
            InitializeRoute(); //reset de route
            _route.UpdateLocation("Rivendel", "Vendel", false);

            Assert.Equal("Vendel", _route._segmentList[1].StopB.Name);
            Assert.False(_route._segmentList[1].StopB.IsStop);
            Assert.Equal("Vendel", _route._segmentList[2].StopA.Name);

        }

        //TODO: UpdateLocation exception tests schrijven

        [Fact]
        public void Test_InsertLocation_Correctly()
        {
            InitializeRoute();
            _route.InsertLocation("Mordor",15.0,"Isengard",true);

            Assert.Equal(15.0, _route._segmentList[5].Distance);
            Assert.Equal(13.0, _route._segmentList[6].Distance);
            Assert.Equal("Mordor", _route._segmentList[5].StopB.Name); // Ensure one location was added
            Assert.Equal("Mordor", _route._segmentList[6].StopA.Name);
        }

        [Fact]
        public void Test_InsertLocation_ThrowsException()
        {
            InitializeRoute();

            var ex = Assert.Throws<RouteException>(() => _route.InsertLocation("Mordor", 15.0, "Sisengard", true));
            Assert.Equal("The fromLocation doesn't exist.", ex.Message);

            var ex1 = Assert.Throws<RouteException>(() => _route.InsertLocation("Mordor", 150.0, "Isengard", true));
            Assert.Equal("Distance can not be smaller/equal to 0", ex1.Message);
        }

        [Fact]
        public void Test_RemoveLocation_Correctly()
        {
            //remove first location
            InitializeRoute();
            _route.RemoveLocation("The Shire");

            Assert.Equal(7, _route._segmentList.Count);

            //remove last location
            InitializeRoute();
            _route.RemoveLocation("Mount Doom");

            Assert.Equal(7, _route._segmentList.Count);

            //remove somewhere in the middle
            InitializeRoute();
            _route.RemoveLocation("Isengard");

            Assert.Equal(7, _route._segmentList.Count);
            Assert.Equal(58.0, _route._segmentList[4].Distance);
            Assert.Equal("Helm's Deep", _route._segmentList[4].StopA.Name);
            Assert.Equal("Minas Tirith", _route._segmentList[4].StopB.Name);
        }

        [Fact]
        public void Test_RemoveLocation_ThrowsException()
        {
            InitializeRoute();

            var ex = Assert.Throws<RouteException>(() => _route.RemoveLocation("SIsengard"));
            Assert.Equal("The fromLocation doesn't exist.", ex.Message);
        }
    }
}
