using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaystationNetworkAPI.Core
{
    public class AchievedTrophy : Trophy
    {
        public DateTime DateEarned { get; set; }
        public string DisplayDate { get; set; }
    }
}
