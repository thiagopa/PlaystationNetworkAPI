using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;

namespace PlaystationNetworkAPI.Core.Common
{
    internal class CollectorBase
    {
        protected CookieContainer _cookieContainer;

        protected CollectorBase()
        {
            _cookieContainer = new CookieContainer();
        }

        /// <summary>Create a HttpWebRequest object and setup to on general web servers.</summary>
        /// <param name="url">URL that will be used on the request.</param>
        /// <returns>HttpWebRequest setup to work on general web servers</returns>
        protected virtual System.Net.HttpWebRequest CreateHttpWebRequest(string url, Dictionary<string, string> getData = null, Dictionary<string, string> postData = null)
        {
            if (getData != null)
            {
                var firstParameter = true;

                foreach (var keyValuePair in getData)
                {
                    url += firstParameter ? "?" : "&";
                    url += string.Format("{0}={1}", keyValuePair.Key, HttpUtility.UrlEncode(keyValuePair.Value));
                    firstParameter = false;
                }
            }

            System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);

            httpWebRequest.UserAgent = Properties.Settings.Default.Common_Collector_Request_UserAgent;
            httpWebRequest.Accept = Properties.Settings.Default.Common_Collector_Request_Accept;
            httpWebRequest.CookieContainer = _cookieContainer;

            if (postData != null)
            {
                var postDataArray = postData.Select(pd => string.Format("{0}={1}", pd.Key, HttpUtility.UrlEncode(pd.Value))).ToArray();
                var postDataString = string.Join("&", postDataArray.ToArray());

                var postDataStream = Encoding.UTF8.GetBytes(postDataString);

                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = postDataStream.Length;
                Stream httpWebRequestStream = httpWebRequest.GetRequestStream();
                httpWebRequestStream.Write(postDataStream, 0, postDataStream.Length);
                httpWebRequestStream.Close();
            }

            return httpWebRequest;
        }

        /// <summary>Get the source code of the requested pages.</summary>
        /// <param name="url">Full URL to get the source code from.</param>
        /// <returns>The returned page source code.</returns>
        protected string GetSourceCode(string url, Dictionary<string, string> getData = null, Dictionary<string, string> postData = null)
        {
            string sourceHtml;

            using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)CreateHttpWebRequest(url, getData: getData, postData: postData).GetResponse())
            {
                using (System.IO.StreamReader streamReader = new System.IO.StreamReader(httpWebResponse.GetResponseStream()))
                {
                    sourceHtml = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                httpWebResponse.Close();
            }

            return sourceHtml;
        }

    }
}
