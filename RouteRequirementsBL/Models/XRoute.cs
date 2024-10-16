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
        
        public List<Location> Locations { get; set; }
        public List<Segment> Segment { get; set; }
        public List<SegmentLocatie> SegmentLocatie { get; set; }

        //public (List<Location>, List<Distance>, List<SegmentLocatie>) _Route {get; set;}


        internal XRoute() // op internal zetten zodat er niet van buitenaf een instantie van Route gemaakt kan worden
        {
            
        }

        public void AddLocation(string location, double distance, bool isStop)
        {
            Locations.Add(new Location(location));
            Segment.Add(new Segment(distance, Segment[(Segment.Count)-1].StopB, location));
            SegmentLocatie.Add(new SegmentLocatie(isStop));
        }

        public double GetDistance() //Get the total distance
        {
            double distance = 0;

            foreach (Segment locatie in Segment)
            {
                distance += locatie.Distance;
            }
            return distance;
        }

        public double GetDistance(string startLocation, string endLocation)
        {
            double totalDistance = 0;

            int startIndex = Segment.FindIndex(x => x.StopA == startLocation);
            int endIndex = Segment.FindIndex(x => x.StopB == endLocation);

            return totalDistance = Segment
                .Skip(startIndex)
                .Take(endIndex - startIndex) // neem de afstanden van het gevraagde deel.
                .Sum(x => x.Distance); //opsomming afstanden
        }

        public bool HasLocation(string location) // kijken of de locatie in de route zit.
        {
            foreach (Location item in Locations)
            {
                if (item.Name == location)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasStop(string location)
        {
            int indexOfLocation = Locations.FindIndex( x => x.Name == location);

            if (SegmentLocatie[indexOfLocation].IsStop) 
                return true;

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
            foreach (Segment item in Segment)
            {
                if (item.StopA == location1 && item.StopB == location2)
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
            foreach (Segment item in Segment)
            {
                route.Add((item.Distance, item.StopB));
            }

            return (Segment[0].StopA, route);
        }

        public (string start, List<(double distance, string location)>) ShowFullRoute(string startLocation, string endLocation)
        {
            //nieuwe instantie van de tuple lijst maken
            List<(double distance, string location)> route = new List<(double distance, string location)> ();

            int startIndex = Segment.FindIndex(x => x.StopA == startLocation);
            int endIndex = Segment.FindIndex(x => x.StopB == endLocation);

            for (int i = startIndex; i < endIndex; i++)
            {
                route.Add((Segment[i].Distance, Segment[i].StopB));
            }

            return (Segment[0].StopA,route);

        }

        public List<string> ShowLocations() //Alle locaties van een route tonen.
        {
            List<string> locations = new List<string>();

            foreach (var item in Locations)
            {
                locations.Add(item.Name);
            }
            return locations;
        }

        public (string start, List<(double distance, string location)>) ShowRoute() // De hele route behalve degenen die geen stop zijn
        {
            double accumulatedDistance = 0;
            //instantie maken van de tuple list
            List<(double distance, string location)> route = new List<(double distance, string location)>();
            //lopen over de dictionary en kijken of de locatie's op een route een stop zijn of niet.
            for (int i = 0; i < Locations.Count - 1; i++)
            {
                accumulatedDistance += Segment[i].Distance;

                if (SegmentLocatie[i + 1].IsStop)
                {
                    route.Add((accumulatedDistance, Locations[i + 1].Name));
                    accumulatedDistance = 0;
                }
            }

            return (Segment[0].StopB, route);
        }

        public (string start, List<(double distance, string location)>) ShowRoute(string startLocation, string endLocation)
        {

            //linq statements om de index te vinden van startIndex en endIndex
            int startIndex = Locations.FindIndex(loc => loc.Name == startLocation);
            int endIndex = Locations.FindIndex(loc => loc.Name == endLocation);

            double accumulatedDistance = 0;
            //instantie maken van de tuple list
            List<(double distance, string location)> route = new List<(double distance, string location)>();
            //lopen over de dictionary en kijken of de locatie's op een route een stop zijn of niet.

            for (int i = startIndex; i < endIndex; i++)
            {
                accumulatedDistance += Segment[i].Distance;

                if (SegmentLocatie[i + 1].IsStop)
                {
                    route.Add((accumulatedDistance, Locations[i + 1].Name));
                    accumulatedDistance = 0;
                }
            }

            return (Segment[0].StopB, route);
        }

        public List<string> ShowStops()
        {
            List<string> stops = new List<string>();
            //Teller bijhouden zodat we weten op welke index we moeten zoeken voor de naam.
            int indexTeller = 0;

            foreach (SegmentLocatie item in SegmentLocatie) //opm: beginstation is altijd een stop?
            {
                if (item.IsStop == true)
                {
                    stops.Add(Locations[indexTeller].Name);
                }
                indexTeller++;
            }
            return stops;
        }

        public void UpdateLocation(string location, string newName, bool isStop)
        {
            int indexTeller = 0;

            foreach (Location item in Locations)
            {
                if (item.Name == location)
                {
                    item.Name = newName;
                    SegmentLocatie[indexTeller].IsStop = isStop;
                }
                indexTeller++;
            }
        }
    }
}
