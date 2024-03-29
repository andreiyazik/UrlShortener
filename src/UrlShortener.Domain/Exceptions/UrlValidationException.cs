﻿using System;
using System.Collections.Generic;

namespace UrlShortener.Domain.Exceptions
{
    public class UrlValidationException : Exception
    {
        public UrlValidationException( string message, List<string> errors )
            : base( message )
        {
            Errors = errors;
        }

        public List<string> Errors { get; set; }
    }
}