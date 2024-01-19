using System;
using System.Data;
using System.IO;

class Program
{
    static void Main()
    {
        // Assuming you have a DataTable named "sharedParameters"
        DataTable sharedParameters = GetSharedParametersFromAdo(); // Replace with your method to get DataTable

        // Specify the file path for the CSV file
        string csvFilePath = "C:\\Path\\To\\Your\\File.csv"; // Replace with the desired file path

        // Call the method to save DataTable to CSV
        SaveDataTableToCsv(sharedParameters, csvFilePath);

        Console.WriteLine("DataTable saved to CSV successfully.");
    }

    static void SaveDataTableToCsv(DataTable dataTable, string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Write the column headers
                foreach (DataColumn column in dataTable.Columns)
                {
                    writer.Write($"{column.ColumnName},");
                }
                writer.WriteLine(); // Move to the next line after writing headers

                // Write the data rows
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        writer.Write($"{item},");
                    }
                    writer.WriteLine(); // Move to the next line after writing a row
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving DataTable to CSV: {ex.Message}");
        }
    }

    static DataTable GetSharedParametersFromAdo()
    {
        // Implement your logic to fetch shared parameters from Azure DevOps or another source
        // Example: return a dummy DataTable for illustration purposes
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("Market-Value", typeof(string));
        dummyTable.Columns.Add("Memberid", typeof(string));

        dummyTable.Rows.Add("CSMM", "1234");
        dummyTable.Rows.Add("CSMT", "1235");

        return dummyTable;
    }
}
