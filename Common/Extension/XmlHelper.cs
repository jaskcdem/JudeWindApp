using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Common.Extension
{
    public static class XmlHelper
    {
        public static string JsonToXML(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            using var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), XmlDictionaryReaderQuotas.Max);
            XElement xml = XElement.Load(reader);
            return xml.ToString();
        }

        public static string? GetXmlElementValue(IEnumerable<XElement> _xml, string _TagName)
            => _xml.Any() ? _xml.Descendants().Where(p => p.Name.LocalName.ToUpper() == _TagName.ToUpper())?.ElementAt(0).Value : string.Empty;

        /// <summary>Get the value of a node by name</summary>
        public static string GetNodeValue(this XmlNode node, string name)
        {
            if (node == null || string.IsNullOrEmpty(name))
                return string.Empty;
            var childNode = node.SelectSingleNode(name);
            return childNode?.InnerText ?? string.Empty;
        }
        /// <summary>Find the first node's value by name</summary>
        public static string FindNodeValue(this XmlNode bodyNode, string name)
        {
            if (bodyNode == null) return string.Empty;

            //Tree search: broad first.
            List<XmlNode> closed = [];
            Queue<XmlNode> open = [];
            foreach (XmlNode node in bodyNode.ChildNodes)
            {
                if (closed.Contains(node)) continue;
                closed.Add(node);
                if (node.ChildNodes.Count > 0) foreach (XmlNode child in node.ChildNodes) open.Enqueue(child);
            }
            while (open.TryDequeue(out XmlNode? node))
            {
                if (node == null || closed.Contains(node)) continue;
                closed.Add(node);
                if (node.Name == name) return node.InnerText;
                if (node.ChildNodes.Count > 0) foreach (XmlNode child in node.ChildNodes) open.Enqueue(child);
            }
            return string.Empty;
        }

        /// <summary>Validates the specified XML node against the provided XSD schema content.</summary>
        /// <param name="xmlNode">The XML node to be validated.</param>
        /// <param name="xsdContent">The XSD schema content as a string.</param>
        /// <returns>
        /// A tuple containing a boolean indicating validation success,
        /// and a list of validation error or warning messages (if any).
        /// </returns>
        /// <exception cref="XmlSchemaException">Thrown if the schema itself is invalid.</exception>
        /// <exception cref="XmlException">Thrown if the XML node is malformed.</exception>
        public static (bool, List<string>) XmlValidate(XmlNode xmlNode, string xsdContent)
        {
            var _errors = new List<string>();
            var reader = new StringReader(xsdContent);
            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings,
                IgnoreComments = true,
                IgnoreWhitespace = true
            };
            settings.Schemas.Add(null, XmlReader.Create(reader));
            settings.ValidationEventHandler += (sender, args) =>
            {
                _errors.Add(args.Message);
            };
            using var nodeReader = new XmlNodeReader(xmlNode);
            using var validatingReader = XmlReader.Create(nodeReader, settings);

            try
            {
                while (validatingReader.Read()) { }
                if (_errors.Count > 0)
                {
                    return (false, _errors);
                }
                return (true, _errors);
            }
            catch
            {
                return (false, _errors);
            }
        }

        /// <summary>
        /// Appends an error element to the given body content and wraps the result in a full envelope.
        /// If the body is null or empty, only the error element is returned inside the envelope.
        /// </summary>
        /// <param name="bodyXml">The inner body XML string to embed. Can be null or empty.</param>
        /// <param name="errorTag">Error element name</param>
        /// <param name="errorActions">Actions to apply to the error element, allowing customization of its content.</param>
        /// <param name="prefix">Optional prefix for envelope, default is "soap".</param>
        /// <returns>A full envelope string containing the original body and an appended error element.</returns>
        /// <exception cref="XmlException">Thrown if the input body XML is malformed.</exception>
        public static string AppendErrorToResponse(string? bodyXml, string errorTag, Action<XmlElement>[] errorActions, string? prefix = "soap")
        {
            var xmlDoc = new XmlDocument();
            var envelope = xmlDoc.CreateElement(prefix, "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            xmlDoc.AppendChild(envelope);

            envelope.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            var body = xmlDoc.CreateElement(prefix, "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            envelope.AppendChild(body);

            if (!string.IsNullOrWhiteSpace(bodyXml))
            {
                try
                {
                    var bodyFragment = xmlDoc.CreateDocumentFragment();
                    bodyFragment.InnerXml = bodyXml;
                    body.AppendChild(bodyFragment);
                }
                catch (XmlException) { throw; }
            }

            XmlElement errorElement = xmlDoc.CreateElement(errorTag);
            foreach (var action in errorActions) action(errorElement);
            body.AppendChild(errorElement);
            return xmlDoc.OuterXml;
        }
    }
}
