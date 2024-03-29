using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ActionAttribute : Attribute
{
    public string StepPattern { get; }

    public ActionAttribute(string stepPattern)
    {
        StepPattern = stepPattern;
    }
}

public class StepDefinitions
{
    [Action("Given I have entered [parameter] as the username")]
    public void GivenIHaveEnteredAsTheUsername(string username)
    {
        Console.WriteLine($"Executing step: Given I have entered '{username}' as the username");
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
        // Load XML file
        XDocument doc = XDocument.Load("Path\\To\\Your\\TestCases.xml");

        // Extract test case steps
        var steps = doc.Descendants("Step").Select(step => step.Value.Trim());

        // Instantiate the class containing step definitions
        var stepDefinitions = new StepDefinitions();

        // Execute steps
        foreach (var step in steps)
        {
            ExecuteStep(stepDefinitions, step);
        }
    }

    static void ExecuteStep(object stepDefinitions, string step)
    {
        // Use reflection to find and invoke the method based on custom attribute
        var methodInfo = FindMethodWithAttribute<StepDefinitions, ActionAttribute>(stepDefinitions, step);
        if (methodInfo != null)
        {
            // Extract parameters from the step (you may need more sophisticated parsing)
            var parameters = ExtractParametersFromExcel("path-to-your-excel-file.xlsx", methodInfo);

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
            var attribute = method.GetCustomAttribute<TAttribute>();
            if (IsStepMatch(step, attribute.StepPattern))
            {
                return method;
            }
        }

        return null;
    }
static object[] ExtractParametersFromExcel(string excelFilePath, MethodInfo methodInfo)
    {
        // Assuming the parameter is always a single string
        // You may need more sophisticated parsing based on your requirements

        // Read Excel file using EPPlus
        using (var package = new ExcelPackage(new System.IO.FileInfo(excelFilePath)))
        {
            var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first sheet
            var parameterValue = worksheet.Cells[1, 1].Text; // Assuming the parameter value is in the first cell
            return new object[] { parameterValue };
        }
    }
    static bool IsStepMatch(string step, string pattern)
    {
        // Implement your matching logic here
        // (you may need more sophisticated matching based on your requirements)
        return step == pattern;
    }
}
