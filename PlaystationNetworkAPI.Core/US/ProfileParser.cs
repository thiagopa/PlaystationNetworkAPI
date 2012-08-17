using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace PlaystationNetworkAPI.Core.US
{
    internal class ProfileParser : Common.ParserBase
    {
        /// <summary>
        /// TrophiesParser constructor.
        /// </summary>
        /// <param name="htmlCode">HTML code to be handled</param>
        internal ProfileParser(string htmlCode) : base(htmlCode) { }

        /// <summary>
        /// Get PSN ID Level
        /// </summary>
        /// <returns>Level number</returns>
        private int? GetLevel()
        {
            var elementXpath = "//div[@id='leveltext']";
            return GetInnerValue<int?>(elementXpath);
        }

        /// <summary>
        /// Get the PSN ID progress to the next level.
        /// </summary>
        /// <returns>Progress percentage number.</returns>
        private int? GetProgress()
        {
            var elementXpath = "//div[@class='progresstext']";
            return GetInnerValue<int?>(elementXpath, toRemove: "%");
        }

        /// <summary>
        /// Get the total number of bronze trophies.
        /// </summary>
        /// <returns>The bronze trophies number.</returns>
        private int? GetBronzeTrohies()
        {
            var elementXpath = "//div[@class='text bronze']";
            return GetInnerValue<int?>(elementXpath, toRemove: "Bronze");
        }

        /// <summary>
        /// Get the total number of silver trophies.
        /// </summary>
        /// <returns>The silver trophies number.</returns>
        private int? GetSilverTrohies()
        {
            var elementXpath = "//div[@class='text silver']";
            return GetInnerValue<int?>(elementXpath, toRemove: "Silver");
        }

        /// <summary>
        /// Get the total number of gold trophies.
        /// </summary>
        /// <returns>The gold trophies number.</returns>
        private int? GetGoldTrophies()
        {
            var elementXpath = "//div[@class='text gold']";
            return GetInnerValue<int?>(elementXpath, toRemove: "Gold");
        }

        /// <summary>
        /// Get the total number of platinum trophies.
        /// </summary>
        /// <returns>The platinum trophies number.</returns>
        private int? GetPlatinumTrophies()
        {
            var elementXpath = "//div[@class='text platinum']";
            return GetInnerValue<int?>(elementXpath, toRemove: "Platinum");
        }

        /// <summary>
        /// Get the total number of trophies.
        /// </summary>
        /// <returns>The total trophies number.</returns>
        private int? GetTotalTrophies()
        {
            var elementXpath = "//div[@id='totaltrophies']/div[@id='text']";
            return GetInnerValue<int?>(elementXpath);
        }

        /// <summary>
        /// Get the PSN ID from the source code.
        /// </summary>
        /// <returns>PSN ID from the source code.</returns>
        private string GetPsnId()
        {
            var elementXpath = "//div[@id='id-handle']";
            return GetInnerValue<string>(elementXpath);
        }

        /// <summary>
        /// Get the Small Avatar path.
        /// </summary>
        /// <returns>Small Avatar path.</returns>
        private string GetAvatarSmall()
        {
            var elementXpath = "//div[@id='id-avatar']/img";
            var attributeName = "src";
            var toRemove = "/playstation/PSNImageServlet?avtar=";
            return GetAttributeValue<string>(elementXpath, attributeName, toRemove: toRemove);
        }

        /// <summary>
        /// Get the Playstation Network Profile from the source page.
        /// </summary>
        /// <returns>Playstation Network Profile</returns>
        internal Profile GetProfile()
        {
            TrophyCount collectedTrophyCount = new TrophyCount()
            {
                Bronze = GetBronzeTrohies(),
                Silver = GetSilverTrohies(),
                Gold = GetGoldTrophies(),
                Platinum = GetPlatinumTrophies(),
                Total = GetTotalTrophies()
            };

            Profile collectedProfile = new Profile()
            {
                Location = "US",
                PsnId = GetPsnId(),
                AvatarSmall = GetAvatarSmall(),
                Level = GetLevel(),
                Progress = GetProgress(),
                TrophyCount = collectedTrophyCount,
            };

            return collectedProfile;
        }
    }
}
