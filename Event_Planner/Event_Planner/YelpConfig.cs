using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YelpSharp;


namespace Event_Planner
{
    class YelpConfig
    {
        private static Options _options;

        public static Options Options
        {
            get
            {
                if (_options == null)
                {
                    _options = new Options()
                    {
                        AccessToken = "GDTVW6OH_FdVfkfJENzj5dvPr7GB8Hva",
                        AccessTokenSecret = "EDA1_03JULnfFTpTFkDh4TOSPXg",
                        ConsumerKey = "gMokL2VQfCYoor9JV5G2xw",
                        ConsumerSecret = "62P5bP-CC0wmaOoJrSdJzzptnLs"
                    };
                }
                return _options;
            }
        }
    }
}


