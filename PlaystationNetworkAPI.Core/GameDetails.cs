using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaystationNetworkAPI.Core
{
    public class GameDetails
    {
        public static GameDetails GetGameDetails(string psnId, string gameId, Common.Login login, string location)
        {

            switch (location)
            {
                case "US":
                    return GetUSGameDetails(psnId, gameId, login);

                default:
                    return GetUSGameDetails(psnId, gameId, login);
            }
        }

        private static GameDetails GetUSGameDetails(string psnId, string gameId, Common.Login login)
        {
            Core.US.Collector collector = new US.Collector(psnId);
            collector.GetGameDetails(gameId, login);

            return null;
        }
    }
}
