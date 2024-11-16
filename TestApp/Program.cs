using RouteRequirementsBL.Models;

namespace TestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RouteFactory factory = new RouteFactory();

            string data1 = @"C:\Users\thiba\Documents\HOGENT\Semester2\ProgGevorderd1\Labo5\data1";
            string data2 = @"C:\Users\thiba\Documents\HOGENT\Semester2\ProgGevorderd1\Labo5\data2";

            XRoute routeData1 = factory.BuildRouteFromFile(data1);
            XRoute routeData2 = factory.BuildRouteFromFile(data2);


            Console.WriteLine();

            for (int i = 0; i < routeData2._segmentList.Count; i++)
            {
                Console.WriteLine($"{routeData2._segmentList[i].StopA.Name}({routeData2._segmentList[i].StopA.IsStop}) -> " +
                     $"{routeData2._segmentList[i].StopB.Name}({routeData2._segmentList[i].StopB.IsStop}): " +
                     $"{routeData2._segmentList[i].Distance}");
            }

            Console.WriteLine();

            XRoute reversedRoute = factory.ReverseRoute(routeData1);

            for (int i = 0; i < reversedRoute._segmentList.Count; i++)
            {
                Console.WriteLine($"{reversedRoute._segmentList[i].StopA.Name}({reversedRoute._segmentList[i].StopA.IsStop}) -> " +
                     $"{reversedRoute._segmentList[i].StopB.Name}({reversedRoute._segmentList[i].StopB.IsStop}): " +
                     $"{reversedRoute._segmentList[i].Distance}");
            }

            Console.WriteLine();

            for (int i = 0; i < routeData2._segmentList.Count; i++)
            {
                Console.WriteLine($"{routeData2._segmentList[i].StopA.Name}({routeData2._segmentList[i].StopA.IsStop}) -> " +
                     $"{routeData2._segmentList[i].StopB.Name}({routeData2._segmentList[i].StopB.IsStop}): " +
                     $"{routeData2._segmentList[i].Distance}");
            }
        }
    }
}
