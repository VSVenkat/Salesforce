using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExcelDataReader;
using System.IO;

[TestClass]
public class DatabaseTests
{
    private const string ConnectionString = "your_connection_string"; // Replace with your actual connection string
    private const string TableName = "your_table_name"; // Replace with your actual table name
    private const string ExcelFilePath = "path_to_excel_file.xlsx"; // Replace with the path to your Excel file

    [TestMethod]
    public void ReadDatabaseRowsAndExcelRows()
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            SqlCommand command = new SqlCommand($"SELECT * FROM {TableName}", connection);
            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Read the ID from the SQL result set
                    string idFromSql = reader["ID"].ToString();

                    // Read the corresponding row from the Excel file based on the ID
                    Dictionary<string, string> excelRow = ReadExcelRowById(idFromSql);

                    if (excelRow != null)
                    {
                        // Define mapping between Excel field names and SQL field names
                        Dictionary<string, string> fieldMapping = new Dictionary<string, string>
                        {
                            {"ExcelFieldName1", "SqlFieldName1"},
                            {"ExcelFieldName2", "SqlFieldName2"},
                            {"ExcelFieldName3", "SqlFieldName3"},
                            // Add more mappings as needed
                        };

                        // Process the SQL and Excel row data
                        ProcessRowData(idFromSql, reader, excelRow, fieldMapping);
                    }
                    else
                    {
                        Console.WriteLine($"No corresponding row found in Excel for ID: {idFromSql}");
                    }
                }
            }
        }
    }

    private Dictionary<string, string> ReadExcelRowById(string id)
    {
        using (var stream = File.Open(ExcelFilePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                var dataTable = result.Tables[0];

                // Assuming the ID is in the first column of the Excel file
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string idFromExcel = dataTable.Rows[i][0]?.ToString();

                    if (idFromExcel == id)
                    {
                        // Store Excel row values in a dictionary
                        Dictionary<string, string> rowData = new Dictionary<string, string>();
                        rowData["ID"] = dataTable.Rows[i][0]?.ToString() ?? string.Empty;
                        rowData["ExcelFieldName1"] = dataTable.Rows[i][1]?.ToString() ?? string.Empty;
                        rowData["ExcelFieldName2"] = dataTable.Rows[i][2]?.ToString() ?? string.Empty;
                        rowData["ExcelFieldName3"] = dataTable.Rows[i][3]?.ToString() ?? string.Empty;
                        // Add more fields as needed
                        return rowData;
                    }
                }
            }
        }
        return null;
    }

    private void ProcessRowData(string idFromSql, SqlDataReader sqlReader, Dictionary<string, string> excelRow, Dictionary<string, string> fieldMapping)
    {
        bool allFieldsMatch = true;

        foreach (var mapping in fieldMapping)
        {
            string excelField = mapping.Key;
            string sqlField = mapping.Value;

            // Access and process values from the SQL result set
            string valueFromSql = sqlReader[sqlField].ToString();

            // Access and process values from the Excel row
            string valueFromExcel = excelRow[excelField];

            // Compare the values
            if (valueFromSql != valueFromExcel)
            {
                Console.WriteLine($"Value mismatch for ID: {idFromSql}, Field: {excelField}");
                allFieldsMatch = false;
            }
        }

        if (allFieldsMatch)
        {
            Console.WriteLine($"Values matched for ID: {idFromSql}");
        }
    }
}
