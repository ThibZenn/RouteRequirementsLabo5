using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RouteRequirementsBL.Exceptions;
using RouteRequirementsBL.Interfaces;

namespace RouteRequirementsBL.Models
{
    public class XRoute : IRoute
    {
        
        public List<RouteSegment> _segmentList; //TODO private zetten, maar nu staat hij public voor de unit testen.

        internal XRoute() // op internal zetten zodat er niet van buitenaf een instantie van Route gemaakt kan worden
        {
            _segmentList = new List<RouteSegment>();
        }

        public void AddLocation(string location, double distance, bool isStop)
        {
            if (_segmentList.Exists(x => (x.StopA.Name == location) || (x.StopB.Name == location)))
            {
                throw new RouteException($"{location} bestaat al");
            }

            _segmentList.Add(new RouteSegment(distance, _segmentList[_segmentList.Count - 1].StopB, new LocationSegment(location,isStop)));
        }

        public double GetDistance() //Get the total distance
        {
            double distance = 0;

            foreach (RouteSegment locatie in _segmentList)
            {
                distance += locatie.Distance;
            }
            return distance;
        }

        public double GetDistance(string startLocation, string endLocation)
        {
            double totalDistance = 0;

            int startIndex = _segmentList.FindIndex(x => x.StopA.Name.ToLower() == startLocation.ToLower());
            int endIndex = _segmentList.FindIndex(x => x.StopB.Name.ToLower() == endLocation.ToLower());

            if (startIndex < 0 || endIndex < 0)
            {
                throw new RouteException($"{startIndex} or {endIndex} doesn't excist in the current route.");
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                totalDistance += _segmentList[i].Distance;
            }

            return totalDistance;
        }

