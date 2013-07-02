using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Event_Planner.Models
{
    public class SearchQuery
    {
        public string Term = ""; // "food" searches for restaurants only. Empty string pulls all results.
        public string Zip { get; set; } // Location
    }
}