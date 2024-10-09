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

        private double _afstandTotVorigeStop;
        public double AfstandTotVorigeStop
        {
            get { return _afstandTotVorigeStop; }
            set 
            {
                if (value < 0)
                {
                    throw new LocatieException($"AfstandTotVorigeStop{value}");
                }
                _afstandTotVorigeStop = value; 
            }
        }

        public bool IsStop { get; set; }

        public Locatie(string naam, double afstandTotVorigeStop, bool isStop)
        {
            Naam = naam;
            AfstandTotVorigeStop = afstandTotVorigeStop;
            IsStop = isStop;
        }
    }
}
