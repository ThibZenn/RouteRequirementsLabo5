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
        private Dictionary<string, Location> _locationDictionary;

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

        public bool HasLocation(string location)
        {
            throw new NotImplementedException(); //TODO method implementeren
        }

        public bool HasStop(string location)
        {
            throw new NotImplementedException(); //TODO method implementeren
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
            throw new NotImplementedException(); //TODO method implementeren
        }

        public (string start, List<(double distance, string location)>) ShowFullRoute()
        {
            throw new NotImplementedException(); //TODO method implementeren
        }

        public (string start, List<(double distance, string location)>) ShowFullRoute(string startLocation, string endLocation)
        {
            throw new NotImplementedException(); //TODO method implementeren
        }

        public List<string> ShowLocations()
        {
            throw new NotImplementedException(); //TODO method implementeren
        }

        public (string start, List<(double distance, string location)>) ShowRoute()
        {
            throw new NotImplementedException(); //TODO method implementeren
        }

        public (string start, List<(double distance, string location)>) ShowRoute(string startLocation, string endLocation)
        {
            throw new NotImplementedException(); //TODO method implementeren
        }

        public List<string> ShowStops()
        {
            throw new NotImplementedException(); //TODO method implementeren
        }

        public void UpdateLocation(string location, string newName, bool isStop)
        {
            throw new NotImplementedException(); //TODO method implementeren
        }
    }
}
