using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Event_Planner.Models
{
    public class ResultsViewModel
    {
        public SearchQuery Query { get; set; }
        public YelpSharp.Data.SearchResults SearchResults;
    }
}