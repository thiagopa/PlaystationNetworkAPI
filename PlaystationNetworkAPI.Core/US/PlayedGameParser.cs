using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace PlaystationNetworkAPI.Core.US
{
    class PlayedGameParser : Common.ParserBase
    {
        /// <summary>
        /// TrophiesParser constructor.
        /// </summary>
        /// <param name="htmlCode">HTML code to be handled</param>
        internal PlayedGameParser(string htmlCode) : base(htmlCode) { }


        /// <summary>
        /// Get the game id.
        /// </summary>
        /// <param name="htmlNode">Html node to look for the information.</param>
        /// <returns>Game id</returns>
        private string GetId(HtmlNode htmlNode)
        {
            var foundNode = htmlNode.Descendants("a").FirstOrDefault(n => n.ParentNode.Name == "div" && n.ParentNode.Attributes["class"].Value == "titlelogo");

            if (foundNode != null)
            {
                var value = foundNode.Attributes["href"].Value;
                value = ParseValue<string>(value);
                value = value.Split(Convert.ToChar("/")).Last();
                return value;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Get the game name.
        /// </summary>
        /// <param name="htmlNode">Html node to look for the information.</param>
        /// <returns>Game name</returns>
        private string GetName(HtmlNode htmlNode)
        {
            var foundNode = htmlNode.Descendants("span").FirstOrDefault(n => n.Attributes["class"].Value == "gameTitleSortField");

            if (foundNode != null)
            {
                var value = foundNode.InnerText;
                value = ParseValue<string>(value);
                return value;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Get the game image.
        /// </summary>
        /// <param name="htmlNode">Html node to look for the information.</param>
        /// <returns>Full URL for the game image</returns>
        private string GetImage(HtmlNode htmlNode)
        {
            var foundNode = htmlNode.Descendants("img").FirstOrDefault(n => n.Attributes["src"].Value.Contains("playstation.net/trophy"));

            if (foundNode != null)
            {
                var value = foundNode.Attributes["src"].Value;
                return value;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Get the progress in the game
        /// </summary>
        /// <param name="htmlNode">Html node to look for the information.</param>
        /// <returns>The progress in the game</returns>
        private int? GetProgress(HtmlNode htmlNode)
        {
            var foundNode = htmlNode.Descendants("span").FirstOrDefault(n => n.Attributes["class"].Value == "gameProgressSortField");

            if (foundNode != null)
            {
                var rawValue = foundNode.InnerText;
                var parsedValue = ParseValue<int?>(rawValue, toRemove: "%");
                return parsedValue;
            }
            else
                return null;
        }

        /// <summary>
        /// Get the trophy count.
        /// </summary>
        /// <param name="htmlNode">Html node to look for the information.</param>
        /// <returns>The trophy count</returns>
        private TrophyCount GetTrophyCount(HtmlNode htmlNode)
        {
            var nodeQuery = htmlNode.Descendants("div").Where(n => n.Attributes["class"].Value == "trophycontent" && n.ParentNode.ParentNode.Attributes["class"].Value == "trophycount normal");

            if (!nodeQuery.Any())
                return null;

            var trophyCount = new TrophyCount();
            var trophyIndex = 0;

            foreach (var foundNode in nodeQuery)
            {
                var rawValue = foundNode.InnerText;
                var parsedValue = ParseValue<int?>(rawValue);

                switch (trophyIndex)
                {
                    case 0:
                        trophyCount.Bronze = parsedValue;
                        break;

                    case 1:
                        trophyCount.Silver = parsedValue;
                        break;

                    case 2:
                        trophyCount.Gold = parsedValue;
                        break;

                    case 3:
                        trophyCount.Platinum = parsedValue;
                        break;

                    default:
                        break;
                }

                trophyIndex++;
            }

            trophyCount.Total = GetTotalTrophies(htmlNode);

            return trophyCount;
        }

        /// <summary>
        /// Get the total number of trophies.
        /// </summary>
        /// <param name="htmlNode">Html node to look for the information.</param>
        /// <returns>The total trophies number.</returns>
        private int? GetTotalTrophies(HtmlNode htmlNode)
        {
            var foundNode = htmlNode.Descendants("span").FirstOrDefault(n => n.Attributes["class"].Value == "gameTrophyCountSortField");

            if (foundNode != null)
            {
                var rawValue = foundNode.InnerText;
                var parsedValue = ParseValue<int?>(rawValue);
                return parsedValue;
            }
            else
                return null;
        }

        /// <summary>
        /// Get the played games for the Playstation Network Profile
        /// </summary>
        /// <returns>Played games</returns>
        internal List<Core.PlayedGame> GetPlayedGames()
        {
            List<Core.PlayedGame> parsedPlayedGames = new List<PlayedGame>();

            foreach (HtmlNode htmlNode in _htmlDocument.DocumentNode.SelectNodes("//div[@class='slot']"))
            {
                //var idElement = htmlNode.Descendants("a").FirstOrDefault(n => n.ParentNode.Name == "div" && n.ParentNode.Attributes["class"].Value == "titlelogo").Attributes["href"].Value.Split(Convert.ToChar("/")).Last();

                PlayedGame parsedPlayedGame = new PlayedGame()
                {
                    Id = GetId(htmlNode),
                    Title = GetName(htmlNode),
                    Image = GetImage(htmlNode),
                    Progress = GetProgress(htmlNode),
                    TrophyCount = GetTrophyCount(htmlNode)
                };

                parsedPlayedGames.Add(parsedPlayedGame);
            }

            return parsedPlayedGames;
        }
    }
}
