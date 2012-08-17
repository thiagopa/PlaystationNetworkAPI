using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace PlaystationNetworkAPI.Core.Common
{

    /// <summary>
    /// Class that will handle the generic parsing features.
    /// </summary>
    class ParserBase
    {
        internal string _htmlCode;
        internal HtmlDocument _htmlDocument;

        /// <summary>
        /// Parser constructor.
        /// </summary>
        /// <param name="htmlCode">Html code that will be parsed.</param>
        internal ParserBase(string htmlCode)
        {
            _htmlCode = htmlCode;
            _htmlDocument = new HtmlDocument();
            _htmlDocument.LoadHtml(_htmlCode);
        }

        /// <summary>
        /// Get the inner value of a html element already typed.
        /// </summary>
        /// <typeparam name="T">Type of the object to be returned.</typeparam>
        /// <param name="elementXpath">XPath to the element in the page.</param>
        /// <param name="toRemove">String to be removed from the element text before it's converted to the desired type.</param>
        /// <returns>Typed value of the element or the default value for the type selected if the element is not found.</returns>
        internal T GetInnerValue<T>(string elementXpath, string toRemove = "", HtmlNode rootHtmlNode = null)
        {
            if (rootHtmlNode == null)
                rootHtmlNode = _htmlDocument.DocumentNode;

            var htmlNode = rootHtmlNode.SelectSingleNode(elementXpath);

            if (htmlNode != null)
            {
                var rawValue = htmlNode.InnerText;
                var parsedValue = ParseValue<T>(rawValue, toRemove: toRemove);
                return parsedValue;
            }
            else
                return default(T);
        }

        /// <summary>
        /// Get the value of a html element attribute already typed.
        /// </summary>
        /// <typeparam name="T">Type of the object to be returned.</typeparam>
        /// <param name="elementXpath">XPath to the element in the page.</param>
        /// <param name="attributeName">Name of the attribute in the element.</param>
        /// <param name="toRemove">String to be removed from the attribute text before it's converted to the desired type.</param>
        /// <returns>Typed value of the attribute or the default value for the type selected if the element is not found.</returns>
        internal T GetAttributeValue<T>(string elementXpath, string attributeName, string toRemove = "", HtmlNode rootHtmlNode = null)
        {
            if (rootHtmlNode == null)
                rootHtmlNode = _htmlDocument.DocumentNode;

            var htmlNode = rootHtmlNode.SelectSingleNode(elementXpath);
            var htmlAttribute = htmlNode.Attributes.FirstOrDefault(attr => attr.Name == attributeName);

            if (htmlAttribute == null)
                return default(T);

            var rawValue = htmlAttribute.Value;
            var parsedValue = ParseValue<T>(rawValue, toRemove: toRemove);
            return parsedValue;
        }

        /// <summary>
        /// Parse the value from string to the desited type.
        /// </summary>
        /// <typeparam name="T">Type of the object to be returned.</typeparam>
        /// <param name="rawValue">The value to be parsed.</param>
        /// <param name="toRemove">String to be removed from the attribute text before it's converted to the desired type.</param>
        /// <returns>Typed value of the attribute or the default value if something fails during the process.</returns>
        internal T ParseValue<T>(string rawValue, string toRemove = "")
        {
            if (!string.IsNullOrEmpty(toRemove))
                rawValue = rawValue.Replace(toRemove, string.Empty);

            rawValue = rawValue.Trim();
            rawValue = HttpUtility.HtmlDecode(rawValue);

            try
            {
                var parsedValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(rawValue);
                return parsedValue;
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                return default(T);
#endif
            }
        }
    }
}
