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

            json = TransformJson(strInput.Result, pathTemplate);
            return json;
        }

        public string RemoveAllNamespacesXml(XElement xmlDocument)
        {
            try
            {
                if (!xmlDocument.HasElements)
                {
                    XElement xElement = new XElement(xmlDocument.Name.LocalName);
                    xElement.Value = xmlDocument.Value;

                    foreach (XAttribute attribute in xmlDocument.Attributes())
                        xElement.Add(attribute);

                    return xElement.ToString();
                }
                return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespacesXml(el))).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in T Remove Namespaces XML: " + ex.Message);
                return "";
            }
        }

        public string CreateRequest(string pathRequest, int numRequest, string[] valuesReplace)
        {
            var soapsend = "";
            var i = 0;

            try
            {
                var soap = $"{pathEnvironment}/{pathRequest}";
                soapsend = File.ReadAllText(soap);

                foreach (var item in valuesReplace)
                {
                    soapsend = soapsend.Replace("{" + i + "}", item);
                    i += 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in T Create Request JSON: " + ex.Message);
            }

            return soapsend;
        }

        public string CreateRequest(string pathRequest, int numRequest, string valuesReplace)
        {
            var soapsend = "";

            try
            {
                var soap = $"{pathEnvironment}/{pathRequest}";

                soapsend = File.ReadAllText(soap);
                soapsend = soapsend.Replace("{0}", valuesReplace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in T Create Request JSON: " + ex.Message);
            }

            return soapsend;
        }

        private string TransformJson(string json, string pathTemplate)
        {
            var transformedString = "";

            try
            {
                var input = json;
                var transformer = File.ReadAllText(pathTemplate);
                transformedString = JsonTransformer.Transform(transformer, input);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Transformation JSON: " + ex.Message);
            }

            return transformedString;
        }
    }
}