using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaystationNetworkAPI.Core
{
    public class PlayedGame : Game
    {
        public int? Progress { get; set; }
        public TrophyCount TrophyCount { get; set; }
        //public System.Collections.Generic.List<PlaystationNetworkAPI.Core.AchievedTrophy> AchievedTrophies { get; set; }
    }
}
