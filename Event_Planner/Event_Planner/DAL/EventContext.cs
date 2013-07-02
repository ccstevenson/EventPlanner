using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data.Entity;
using Event_Planner.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Event_Planner.Models
{
    public class EventContext : DbContext

    {
        public DbSet<RestaurantModel> Restaurants { get; set; }
        public DbSet<EventModel> Events { get; set; }

    }
}