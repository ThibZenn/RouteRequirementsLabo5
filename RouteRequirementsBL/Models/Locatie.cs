using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteRequirementsBL.Models
{
    public class Locatie
    {
		private string _naam;

		public string Naam
		{
			get { return _naam; }
            set
            {
                if (value != _naam)
                {

                }

                _naam = value;
            }
        }

	}
}
