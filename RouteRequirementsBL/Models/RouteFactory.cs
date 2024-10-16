using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

            List<Location> locationsList = new List<Location>();
            List<Distance> distancesList = new List<Distance>();

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
                        string pattern = @"^(.+?),(\d+),(true|false)$";
                        string pattern2 = @"^(.+?)\((stop|transit)\),(.+?)\((stop|transit)\),(\d+)$";
                        Match match = Regex.Match(line, pattern);
                        Match match2 = Regex.Match(line, pattern2);
                        List<int> distancesMatch2 = new List<int>();

                        // Als er een match is met het patroon gaan we verder
                        if (match.Success)
                        {
                            // omdat we groepen gemaakt hebben in onze regex kunnen we deze gemakkelijk toekennen.
                            string locationString = match.Groups[1].Value;
                            int distanceInt = int.Parse(match.Groups[2].Value);
                            bool isStopBool = bool.Parse(match.Groups[3].Value);

                            // maak een nieuw object van locatie aan en voeg deze toe in de lijst.
                            Location location = new Location(locationString, isStopBool);
                            locationsList.Add(location);

                            if (teller == 0)
                            {
                                // De eerste stop moet hetzelfde start als eindpunt hebben.
                                Distance distance = new Distance(distanceInt, locationString, locationString);
                                distancesList.Add(distance);
                            }
                            else
                            {
                                // Distance toekennen en puntA toewijzen met de data van de vorige locatie.
                                Distance distance = new Distance(distanceInt, distancesList[teller - 1].StopB, locationString);
                                distancesList.Add(distance);
                            }

                            teller++;
                        }

                        if (match2.Success)
                        {
                            string locationString = match.Groups[1].Value;
                            bool isStopBool = bool.Parse(match2.Groups[2].Value);
                            int distanceInt = int.Parse(match2.Groups[5].Value);

                            Location location = new Location(locationString, isStopBool);
                            locationsList.Add(location);

                            if(teller == 0)
                            {
                                distancesMatch2.Add(0);
                                distancesMatch2.Add(distanceInt);
                                Distance distance = new Distance(distancesMatch2[teller], locationString, locationString);
                                distancesList.Add(distance);
                            }
                            else
                            {
                                Distance distance = new Distance(distancesMatch2[teller], distancesList[teller - 1].StopB, locationString);
                                distancesList.Add(distance);
                            }
                            
                            teller++;
                        }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(logFilePath, ex.Message + Environment.NewLine);
                    }
                }

                //add locations to the Route
                foreach (var location in locationsList)
                {
                    route.Locations.Add(location);
                }
                foreach (var distance in distancesList)
                {
                    route.Distances.Add(distance);
                }
            }

            return route;
        }
    
        public XRoute BuildRoute(List<string> locations, List<bool> stops, List<double> distances) 
        {
            throw new NotImplementedException(); //TODO method implementeren

        }
        public XRoute ReverseRoute(XRoute route) 
        {
            throw new NotImplementedException(); //TODO method implementeren
        }
    }
}
