using System;
using System.Xml;

class Program
{
    static void Main()
    {
        string xmlData = @"<SharedParameters>
                              <Parameter>
                                <Name>SharedParameter1</Name>
                                <Value>Value1</Value>
                              </Parameter>
                              <Parameter>
                                <Name>SharedParameter2</Name>
                                <Value>Value2</Value>
                              </Parameter>
                           </SharedParameters>";

        var sharedParameters = ParseXml(xmlData);

        // Access shared parameters as needed...
        Console.WriteLine($"SharedParameter1: {sharedParameters["SharedParameter1"]}");
        Console.WriteLine($"SharedParameter2: {sharedParameters["SharedParameter2"]}");
    }

    static Dictionary<string, string> ParseXml(string xmlData)
    {
        var sharedParameters = new Dictionary<string, string>();

        try
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlData);

            var parameterNodes = xmlDoc.SelectNodes("//Parameter");

            foreach (XmlNode parameterNode in parameterNodes)
            {
                var name = parameterNode.SelectSingleNode("Name")?.InnerText;
                var value = parameterNode.SelectSingleNode("Value")?.InnerText;

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    sharedParameters[name] = value;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing XML: {ex.Message}");
        }

        return sharedParameters;
    }
}
