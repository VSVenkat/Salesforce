using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

class Program
{
    static void Main()
    {
        string xmlData = @"<parameterSet>
                              <paramNames>
                                <param>Market-Value</param>
                                <param>Memberid</param>
                              </paramNames>
                              <paramData lastid=""1"">
                                <dataRow id=""1"">
                                  <kvp key=""Market-Value"" value=""CSMM""></kvp>
                                  <kvp key=""Memberid"" value=""1234""></kvp>
                                </dataRow>
                                <dataRow id=""2"">
                                  <kvp key=""Market-Value"" value=""CSMT""></kvp>
                                  <kvp key=""Memberid"" value=""1235""></kvp>
                                </dataRow>
                              </paramData>
                           </parameterSet>";

        var sharedParametersTable = ParseXml(xmlData);

        // Access shared parameters as needed...
        foreach (DataRow row in sharedParametersTable.Rows)
        {
            Console.WriteLine($"Row ID: {row["id"]}");
            foreach (DataColumn column in sharedParametersTable.Columns)
            {
                Console.WriteLine($"{column.ColumnName}: {row[column]}");
            }
            Console.WriteLine();
        }
    }

    static DataTable ParseXml(string xmlData)
    {
        var sharedParametersTable = new DataTable();

        try
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlData);

            // Extract param names
            var paramNodes = xmlDoc.SelectNodes("//paramNames/param");
            foreach (XmlNode paramNode in paramNodes)
            {
                sharedParametersTable.Columns.Add(paramNode.InnerText, typeof(string));
            }

            // Extract param data
            var dataRowNodes = xmlDoc.SelectNodes("//paramData/dataRow");

            foreach (XmlNode dataRowNode in dataRowNodes)
            {
                var rowData = sharedParametersTable.NewRow();
                rowData["id"] = int.Parse(dataRowNode.Attributes["id"]?.Value);

                var kvpNodes = dataRowNode.SelectNodes("kvp");

                foreach (XmlNode kvpNode in kvpNodes)
                {
                    var key = kvpNode.Attributes["key"]?.Value;
                    var value = kvpNode.Attributes["value"]?.Value;

                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                    {
                        if (sharedParametersTable.Columns.Contains(key))
                        {
                            rowData[key] = value;
                        }
                        else
                        {
                            Console.WriteLine($"Ignoring key: {key}, not found in paramNames.");
                        }
                    }
                }

                sharedParametersTable.Rows.Add(rowData);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing XML: {ex.Message}");
        }

        return sharedParametersTable;
    }
}
