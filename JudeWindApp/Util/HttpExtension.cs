using System.Xml;

namespace JudeWindApp.Util
{
    /// <summary>Exten function of HTTP</summary>
    public static class HttpExtension
    {
        /// <summary>Loads the XML from the HTTP request body and returns it as a string and an XmlDocument.</summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<(string xmlString, XmlDocument requestXml)> LoadRequestXMLAsync(HttpRequest request)
        {
            var xmlString = await new StreamReader(request.Body).ReadToEndAsync();
            var requestXml = new XmlDocument();
            requestXml.LoadXml(xmlString);
            return (xmlString, requestXml);
        }
    }
}
