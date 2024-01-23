using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ActionAttribute : Attribute
{
    public string StepPattern { get; }

    public ActionAttribute(string stepPattern)
    {
        StepPattern = stepPattern;
    }
}

[TestClass]
public class StepDefinitions
{
    [Action("Given I have entered @username as the @role")]
    public void GivenIHaveEnteredAsTheUsernameAndRole(string username, string role)
    {
        Console.WriteLine($"Executing step: Given I have entered '{username}' as the username and '{role}' as the role");
        // Add your step implementation logic here
    }

    // Other step definition methods
}

[TestClass]
public class TestExecutor
{
    [TestMethod]
    public void ExecuteTest()
    {
        var stepDefinitions = new StepDefinitions();

        // Example step
        string step = "Given I have entered 'John' as the username and 'Admin' as the role";

        // Use reflection to find and invoke the method based on custom attribute
        var methodInfo = FindMethodWithAttribute<StepDefinitions, ActionAttribute>(stepDefinitions, step);
        if (methodInfo != null)
        {
            // Extract parameters from the step using Excel values
            var parameters = ExtractParametersFromExcel("path-to-your-excel-file.xlsx", step, methodInfo);

            // Invoke the method
            methodInfo.Invoke(stepDefinitions, parameters);
        }
        else
        {
            Console.WriteLine($"No method found for step: {step}");
        }
    }

    static MethodInfo FindMethodWithAttribute<T, TAttribute>(T instance, string step) where TAttribute : Attribute
    {
        var methods = typeof(T).GetMethods().Where(method => method.GetCustomAttribute<TAttribute>() != null);

        foreach (var method in methods)
        {
            var attribute = method.GetCustomAttribute<TAttribute>() as ActionAttribute;
            if (attribute != null && IsStepMatch(step, attribute.StepPattern))
            {
                return method;
            }
        }

        return null;
    }

    static bool IsStepMatch(string step, string pattern)
    {
        return step.Contains(pattern.Replace("@", ""));
    }

    static object[] ExtractParametersFromExcel(string excelFilePath, string step, MethodInfo methodInfo)
    {
        var parameters = new List<object>();

        // Assuming the parameter names are specified in the step and the first row of the Excel sheet
        var parameterNames = GetParameterNames(step);

        // Read Excel file using EPPlus
        using (var package = new ExcelPackage(new System.IO.FileInfo(excelFilePath)))
        {
            var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first sheet

            // Assuming the parameter names are in the first row of the Excel sheet
            var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];

            // Iterate through rows and extract parameter values
            for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // Assuming data starts from the second row
            {
                var parameterValues = new List<string>();
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    parameterValues.Add(worksheet.Cells[row, col].Text);
                }

                var parameterDictionary = GetParameterDictionary(headerRow, parameterValues);
                var methodParameters = MapParametersToMethodParameters(methodInfo.GetParameters(), parameterDictionary);

                parameters.AddRange(methodParameters);
            }
        }

        return parameters.ToArray();
    }

    static string[] GetParameterNames(string step)
    {
        // Assuming parameter names are specified with @ in the step
        return step.Split(' ').Where(s => s.StartsWith("@")).Select(s => s.Trim('@')).ToArray();
    }

    static Dictionary<string, string> GetParameterDictionary(ExcelRangeBase headerRow, List<string> parameterValues)
    {
        var parameterDictionary = new Dictionary<string, string>();
        for (int col = 1; col <= headerRow.End.Column; col++)
        {
            var parameterName = headerRow[1, col].Text;
            var parameterValue = parameterValues[col - 1];
            parameterDictionary.Add(parameterName, parameterValue);
        }
        return parameterDictionary;
    }

    static object[] MapParametersToMethodParameters(ParameterInfo[] methodParameters, Dictionary<string, string> parameterDictionary)
    {
        var methodParameterValues = new List<object>();
        foreach (var parameterInfo in methodParameters)
        {
            if (parameterDictionary.TryGetValue(parameterInfo.Name, out var parameterValue))
            {
                // Convert the parameter value to the type expected by the method parameter
                var convertedValue = Convert.ChangeType(parameterValue, parameterInfo.ParameterType);
                methodParameterValues.Add(convertedValue);
            }
            else
            {
                // Handle missing parameter value
                methodParameterValues.Add(null);
            }
        }
        return methodParameterValues.ToArray();
    }
}
