using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YelpSharp;
using Event_Planner.Models; // Allows access to Models (such as ZipCode zipCode)
using System.Xml.Linq;
using System.Net;

namespace Event_Planner.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Enter a zip code or city name to search for local businesses.";
            ViewBag.Page = "Home";

            return View();
        }

        public ActionResult About()
        {
            //ViewBag.Message = "Your app description page.";
            ViewBag.Page = "About";

            return View();
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";
            ViewBag.Page = "Contact";

            return View();
        }

        public ActionResult SearchYelp(string location, string term)
        {
            Yelp yelp = new Yelp(YelpConfig.Options);
            var results = yelp.Search(term, location).Result;
            return View(results);
        }

        // If an HttpPost request is made, pass the entered zip code to the Results View.
        [HttpPost]
        public ViewResult Index(SearchQuery query)
        {
            Yelp yelp = new Yelp(YelpConfig.Options);
            
            // if no zipcode was entered do a search by location.
            if (query.Zip == null)
            {
                return LocationSearch(query);
            }
            
            else
            {
                var results = yelp.Search(query.Term, query.Zip).Result;

                var model = new ResultsViewModel
                    {
                        Query = query,
                        SearchResults = results
                    };

                return View("Results", model);
            }
        }

        //class to hold the lat and long info
        public class LocationInfo
        {
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }

        //If no Zip is Entered, will search based on location
        public ViewResult LocationSearch(SearchQuery query)
        {
            LocationInfo v = new LocationInfo();
            string ipaddress = GetUsersIP();

            if (ipaddress != "127.0.0.1" && ipaddress != null && ipaddress != "::1" && ipaddress != "unknown" && false)
            {
                LocationInfo result = null;
                IPAddress i = System.Net.IPAddress.Parse(ipaddress);
                string ip = i.ToString();
                string r;
                
                using (var w = new WebClient())
                {
                    r = w.DownloadString(String.Format("http://api.hostip.info/?ip={0}&position=true", ip));
                }
               
                var xmlResponse = XDocument.Parse(r);
                var gml = (XNamespace)"http://www.opengis.net/gml";
                var ns = (XNamespace)"http://www.hostip.info/api";

                try
                {
                    result = (from x in xmlResponse.Descendants(ns + "Hostip")
                              select new LocationInfo
                              {
                                  Latitude = float.Parse(x.Descendants(gml + "coordinates").Single().Value.Split(',')[0]),
                                  Longitude = float.Parse(x.Descendants(gml + "coordinates").Single().Value.Split(',')[1]),
                              }).SingleOrDefault();
                }
                catch (NullReferenceException)
                {
                    //Looks like we didn't get what we expected.
                }


                double lon = double.Parse(result.Longitude.ToString());
                double lat = double.Parse(result.Latitude.ToString());

                Yelp yelp = new Yelp(YelpConfig.Options);

                var searchOptions = new YelpSharp.Data.Options.SearchOptions()
                {
                    GeneralOptions = new YelpSharp.Data.Options.GeneralOptions() { radius_filter = 3000, term = "food" },
                    LocationOptions = new YelpSharp.Data.Options.CoordinateOptions()
                    {
                        latitude = lat,
                        longitude = lon

                    }
                };
                var results = yelp.Search(searchOptions).Result;

                var model = new ResultsViewModel
                {
                    Query = query,
                    SearchResults = results
                };

                return View("Results", model);

            } //end of if statement

            //if ip lookup failed then search based on default location. Orem, UT.
            else
            {
                Yelp yelp = new Yelp(YelpConfig.Options);

                var searchOptions = new YelpSharp.Data.Options.SearchOptions()
                {
                    GeneralOptions = new YelpSharp.Data.Options.GeneralOptions() { radius_filter = 3000, term = "food" },
                    LocationOptions = new YelpSharp.Data.Options.CoordinateOptions()
                    {
                        latitude = 40.274057852586076,
                        longitude = -111.68078899383545
                    }
                };
                var results = yelp.Search(searchOptions).Result;

                var model = new ResultsViewModel
                {
                    Query = query,
                    SearchResults = results
                };

                return View("Results", model);
            }   
        }

        //Called by Location Search to get IP.
        public static string GetUsersIP()
        {
            System.Web.HttpContext current = System.Web.HttpContext.Current;
            string ipAddress = null;
            if (current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                ipAddress = current.Request.ServerVariables["HTTP_CLIENT_IP"];
            if (ipAddress == null || ipAddress.Length == 0 || ipAddress == "unknown")
                if (current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    ipAddress = current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipAddress == null || ipAddress.Length == 0 || ipAddress == "unknown")
                if (current.Request.ServerVariables["REMOTE_ADDR"] != null)
                    ipAddress = current.Request.ServerVariables["REMOTE_ADDR"];
            if(ipAddress == null || ipAddress.Length == 0)
                ipAddress = "unknown";
                    return ipAddress;

        }

    }
}
