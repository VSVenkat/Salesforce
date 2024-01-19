using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;

class Program
{
    static void Main()
    {
        // Specify the path to your CSV file
        string csvFilePath = "C:\\Path\\To\\Your\\File.csv"; // Replace with the actual file path

        // Specify the row number you want to read
        int targetRowNumber = 2; // Replace with the desired row number

        // Specify the header you want to retrieve
        string targetHeader = "Market-Value"; // Replace with the desired header

        // Call the method to read a specific row from CSV
        string fieldValue = ReadCsvFieldValue(csvFilePath, targetRowNumber, targetHeader);

        // Display the value of the specified field
        Console.WriteLine($"{targetHeader} from Row {targetRowNumber}: {fieldValue}");
    }

    static string ReadCsvFieldValue(string filePath, int rowNumber, string header)
    {
        try
        {
            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Read rows until reaching the target row
                while (!parser.EndOfData && parser.LineNumber < rowNumber)
                {
                    parser.ReadLine();
                }

                // Check if the parser is positioned correctly
                if (parser.LineNumber == rowNumber)
                {
                    // Read the target row
                    string[] headers = parser.ReadFields();

                    // Find the index of the target header
                    int targetHeaderIndex = Array.IndexOf(headers, header);

                    if (targetHeaderIndex != -1)
                    {
                        // Read the entire row
                        string[] rowData = parser.ReadFields();

                        // Return the value of the specified field
                        return targetHeaderIndex < rowData.Length ? rowData[targetHeaderIndex] : null;
                    }
                    else
                    {
                        Console.WriteLine($"Error: Header '{header}' not found in CSV file.");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"Error: CSV file has fewer rows than {rowNumber}.");
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading CSV file: {ex.Message}");
            return null;
        }
    }
}
