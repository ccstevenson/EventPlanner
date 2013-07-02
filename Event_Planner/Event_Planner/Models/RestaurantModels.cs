using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
//using System.Data.Linq;
//using System.Data.Linq.Mapping;
//using Newtonsoft.Json.Linq;
using YelpSharp;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Planner.Models
{
    
    
   
    public class RestaurantModel
    {
        //[Column(IsPrimaryKey = true)]
        //public string RestaurantModelID { get; set; }
        
        public int RestaurantModelId { get; set; }
   
        public string YelpId { get; set; }

        public string Name { get; set; }
  
        public string Phone { get; set; }

        public string[] Address { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public virtual EventModel EventModel { get; set; }
        
        [NotMapped]
        public YelpSharp.Data.Business YelpData {get; set;}

        public void SetBusinessFromYelp(string _id)
        {

            Yelp yelp = new Yelp(YelpConfig.Options);
            var result = yelp.GetBusiness(_id).Result;
            YelpId = result.id;
            Name = result.name;
            Phone = result.phone;
            Address = result.location.display_address;
        }

        public void SetBusinessAndTime(string _id, DateTime _start, DateTime _end)
        {
            SetBusinessFromYelp(_id);
            StartTime = _start;
            EndTime = _end;
        }
            
    }

    public class EventModel
    {

        public int EventModelID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<RestaurantModel> RestaurantModel { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }

    public class Stub
    {
        public string id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }

    public class BaseEvent
    {
        public string Name { get; set; }
        public Stub[] Restaurants { get; set; }
    }
}