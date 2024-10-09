﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RouteRequirementsBL.Exceptions
{
    public class RouteException : Exception
    {
        public RouteException(string? message) : base(message)
        {
        }

        public RouteException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
