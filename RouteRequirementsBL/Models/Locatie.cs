using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteRequirementsBL.Exceptions;

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
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new LocatieException($"LocatieNaam is leeg");
                    //TODO in streamreader zorgen dat de naam altijd met een hoofdletter begint.
                } 
                _naam = value;
            }
        }

        public Afstand Afstand {get; set;}

        public bool IsStop { get; set; }

        public Locatie(string naam, Afstand afstand, bool isStop)
        {
            Naam = naam;
            Afstand = afstand;
            IsStop = isStop;
        }
    }
}
