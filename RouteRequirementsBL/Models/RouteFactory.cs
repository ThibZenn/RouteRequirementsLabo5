using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RouteRequirementsBL.Exceptions;
using static System.Formats.Asn1.AsnWriter;

namespace RouteRequirementsBL.Models
{
    public class RouteFactory
    {
        public XRoute BuildRouteFromFile(string fileName) //TODO capital check?
        {
            //instantie van Xroute maken
            XRoute route = new XRoute();
            //Errors bijhouden
            string logFilePath = @"C:\Users\thiba\Documents\HOGENT\Semester2\ProgGevorderd1\Labo5\RouteRequirementsBL\Errors\ErrorLogBuildRouteFromFile.log";
            //Errorfile leegmaken
            if (File.Exists(logFilePath)) File.WriteAllText(logFilePath, string.Empty);

            List<string> locations = new List<string>();
            List< bool > stops = new List<bool>();
            List<double> distances = new List<double>();

            using (StreamReader sr = new StreamReader(fileName))
            {
                //Lijn die gelezen wordt.
                string line;
                int teller = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    try
                    {
                        // Regex pattern apart schrijven omdat we hem meerdere keren gebruiken.
                        string pattern = @"^(.+?),(\d+),(true|false)$"; //Regex eerste fileSoort
                        string pattern2 = @"^(.+?)\((stop|transit)\),(.+?)\((stop|transit)\),(\d+)$"; //Regex tweede FileSoort.
                        Match match = Regex.Match(line, pattern);
                        Match match2 = Regex.Match(line, pattern2);

                        // Als er een match is met het patroon gaan we verder
                        if (match.Success)
                        {
                            // omdat we groepen gemaakt hebben in onze regex kunnen we deze gemakkelijk toekennen.
                            string locationString = match.Groups[1].Value;

                            if (!double.TryParse(match.Groups[2].Value, out double distanceInt))
                            {
                                throw new RouteException($"Distance is't in the correct format in line : {line}");
                            }

                            if (!bool.TryParse(match.Groups[3].Value, out bool isStopBool))
                            {
                                throw new RouteException($"IsStop isn't a correct bool in line : {line}");
                            }
                            

                            // maak een nieuw object van locatie aan en voeg deze toe in de lijst.
                            locations.Add(locationString);
                            if (distanceInt != 0) //We willen de distance van de eerste locatie niet opslaan in onze lijst.
                            {
                                distances.Add(distanceInt);
                            }
                            distances.Add(distanceInt);

                        }

                        // Als er een match is met het tweede patroon gaan we verder
                        else if (match2.Success)
                        {
                            string locationA = match2.Groups[1].Value;
                            string locationB = match2.Groups[3].Value;
                        

                            if (!double.TryParse(match.Groups[2].Value, out double distance))
                            {
                                throw new RouteException($"Distance is't in the correct format in line : {line}");
                            }

                            // Tip van Aster, om de 'transit' en 'stop' op te vangen met de correcte bool een switch gebruiken.

                            //stops eerste locatie
                            bool isStartStop;

                            switch (match2.Groups[2].Value)
                            {
                                case ("stop"):
                                    isStartStop = true;
                                    break;

                                case ("transit"):
                                    isStartStop = false;
                                    break;

                                default:
                                    throw new RouteException("Unexpected value: " + match2.Groups[2].Value);
                            }

                            //stops tweede locatie
                            bool isEndStop;

                            switch (match2.Groups[4].Value)
                            {
                                case ("stop"):
                                    isEndStop = true;
                                    break;

                                case ("transit"):
                                    isEndStop = false;
                                    break;

                                default:
                                    throw new RouteException("Unexpected value: " + match2.Groups[4].Value);
                            }

                            distances.Add (distance);

                            if(!locations.Contains(locationB))
                            {
                                locations.Add(locationB);
                                stops.Add(isEndStop);
                            }

                        } else
                        {
                            File.AppendAllText(logFilePath, $"Warning: Line did not match any pattern - {line}");
                        }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(logFilePath, ex.Message + Environment.NewLine);
                    }
                }
                //add locations to the Route
                BuildRoute(locations, stops, distances);
                return route;
            }
        }
    
        public XRoute BuildRoute(List<string> locations, List<bool> stops, List<double> distances) 
        {
            string logFilePath = @"C:\Users\thiba\Documents\HOGENT\Semester2\ProgGevorderd1\Labo5\RouteRequirementsBL\Errors\ErrorLogBuildRoute.log"; ;
            //errorlog leegmaken bij het bouwen van een nieuwe route
            if (File.Exists(logFilePath)) File.WriteAllText(logFilePath, string.Empty);

            XRoute route = new XRoute();

            try
            {
                // exception als de lijsten die we binnen krijgen leeg zijn.
                if (locations == null || stops == null || distances == null)
                    throw new RouteException("InputLists are empty");

                // Voeg alle locaties toe
                for (int i = 0; i < distances.Count-1; i++)
                {
                    LocationSegment locationA = new LocationSegment(locations[i], stops[i]);
                    LocationSegment locationB = new LocationSegment(locations[i + 1], stops[i + 1]);
                    RouteSegment segment = new RouteSegment(distances[i+1], locationA, locationB);
                    route._segmentList.Add(segment);
                }

            }
            catch (RouteException ex)
            {
                File.AppendAllText(logFilePath, ex.Message + Environment.NewLine);
            }

            return route;
        }
        public XRoute ReverseRoute(XRoute route) 
        {

            // Check if the input route is null or has no segments
            if (route == null || route._segmentList == null)
            {
                throw new ArgumentNullException(nameof(route), "Route or route segments cannot be null.");
            }

            XRoute reversedRoute = new XRoute() ;

            foreach (RouteSegment segment in route._segmentList.AsEnumerable().Reverse())
            {
                reversedRoute._segmentList.Add(new RouteSegment(segment.Distance, new LocationSegment ( segment.StopB.Name, segment.StopB.IsStop), new LocationSegment(segment.StopA.Name, segment.StopA.IsStop)));
            }

            return reversedRoute;
        }
    }
}
