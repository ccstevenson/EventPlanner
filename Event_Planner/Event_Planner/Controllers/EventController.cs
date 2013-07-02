using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Event_Planner.Models;
using YelpSharp;

namespace Event_Planner.Controllers
{
    public class EventController : Controller
    {
        //
        // GET: /Event/

        private EventContext db = new EventContext();

        public ActionResult Index(int id)
        {
            EventModel e = db.Events.Find(id);
            Yelp yelp = new Yelp(YelpConfig.Options);
            foreach (RestaurantModel b in e.RestaurantModel)
            {

                b.YelpData = yelp.GetBusiness(b.YelpId).Result;
            }

            return View(e);
        }

        public ActionResult SaveEvent(BaseEvent newEvent)
        {
            EventModel e = new EventModel()
            {
                Name = newEvent.Name,
                RestaurantModel = new List<RestaurantModel>(),
            };
            foreach (Stub b in newEvent.Restaurants)
            {
                RestaurantModel r = new RestaurantModel();
                r.SetBusinessAndTime(b.id, b.start, b.end);
                e.RestaurantModel.Add(r);
            }
            db.Events.Add(e);
            db.SaveChanges();
            

            return Content("/Event/?id=" + e.EventModelID);
        }

    }

    
}
