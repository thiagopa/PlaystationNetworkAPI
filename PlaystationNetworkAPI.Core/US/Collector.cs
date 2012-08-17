using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using HtmlAgilityPack;

namespace PlaystationNetworkAPI.Core.US
{
    internal class Collector : Common.CollectorBase
    {
        #region Variables and properties

        string _psnId;
        string _sessionId;

        string _pingPageUrl = Properties.Settings.Default.US_Collector_PingPageUrl;
        //string _loginLandingUrl = "https://account.sonyentertainmentnetwork.com/external/auth/login.action?request_locale=en_US&service-entity=psn&returnURL=https://us.playstation.com/uwps/PSNTicketRetrievalGenericServlet";
        string _loginUrl = "https://account.sonyentertainmentnetwork.com/external/auth/login!authenticate.action";
        string _loginReturnUrl = "https://us.playstation.com/uwps/PSNTicketRetrievalGenericServlet";
        string _ticketUrl = "https://us.playstation.com/uwps/PSNTicketRetrievalGenericServlet?psnAuth=true&sessionId={0}";

        string _psnProfileUrl = Properties.Settings.Default.US_Collector_ProfileUrl;
        string _psnGameDetailsUrl = "http://us.playstation.com/playstation/psn/profile/{0}/get_ordered_title_details_data";

        string _psnProfileHtml;
        string _psnGamesUrl = Properties.Settings.Default.US_Collector_GamesUrl;
        string _psnGamesHtml;
        string _psnGameDetailsHtml;

        /// <summary>US Playstation Network Trophies page source code.</summary>
        internal string PsnProfileHtml
        {
            get { return _psnProfileHtml; }
        }

        /// <summary>US Playstation Network Games page source code.</summary>
        internal string GamesHtml
        {
            get { return _psnGamesHtml; }
        }

        #endregion

        /// <summary>US Playstation Network Collector constructor</summary>
        /// <param name="psnId">Playstation Network Id to get the information collected.</param>
        /// <exception cref="ArgumentException">The psnId parameter is empty, null or only white spaces.</exception>
        internal Collector(string psnId)
            : base()
        {
            if (string.IsNullOrEmpty(psnId) || string.IsNullOrWhiteSpace(psnId))
                throw new ArgumentException("This parameter cannot be empty, null or only white spaces.", "gameId");

            _psnId = psnId;
        }

        /// <summary>Create a HttpWebRequest object and setup to work with the US Playstation Netowrk web servers.</summary>
        /// <param name="url">URL that will be used on the request.</param>
        /// <returns>HttpWebRequest setup to work on the US Playstation Netowrk web servers.</returns>
        protected override System.Net.HttpWebRequest CreateHttpWebRequest(string url, Dictionary<string, string> getData = null, Dictionary<string, string> postData = null)
        {
            System.Net.HttpWebRequest httpWebRequest = base.CreateHttpWebRequest(url, getData: getData, postData: postData);

            httpWebRequest.Referer = Properties.Settings.Default.US_Collector_Request_Referer;

            return httpWebRequest;
        }

        /// <summary>Added this ping to a the Playstation Network public profile page to get the cookies used on the rest of the section.</summary>
        private void PingPageToGetCookies()
        {
            using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)CreateHttpWebRequest(_pingPageUrl).GetResponse())
            {
                httpWebResponse.Close();
            }
        }

        public void LoginPsn(Common.Login login)
        {
            //var getData = new Dictionary<string, string>();

            //getData.Add("request_locale", "en_US");
            //getData.Add("service-entity", "psn");
            //getData.Add("returnURL", _loginReturnUrl);

            //var loginSource = GetSourceCode(_loginUrl, getData: getData);

            //var htmlDocument = new HtmlDocument();
            //htmlDocument.LoadHtml(loginSource);

            //var strutsTokenName = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='struts.token.name']").Attributes["value"].Value;
            //var strutsToken = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='struts.token']").Attributes["value"].Value;

            var postData = new Dictionary<string, string>();

            //postData.Add("struts.token.name", strutsTokenName);
            //postData.Add("struts.token", strutsToken);
            postData.Add("j_username", login.Username);
            //postData.Add("rememberSignIn", "on");
            postData.Add("j_password", login.Password);
            //postData.Add("service-entity", "psn");

            using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)CreateHttpWebRequest(_loginUrl, postData: postData).GetResponse())
            {
                if (httpWebResponse.Cookies["JSESSIONID"] == null)
                    throw new InvalidCredentialsException();

                _sessionId = httpWebResponse.Cookies["JSESSIONID"].Value;

                httpWebResponse.Close();
            }

            var ticketUrl = string.Format(_ticketUrl, _sessionId);

            using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)CreateHttpWebRequest(ticketUrl).GetResponse())
            {
                httpWebResponse.Close();
            }
        }

        #region PSN Profile

        /// <summary>Get the source code of the profile pages.</summary>
        public void GetProfile()
        {
            SetProfileUrls();
            PingPageToGetCookies();
            GetTrophiesSource();
            GetGamesSource();
        }

        /// <summary>Configure all the URLs to use the current Playstation Network ID</summary>
        private void SetProfileUrls()
        {
            _pingPageUrl = string.Format(_pingPageUrl, _psnId);
            _psnProfileUrl = string.Format(_psnProfileUrl, _psnId);
            _psnGamesUrl = string.Format(_psnGamesUrl, _psnId);
        }

        /// <summary>Get the torphies page source code.</summary>
        private void GetTrophiesSource()
        {
            _psnProfileHtml = GetSourceCode(_psnProfileUrl);
        }

        /// <summary>Get the games page source code.</summary>
        private void GetGamesSource()
        {
            _psnGamesHtml = GetSourceCode(_psnGamesUrl);
        }

        #endregion

        #region PSN GameDetails

        /// <summary>Get the source code of the game details page</summary>
        /// <exception cref="ArgumentException">The gameId parameter is empty, null or only white spaces.</exception>
        /// <exception cref="ArgumentNullException">The the login parameter is null</exception>
        public void GetGameDetails(string gameId, Common.Login login)
        {
            if (string.IsNullOrEmpty(gameId) || string.IsNullOrWhiteSpace(gameId))
                throw new ArgumentException("This parameter cannot be empty, null or only white spaces.", "gameId");

            if (login == null)
                throw new ArgumentNullException("login");

            //PingPageToGetCookies();
            LoginPsn(login);
            GetGameDetailsSource(gameId, login);
        }

        private void GetGameDetailsSource(string gameId, Common.Login login)
        {
            var getData = new Dictionary<string, string>();

            getData.Add("sortBy", "id_asc");
            getData.Add("titleId", gameId);

            _psnGameDetailsHtml = GetSourceCode(_psnGameDetailsUrl, getData: getData);
        }
        #endregion

        #region PSN Login
        #endregion
    }
}
