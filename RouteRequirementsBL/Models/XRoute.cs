using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteRequirementsBL.Exceptions;
using RouteRequirementsBL.Interfaces;

namespace RouteRequirementsBL.Models
{
    public class XRoute : IRoute
    {
        // TODO: routeFactory instantie maken + juiste methode's aanroepen zodat we de route binnenkrijgen.
        public List<Location> Locations { get; set; }
        public List<Distance> Distances { get; set; }
        public List<IsStop> IsStop { get; set; }

        public void AddLocation(string location, double distance, bool isStop)
        {
            _locationDictionary.Add(location, new Location(location, new Distance(distance), isStop));
        }

        public double GetDistance() // in klasse Afstand schrijven?
        {
            List<Location> locationList = new List<Location>();
            locationList.AddRange(this._locationDictionary.Values);

            double distance = 0;

            foreach (Location locatie in locationList)
            {
                distance += locatie.Distance.DistancePreviousStop;
            }
            return distance;
        }

        public double GetDistance(string startLocation, string endLocation)
        {
            if (!_locationDictionary.ContainsKey(startLocation) && !_locationDictionary.ContainsKey(endLocation)) throw new RouteException("Start or End location doesn't excist in the current context"); //Check of de locaties bestaan

            List<Location> locationValuesList = new List<Location>(); // dictionary naar lijst maken om via de index aan de juiste values te geraken.
            locationValuesList.AddRange(this._locationDictionary.Values);

            int startIndex = _locationDictionary.Keys.ToList().IndexOf(startLocation); //TODO efficientere manier?
            int endIndex = _locationDictionary.Keys.ToList().IndexOf(endLocation); //TODO efficientere manier?

            double totalDistance = 0; //distance opteller resetten.

            for (int i = startIndex; i <= endIndex; i++)
            {
                totalDistance += locationValuesList[i].Distance.DistancePreviousStop;
            }

            return totalDistance;
        }

        public bool HasLocation(string location) // kijken of de locatie in de route zit.
        {
            if (!_locationDictionary.ContainsKey(location)) return false;
            return true;
        }

        public bool HasStop(string location)
        {
            if (HasLocation(location)) // check of de locatie bestaat in de huidige context
            {
                return _locationDictionary[location].IsStop;
            }
            return false;

        }

        public void InsertLocation(string location, double distance, string fromLocation, bool isStop)
        {
            throw new NotImplementedException(); //TODO method implementeren
        }

        public void RemoveLocation(string location)
        {
            throw new NotImplementedException(); //TODO method implementeren
        }

        public void SetDistance(double distance, string location1, string location2)
        {
            //TODO method implementeren
        }

        public (string start, List<(double distance, string location)>) ShowFullRoute()
        {
            string startLocation = _locationDictionary.First().Value.Name;

            List<(double distance, string location)> route = new List<(double distance, string location)>();
            foreach (Location location in _locationDictionary.Values)
            {
                route.Add((location.Distance.DistancePreviousStop, location.Name));
            }

            return (startLocation, route);
        }

        public (string start, List<(double distance, string location)>) ShowFullRoute(string startLocation, string endLocation)
        {
            List<(double distance, string location)> route = new List<(double distance, string location)>();
            // dictionary naar list omvormen zodat we de index kunnen zoeken met linq statements
            List<Location> locationsList = _locationDictionary.Values.ToList();
            //linq statements om de index te vinden van startIndex en endIndex
            int startIndex = locationsList.FindIndex(loc => loc.Name == startLocation);
            int endIndex = locationsList.FindIndex(loc => loc.Name == endLocation);

            if (_locationDictionary.ContainsKey(startLocation) && _locationDictionary.ContainsKey(endLocation))
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    route.Add((locationsList[i].Distance.DistancePreviousStop, locationsList[i].Name));
                }
            }

            return (startLocation, route);
        }

        public List<string> ShowLocations() //Alle locaties van een route tonen.
        {
            List<string> locations = new List<string>();

            foreach (var item in _locationDictionary.Values)
            {
                locations.Add(item.Name);
            }
            return locations;
        }

        public (string start, List<(double distance, string location)>) ShowRoute() // De hele route behalve degenen die geen stop zijn
        {
            string startLocation = _locationDictionary.First().Value.Name;
            //instantie maken van de tuple list
            List<(double distance, string location)> route = new List<(double distance, string location)>();
            //lopen over de dictionary en kijken of de locatie's op een route een stop zijn of niet.
            foreach (Location location in _locationDictionary.Values)
            {
                if (location.IsStop)
                {
                    route.Add((location.Distance.DistancePreviousStop, location.Name));
                }
            }

            return (startLocation, route);
        }

        public (string start, List<(double distance, string location)>) ShowRoute(string startLocation, string endLocation)
        {
            List<(double distance, string location)> route = new List<(double distance, string location)>();
            // dictionary naar list omvormen zodat we de index kunnen zoeken met linq statements
            List<Location> locationsList = _locationDictionary.Values.ToList();
            //linq statements om de index te vinden van startIndex en endIndex
            int startIndex = locationsList.FindIndex(loc => loc.Name == startLocation);
            int endIndex = locationsList.FindIndex(loc => loc.Name == endLocation);

            if (_locationDictionary.ContainsKey(startLocation) && _locationDictionary.ContainsKey(endLocation))
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (locationsList[i].IsStop)
                    {
                        route.Add((locationsList[i].Distance.DistancePreviousStop, locationsList[i].Name));
                    }
                }
            }

            return (startLocation, route);
        }

        public List<string> ShowStops()
        {
            List<string> stops = new List<string>();

            foreach (var item in _locationDictionary.Values) //opm: beginstation is altijd een stop?
            {
                if (item.IsStop == true)
                {
                    stops.Add(item.Name);
                }
            }
            return stops;
        }

        public void UpdateLocation(string location, string newName, bool isStop)
        {
            if (_locationDictionary.ContainsKey(location))
            {
                _locationDictionary[location].Name = newName;
                _locationDictionary[location].IsStop = isStop;
            }
        }
    }
}
