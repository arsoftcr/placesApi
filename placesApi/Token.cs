using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace placesApi
{
    public class Token
    {
        public string access_token { get; set; }

        public DateTime expires_in { get; set; }
    }
}