        public bool HasLocation(string location) // kijken of de locatie in de route zit.
        {
            for (int i = 0; i < _segmentList.Count - 1; i++)
            {
                if (_segmentList[i].StopA.Name == location)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasStop(string location)
        {
            
            return _segmentList.Where(x => x.StopA.Name == location).Select( x => x.StopA.IsStop).First();
        }

        public void InsertLocation(string location, double distance, string fromLocation, bool isStop)
        {
            
            int index = _segmentList.FindIndex( x => x.StopA.Name == fromLocation); 

            if (index < 0)
            {
                throw new RouteException("The fromLocation doesn't exist.");
            }

            double currentSegmentDistance = _segmentList[index].Distance;
            double updatedDistance = currentSegmentDistance - distance;

            if (updatedDistance <= 0)
            {
                throw new RouteException("Distance can not be smaller/equal to 0");
            }

            //nieuw segment maken
            LocationSegment insertFirstLocation = new LocationSegment(location, isStop);
            RouteSegment insertSegment = new RouteSegment( updatedDistance, insertFirstLocation, _segmentList[index].StopB);
            _segmentList.Insert(index + 1 , insertSegment); // +1 zodat we op de juiste locatie gaan inserten

            //vorig segment aanpassen
            _segmentList[index].StopB = insertFirstLocation;
            _segmentList[index].Distance = distance;
        }

        public void RemoveLocation(string location) //TODO: Exceptions schrijven
        {
            //eerste locatie
            if (_segmentList[0].StopA.Name == location)
            {
                _segmentList.RemoveAt(0); //Het gehele eerste segment gaat weg en we moeten geen distance herrekenen.
                return;
            }

            int indexOfLocation = _segmentList.FindIndex(x => x.StopB.Name == location);

            if (indexOfLocation < 0)
            {
                throw new RouteException("The fromLocation doesn't exist.");
            }

            if (indexOfLocation > 0 && indexOfLocation < _segmentList.Count - 1)
            {
                //distance van te verwijderen segment opslaan
                double distance = _segmentList[indexOfLocation].Distance + _segmentList[indexOfLocation + 1].Distance;

                //nieuw segment aanmaken
                _segmentList[indexOfLocation] = new RouteSegment(distance, _segmentList[indexOfLocation].StopA, _segmentList[indexOfLocation + 1].StopB);
                //segment op die index verwijderen
                _segmentList.RemoveAt(indexOfLocation +1);
            }
            else if (indexOfLocation == _segmentList.Count - 1) //als het de laatste locatie is
            {
                _segmentList.RemoveAt(indexOfLocation);
            }

        }

        public void SetDistance(double distance, string location1, string location2)
        {
            if (!_segmentList.Exists(x => (x.StopA.Name == location1) || (x.StopA.Name == location2)))
            {
                throw new RouteException($"{location1} of {location2} doesn't excist in the current route");
            }

            foreach (RouteSegment item in _segmentList)
            {
                if (item.StopA.Name == location1 && item.StopB.Name == location2)
                {
                    item.Distance = distance;
                }
            }
        }

        public (string start, List<(double distance, string location)>) ShowFullRoute() 
        {
            //nieuwe instantie van de tuple lijst aanmaken
            List<(double distance, string location)> route = new List<(double distance, string location)>();

            //loop over lijst van segmenten en items toewijzen aan de list.
            foreach (RouteSegment item in _segmentList)
            {
                route.Add((item.Distance, item.StopB.Name));
            }

            return (_segmentList[0].StopA.Name, route);
        }

        public (string start, List<(double distance, string location)>) ShowFullRoute(string startLocation, string endLocation) //TODO: check of locations bestaan + de volgorde van de meegeven route moet juist staan + start en stop moeten een stop locatie zijn.
        {
            //nieuwe instantie van de tuple lijst maken
            List<(double distance, string location)> route = new List<(double distance, string location)> ();

            int startIndex = _segmentList.FindIndex(x => x.StopB.Name == startLocation);
            int endIndex = _segmentList.FindIndex(x => x.StopB.Name == endLocation);

            for (int i = startIndex; i < endIndex; i++)
            {
                route.Add((_segmentList[i].Distance, _segmentList[i].StopB.Name));
            }

            return (startLocation,route);

        }

        public List<string> ShowLocations() //Alle locaties van een route tonen.
        {
            List<string> locations = new List<string>();
            locations.Add(_segmentList[0].StopA.Name); //eerste stop toevoegen

            foreach (var item in _segmentList)
            {
                locations.Add(item.StopB.Name);
            }
            return locations;
        }

        public (string start, List<(double distance, string location)>) ShowRoute() // De hele route behalve degenen die geen stop zijn
        {
            string startLocation = _segmentList[0].StopA.Name;
            string endLocation = _segmentList[_segmentList.Count - 1].StopB.Name;

            return ShowRoute(startLocation, endLocation);
        }

        public (string start, List<(double distance, string location)>) ShowRoute(string startLocation, string endLocation)
        {

            //linq statements om de index te vinden van startIndex en endIndex
            int startIndex = _segmentList.FindIndex(loc => loc.StopA.Name == startLocation);
            int endIndex = _segmentList.FindIndex(loc => loc.StopB.Name == endLocation);

            if (startIndex < 0 || endIndex < 0)
            {
                throw new RouteException($"{startLocation} or {endLocation} doesn't excist in the current route.");
            }

            if (endIndex < startIndex)
            {
                throw new RouteException("End location must come after start location.");
            }

            if ((!_segmentList[startIndex].StopA.IsStop) || (!_segmentList[endIndex].StopB.IsStop))
            {
                throw new RouteException($"{startLocation} or {endLocation} isn't a stop in the current route.");
            }

            double accumulatedDistance = 0;
            //instantie maken van de tuple list
            List<(double distance, string location)> route = new List<(double distance, string location)>();
            //lopen over de dictionary en kijken of de locatie's op een route een stop zijn of niet.

            for (int i = startIndex; i <= endIndex; i++)
            {
                accumulatedDistance += _segmentList[i].Distance;

                if (_segmentList[i].StopB.IsStop)
                {
                    route.Add((accumulatedDistance, _segmentList[i].StopB.Name));
                    accumulatedDistance = 0;
                }
            }

            return (startLocation, route);
        }

        public List<string> ShowStops()
        {
            HashSet<string> stops = new HashSet<string> (); // op deze manier zorgen we ervoor dat er geen dubbels in onze lijst voorkomen. Hashset aanvaard enkel maar unieke entries.

            for (int i = 0; i < _segmentList.Count; i++)
            {
                if (_segmentList[i].StopA.IsStop)
                {
                    stops.Add(_segmentList[i].StopA.Name);
                }
                if (_segmentList[i].StopB.IsStop)
                {
                    stops.Add(_segmentList[i].StopB.Name);
                }
            }
            return stops.ToList();
        }

        public void UpdateLocation(string location, string newName, bool isStop) 
        {
            //locationsegment object voor segment aan te passen
            LocationSegment updateLocation = new LocationSegment(newName, isStop);

            //check if the newName already excists in the current context
            if (_segmentList.Any(x => (x.StopB.Name == newName) || (x.StopA.Name == newName))) { throw new RouteException($"{newName} already exists in the current route."); }

            // als het de eerste locatie van de route is
            if (_segmentList[0].StopA.Name == location)
            {
                _segmentList[0].StopA = updateLocation;
                return;
            }
            // als het niet de eerste locatie is, de juiste index opzoeken
            int indexLoc = _segmentList.FindIndex(x => x.StopB.Name == location);

            //check of de up te daten locatie bestaat in de route
            if (indexLoc == -1) { throw new RouteException($"{location} doesn't excist in the current route."); }
            

            //beide segmenten aanpassen (locatie zal bestaan in 2 segmenten)
            _segmentList[indexLoc].StopB = updateLocation;

            if (indexLoc < _segmentList.Count - 1) //als het niet de laatste locatie is
            {
                _segmentList[indexLoc + 1].StopA = updateLocation;
            }
        }
    }
}
