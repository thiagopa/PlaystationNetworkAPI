using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaystationNetworkAPI.Core
{
    public class Trophy
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public TrophyType? TrophyType { get; set; }
        public string FullDescription { get; set; }
        public bool Hidden { get; set; }
    }
}
