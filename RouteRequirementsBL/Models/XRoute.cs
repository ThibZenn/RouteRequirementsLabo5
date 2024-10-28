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
        
        internal List<Segment> _segmentList { get; set; }

        internal XRoute() // op internal zetten zodat er niet van buitenaf een instantie van Route gemaakt kan worden
        {
            
        }

        public void AddLocation(string location, double distance, bool isStop)
        {
            if (_segmentList.Exists(x => (x.StopA.Name == location) || (x.StopB.Name == location)))
            {
                throw new RouteException($"{location} bestaat al");
            }

            _segmentList.Add(new Segment(distance, _segmentList[_segmentList.Count - 1].StopB, new SegmentLocatie(location,isStop)));
        }

        public double GetDistance() //Get the total distance
        {
            double distance = 0;

            foreach (Segment locatie in _segmentList)
            {
                distance += locatie.Distance;
            }
            return distance;
        }

        public double GetDistance(string startLocation, string endLocation)
        {
            double totalDistance = 0;

            int startIndex = _segmentList.FindIndex(x => x.StopA.Name == startLocation);
            int endIndex = _segmentList.FindIndex(x => x.StopB.Name == endLocation);

            return totalDistance = _segmentList
                .Skip(startIndex)
                .Take(endIndex - startIndex) // neem de afstanden van het gevraagde deel.
                .Sum(x => x.Distance); //opsomming afstanden
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
            return _segmentList.Exists(x => (x.StopA.Name == location) || (x.StopB.Name == location));
        }

        public void InsertLocation(string location, double distance, string fromLocation, bool isStop)
        {
            
            int indexInsertLocation = _segmentList.FindIndex( x => x.StopA.Name == fromLocation +1); //we doen +1 want zonder zouden we de index hebben van de fromlocatie zelf.

            //TODO insertLocation toevoegen
        }

        public void RemoveLocation(string location)
        {
            int indexOfLocation = _segmentList.FindIndex(x => (x.StopA.Name == location) || (x.StopB.Name == location));
            //distance van te verwijderen segment opslaan
            double distance = _segmentList[indexOfLocation].Distance;
            //segment op die index verwijderen
            _segmentList.RemoveAt(indexOfLocation);
            //segment +1 moet aangepast worden
            _segmentList[indexOfLocation + 1].Distance += distance;
            _segmentList[indexOfLocation + 1].StopA = _segmentList[indexOfLocation].StopB;

        }

        public void SetDistance(double distance, string location1, string location2)
        {
            foreach (Segment item in _segmentList)
            {
                if (item.StopA.Name == location1 && item.StopB.Name == location2)
                {
                    item.Distance = distance;
                }
            }
        }

        public (string start, List<(double distance, string location)>) ShowFullRoute() //OPM: mag de beginStop nog in de list voorkomen?
        {
            //nieuwe instantie van de tuple lijst aanmaken
            List<(double distance, string location)> route = new List<(double distance, string location)>();

            //loop over lijst van segmenten en items toewijzen aan de list.
            foreach (Segment item in _segmentList)
            {
                route.Add((item.Distance, item.StopB.Name));
            }

            return (_segmentList[0].StopA.Name, route);
        }

        public (string start, List<(double distance, string location)>) ShowFullRoute(string startLocation, string endLocation)
        {
            //nieuwe instantie van de tuple lijst maken
            List<(double distance, string location)> route = new List<(double distance, string location)> ();

            int startIndex = _segmentList.FindIndex(x => x.StopA.Name == startLocation);
            int endIndex = _segmentList.FindIndex(x => x.StopB.Name == endLocation);

            for (int i = startIndex; i < endIndex; i++)
            {
                route.Add((_segmentList[i].Distance, _segmentList[i].StopB.Name));
            }

            return (_segmentList[0].StopA.Name,route);

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
            double accumulatedDistance = 0;
            //instantie maken van de tuple list
            List<(double distance, string location)> route = new List<(double distance, string location)>();
            //lopen over de list en kijken of de locatie's op een route een stop zijn of niet.
            for (int i = 0; i < _segmentList.Count - 1; i++)
            {
                accumulatedDistance += _segmentList[i].Distance;

                if (_segmentList[i + 1].StopA.IsStop)
                {
                    route.Add((accumulatedDistance, _segmentList[i + 1].StopA.Name));
                    accumulatedDistance = 0;
                }
            }

            return (_segmentList[0].StopA.Name, route);
        }

        public (string start, List<(double distance, string location)>) ShowRoute(string startLocation, string endLocation)
        {

            //linq statements om de index te vinden van startIndex en endIndex
            int startIndex = _segmentList.FindIndex(loc => loc.StopA.Name == startLocation);
            int endIndex = _segmentList.FindIndex(loc => loc.StopB.Name == endLocation);

            double accumulatedDistance = 0;
            //instantie maken van de tuple list
            List<(double distance, string location)> route = new List<(double distance, string location)>();
            //lopen over de dictionary en kijken of de locatie's op een route een stop zijn of niet.

            for (int i = startIndex; i < endIndex; i++)
            {
                accumulatedDistance += _segmentList[i].Distance;

                if (_segmentList[i + 1].StopA.IsStop)
                {
                    route.Add((accumulatedDistance, _segmentList[i + 1].StopA.Name));
                    accumulatedDistance = 0;
                }
            }

            return (_segmentList[0].StopA.Name, route);
        }

        public List<string> ShowStops()
        {
            List<string> stops = new List<string> {
                    _segmentList[0].StopA.Name, //Eerste locatie is altijd een stop.
                };
            
            //Teller bijhouden zodat we weten op welke index we moeten zoeken voor de naam.
            int indexTeller = 0;

            foreach (Segment item in _segmentList) //opm: beginstation is altijd een stop?
            {
                if (item.StopB.IsStop == true)
                {
                    stops.Add(_segmentList[indexTeller].StopB.Name);
                }
                indexTeller++;
            }
            return stops;
        }

        public void UpdateLocation(string location, string newName, bool isStop)
        {
            int indexTeller = 0;

            foreach (var item in _segmentList)
            {
                if (item.StopA.Name == location)
                {
                    item.StopA.Name = newName;
                    _segmentList[indexTeller].StopA.IsStop = isStop;
                } else if (item.StopB.Name == location)
                {
                    item.StopB.Name = newName;
                    _segmentList[indexTeller].StopB.IsStop = isStop;
                }
                indexTeller++;
            }
        }
    }
}
