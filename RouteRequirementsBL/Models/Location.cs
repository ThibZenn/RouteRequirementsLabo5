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
                    //TODO in streamreader zorgen dat de naam altijd met een hoofdletter begint.
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
