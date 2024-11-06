using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteRequirementsBL.Exceptions;

namespace RouteRequirementsBL.Models
{
    public abstract class Location
    {
		private string _name;

		public string Name
		{
			get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new LocationException($"LocatieNaam is empty");
                    //TODO: hoofdletter afdwingen? 
                }
                else if (!char.IsUpper(value[0]))
                {
                    throw new RouteException($"{value} begint niet met een hoofdletter");
                }
                _name = value;
            }
        }

        public Location(string name)
        {
            Name = name;
        }
    }
}
