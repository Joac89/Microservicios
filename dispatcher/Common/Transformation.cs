using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using dispatcher.Models;
using JUST;

namespace dispatcher.Common
{
    public class Transformation
    {
        private string pathEnvironment = "";
        public Transformation()
        {
            pathEnvironment = $"{System.Reflection.Assembly.GetExecutingAssembly().Location.Split("dispatcher")[0]}dispatcher";
        }
        public string Execute(TransformResult strInput, string template)
        {
            var pathTemplate = $"{pathEnvironment}/{template}";
            var json = "";

            json = TrsJSON(strInput.Result, pathTemplate);
            return json;
        }

        public XElement RemoveAllNamespacesXml(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespacesXml(el)));
        }

        public string CreateRequest(string pathRequest, int numRequest, string[] valuesReplace)
        {
            var soap = $"{pathEnvironment}/{pathRequest}";
            var soapsend = File.ReadAllText(soap);
            var i = 0;

            foreach (var item in valuesReplace)
            {
                soapsend = soapsend.Replace("{" + i + "}", item);
                i += 1;
            }

            return soapsend;
        }

        public string CreateRequest(string pathRequest, int numRequest, string valuesReplace)
        {
            var soap = $"{pathEnvironment}/{pathRequest}";
            var soapsend = File.ReadAllText(soap);

            soapsend = soapsend.Replace("{0}", valuesReplace);

            return soapsend;
        }

        private string TrsJSON(string json, string pathTemplate)
        {
            var input = json;
            var transformer = File.ReadAllText(pathTemplate);
            var transformedString = JsonTransformer.Transform(transformer, input);

            return transformedString;
        }
    }
}