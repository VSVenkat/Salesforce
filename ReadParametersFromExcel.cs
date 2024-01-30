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

=======================================================

    using System;
using System.Data;
using System.Reflection;

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
        DataTable parametersTable = GetParametersFromDataTable(); // Get parameters from DataTable

        // Create an instance of the MethodInvoker class
        MethodInvoker invoker = new MethodInvoker();

        // Get the method info based on the keyword
        MethodInfo methodInfo = typeof(MethodInvoker).GetMethod(keyword);

        if (methodInfo != null)
        {
            // Get the parameters count of the method
            ParameterInfo[] methodParams = methodInfo.GetParameters();

            // Check if the number of parameters match
            if (methodParams.Length == parametersTable.Columns.Count)
            {
                // Construct an array of parameters dynamically
                object[] methodParameters = new object[methodParams.Length];
                for (int i = 0; i < methodParams.Length; i++)
                {
                    // Convert the parameter to the correct type
                    methodParameters[i] = Convert.ChangeType(parametersTable.Rows[0][i], methodParams[i].ParameterType);
                }

                // Invoke the method with the given parameters
                methodInfo.Invoke(invoker, methodParameters);
            }
            else
            {
                Console.WriteLine($"Number of parameters does not match for method '{keyword}'.");
            }
        }
        else
        {
            Console.WriteLine($"Method '{keyword}' not found.");
        }
    }

    // Dummy method to simulate reading parameters from DataTable
    static DataTable GetParametersFromDataTable()
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("Param1", typeof(int));
        dataTable.Columns.Add("Param2", typeof(string));

        // Add sample row with parameters
        dataTable.Rows.Add(42, "hello");

        return dataTable;
    }
}

