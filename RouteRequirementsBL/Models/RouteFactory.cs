using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace RouteRequirementsBL.Models
{
    public class RouteFactory
    {
        public XRoute BuildRouteFromFile(string fileName) 
        {
            //instantie van Xroute maken
            XRoute route = new XRoute();
            //Errors bijhouden
            string logFilePath = @"C:\Users\thiba\Documents\HOGENT\Semester2\ProgGevorderd1\Labo5\RouteRequirementsBL\Errors\ErrorLog.log";
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
                        List<int> distancesMatch2 = new List<int>();

                        // Als er een match is met het patroon gaan we verder
                        if (match.Success)
                        {
                            // omdat we groepen gemaakt hebben in onze regex kunnen we deze gemakkelijk toekennen.
                            string locationString = match.Groups[1].Value;
                            double distanceInt = double.Parse(match.Groups[2].Value);
                            bool isStopBool = bool.Parse(match.Groups[3].Value);

                            // maak een nieuw object van locatie aan en voeg deze toe in de lijst.
                            locations.Add(locationString);
                            stops.Add(isStopBool);
                            distances.Add(distanceInt);

                        }

                        // Als er een match is met het tweede patroon gaan we verder
                        if (match2.Success)
                        {
                            if (teller == 0)
                            {
                                bool isStopBool = false;
                                bool isStopBool2 = false;
                                string locationString = match.Groups[1].Value;
                                string locationString2 = match.Groups[3].Value;

                                if (match2.Groups[2].Value == "stop")
                                    isStopBool = true;
                                
                                if (match2.Groups[4].Value == "stop")
                                    isStopBool2 = true;

                                int distanceInt = int.Parse(match2.Groups[5].Value);

                                locations.Add(locationString);
                                locations.Add(locationString2);
                                stops.Add(isStopBool);
                                stops.Add(isStopBool2);
                                distances.Add(0);
                                distances.Add(distanceInt);

                            } else {
                                string locationString = match.Groups[3].Value;
                                bool isStopBool = false;
                                if (match2.Groups[4].Value == "stop")
                                    isStopBool = true;
                                int distanceInt = int.Parse(match2.Groups[5].Value);

                                locations.Add(locationString);
                                stops.Add(isStopBool);
                                distances.Add(distanceInt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(logFilePath, ex.Message + Environment.NewLine);
                    }
                }

                //add locations to the Route
                BuildRoute(locations, stops,distances);
            }

            return route;
        }
    
        public XRoute BuildRoute(List<string> locations, List<bool> stops, List<double> distances) 
        {
            XRoute route = new XRoute();
            
            for (int i = 0; i < locations.Count - 2; i++)
            {
                route._segmentList.Add(new Segment(distances[i + 1], new SegmentLocatie(locations[i], stops[i]),new SegmentLocatie(locations[i+1], stops[i+1])));
            }

            return route;
        }
        public XRoute ReverseRoute(XRoute route) 
        {
            XRoute reversedRoute = route;

            foreach (Segment segment in reversedRoute._segmentList)
            {
                SegmentLocatie replacementLocation = segment.StopA;
                segment.StopA = segment.StopB;
                segment.StopB = replacementLocation;
            }

            reversedRoute._segmentList.Reverse();

            return reversedRoute;
        }
    }
}
