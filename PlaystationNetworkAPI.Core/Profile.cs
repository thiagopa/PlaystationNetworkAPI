using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaystationNetworkAPI.Core
{
    public class Profile
    {
        public string Location { get; set; }
        public string PsnId { get; set; }
        public string AvatarSmall { get; set; }
        //public string AvatarMedium { get; set; }
        //public string AvatarFull { get; set; }
        //public string AboutMe { get; set; }
        public int? Level { get; set; }
        public int? Progress { get; set; }
        public TrophyCount TrophyCount { get; set; }
        public List<PlayedGame> PlayedGames { get; set; }

        public Profile() { }

        public static Profile GetProfile(string psnId, string location)
        {
            switch (location)
            {
                case "US":
                    return GetUSProfile(psnId);

                default:
                    return GetUSProfile(psnId);
            }
        }

        private static Profile GetUSProfile(string psnId)
        {
            Core.US.Collector collector = new US.Collector(psnId);
            collector.GetProfile();
            Core.US.ProfileParser profileParser = new US.ProfileParser(collector.PsnProfileHtml);
            Core.US.PlayedGameParser playedGameParser = new US.PlayedGameParser(collector.GamesHtml);

            Profile collectedProfile = profileParser.GetProfile();
            collectedProfile.PlayedGames = playedGameParser.GetPlayedGames();

            return collectedProfile;
        }
    }
}