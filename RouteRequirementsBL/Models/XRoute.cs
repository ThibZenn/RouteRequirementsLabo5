﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteRequirementsBL.Interfaces;

namespace RouteRequirementsBL.Models
{
    public class XRoute : IRoute
    {
        private List<Locatie> LocatieList;

        public void AddLocation(string location, double distance, bool isStop)
        {
            throw new NotImplementedException(); //TODO method implementeren
        }

        public double GetDistance()
        {
            throw new NotImplementedException(); //TODO method implementeren
        }

        public double GetDistance(string startLocation, string endLocation)
        {
            throw new NotImplementedException(); //TODO method implementeren
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