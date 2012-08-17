using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaystationNetworkAPI.Core
{
    public class TrophyCount
    {
        public int? Platinum { get; set; }
        public int? Gold { get; set; }
        public int? Silver { get; set; }
        public int? Bronze { get; set; }
        public int? Total { get; set; }

        public TrophyCount() { }


    }
}