using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace PlaystationNetworkAPI.WebService
{

    /// <summary>
    /// Collect information from Playstation Netowrk Information.
    /// </summary>
    [WebService(Namespace = "http://psnapi.codeplex.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PlaystationNetworkAPI : System.Web.Services.WebService
    {
        /// <summary>
        /// Get the Playstation Network user profile information.
        /// </summary>
        /// <param name="psnId">Playstation Network Id that will used to get the information.</param>
        /// <param name="location">Location from the Playstation Network the information will be collected.</param>
        /// <returns>Playstation Network user profile information.</returns>
        [WebMethod(Description = "Collect Playstation Network basic profile information", BufferResponse = true, CacheDuration = (24 * 60 * 60))]
        public Core.Profile GetProfile(string psnId, string location)
        {
            return Core.Profile.GetProfile(psnId, location);
        }

        /// <summary>
        /// Get the Playstation Network detailed game information.
        /// </summary>
        /// <param name="psnId">Playstation Network Id that will used to get the information.</param>
        /// <param name="gameId">Game Id that will be used to get the information.</param>
        /// <param name="username">Username that will be used to login at Playstation Network</param>
        /// <param name="password">Password that will be used to login at Playstation Network</param>
        /// <param name="location">Location from the Playstation Network the information will be collected.</param>
        /// <returns>Playstation Network detailed game information.</returns>
        [WebMethod(Description = "Collect Playstation Network detailed game information", BufferResponse = true, CacheDuration = (24 * 60 * 60))]
        public Core.GameDetails GetGameDetails(string psnId, string gameId, string username, string password, string location)
        {
            var login = new Core.Common.Login(username, password);
            return Core.GameDetails.GetGameDetails(psnId, gameId, login, location);
        }
    }
}