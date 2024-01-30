using System;
using System.IO;
using System.Reflection;
using OfficeOpenXml;

public class MethodInvoker
{
    public void Method1(string param1)
    {
        Console.WriteLine($"Method1 called with parameter: {param1}");
    }

    public void Method2(int param1, string param2)
    {
        Console.WriteLine($"Method2 called with parameters: {param1}, {param2}");
    }

    public void Method3(double param1)
    {
        Console.WriteLine($"Method3 called with parameter: {param1}");
    }

    // Add more methods as needed...
}

class Program
{
    static void Main(string[] args)
    {
        string keyword = "Method2"; // The method to call
        string excelFilePath = "parameters.xlsx"; // Path to the Excel file

        // Load Excel file using EPPlus
        using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
        {
            var worksheet = package.Workbook.Worksheets[0];

            // Get the number of rows and columns in the worksheet
            int rows = worksheet.Dimension.Rows;
            int columns = worksheet.Dimension.Columns;

            // Create an instance of the MethodInvoker class
            MethodInvoker invoker = new MethodInvoker();

            // Get the method info based on the keyword
            MethodInfo methodInfo = typeof(MethodInvoker).GetMethod(keyword);

            if (methodInfo != null)
            {
                // Loop through each row in the Excel file
                for (int row = 2; row <= rows; row++) // Assuming the first row contains headers
                {
                    // Get the parameters from the current row
                    object[] parameters = new object[columns];
                    for (int col = 1; col <= columns; col++)
                    {
                        parameters[col - 1] = worksheet.Cells[row, col].Value;
                    }

                    // Invoke the method with the given parameters
                    methodInfo.Invoke(invoker, parameters);
                }
            }
            else
            {
                Console.WriteLine($"Method '{keyword}' not found.");
            }
        }
    }
}
