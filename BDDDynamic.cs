Feature: My Feature

  Scenario Outline: Perform Action with Different Usernames
    Given I have the username "<Username>"
    When I perform a specific action "<Action>"
    And I enter "<Password>"
    Then I should see the expected result

    Examples: dynamic

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TechTalk.SpecFlow;

[Binding]
public class MySteps
{
    private List<Dictionary<string, string>> exampleDataList;
    private Dictionary<string, string> currentExample;

    [BeforeScenario]
    public void BeforeScenario()
    {
        // Load examples data from an external source (e.g., CSV file)
        string csvFilePath = "path_to_your_data.csv";
        exampleDataList = LoadExamplesData(csvFilePath);

        // Replace the Examples placeholder in the feature file with actual examples
        ReplaceExamplesPlaceholder(exampleDataList);
    }

    [Given(@"I have the username ""(.*)""")]
    public void GivenIHaveTheUsername(string username)
    {
        // Find the current example based on the provided username
        currentExample = exampleDataList.Find(e => e["Username"] == username);

        if (currentExample == null)
        {
            throw new Exception($"Example data not found for username: {username}");
        }

        Console.WriteLine($"Setting up for Username: {username}");
    }

    [When(@"I perform a specific action ""(.*)""")]
    public void WhenIPerformASpecificAction(string action)
    {
        // Perform the specific action based on the current example
        Console.WriteLine($"Performing the specific action '{action}' for Username: {currentExample["Username"]}");
    }

    [When(@"I enter ""(.*)""")]
    public void WhenIEnter(string password)
    {
        // Set the current password for the current example
        currentExample["Password"] = password;
        Console.WriteLine($"Entering Password: {password}");
    }

    [Then(@"I should see the expected result")]
    public void ThenIShouldSeeTheExpectedResult()
    {
        // Perform assertions or validations for the expected result based on the current example
        Console.WriteLine($"Verifying the expected result for Username: {currentExample["Username"]}, Password: {currentExample["Password"]}");
    }

    private List<Dictionary<string, string>> LoadExamplesData(string filePath)
    {
        var examplesData = new List<Dictionary<string, string>>();
        var lines = File.ReadAllLines(filePath);

        // Assuming the first line contains the headers
        var headers = lines.First().Split(',');

        foreach (var line in lines.Skip(1)) // Skip header row
        {
            var values = line.Split(',');

            var example = new Dictionary<string, string>();
            for (int i = 0; i < headers.Length; i++)
            {
                example[headers[i]] = values[i];
            }

            examplesData.Add(example);
        }

        return examplesData;
    }

    private void ReplaceExamplesPlaceholder(List<Dictionary<string, string>> examples)
    {
        var featureFile = ScenarioContext.Current.ScenarioInfo.FeatureInfo.SourceFile;
        var lines = System.IO.File.ReadAllLines(featureFile).ToList();

        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Contains("Examples: dynamic"))
            {
                // Remove the placeholder line
                lines.RemoveAt(i);

                // Add the actual examples
                lines.AddRange(examples.Select(example => $"| {string.Join(" | ", example.Values)} |"));

                // Stop the loop
                break;
            }
        }

      ===============================

        using System;
using System.IO;
using System.Reflection;
using TechTalk.SpecFlow;

[Binding]
public class YourStepDefinitions
{
    [Given("I read the content of the feature file")]
    public void GivenIReadTheContentOfTheFeatureFile()
    {
        // Access the current FeatureContext
        var featureContext = FeatureContext.Current;

        // Use reflection to get the private 'featureContext' field from the ScenarioContext
        FieldInfo featureContextField = typeof(FeatureContext).GetField("featureContext", BindingFlags.NonPublic | BindingFlags.Instance);

        // Check if the field is found
        if (featureContextField != null)
        {
            // Get the value of the 'featureContext' field
            var context = featureContextField.GetValue(featureContext);

            // Use reflection to get the private 'featureFilePath' field from the FeatureContext
            FieldInfo featureFilePathField = context.GetType().GetField("featureFilePath", BindingFlags.NonPublic | BindingFlags.Instance);

            // Check if the field is found
            if (featureFilePathField != null)
            {
                // Get the value of the 'featureFilePath' field
                string featureFilePath = featureFilePathField.GetValue(context) as string;

                if (!string.IsNullOrEmpty(featureFilePath))
                {
                    // Read the content of the feature file
                    string featureFileContent = ReadFeatureFileContent(featureFilePath);

                    // Display the content (you can perform further processing with the content)
                    Console.WriteLine($"Content of the feature file:\n{featureFileContent}");
                }
                else
                {
                    Console.WriteLine("Error: Feature file path is empty or null.");
                }
            }
            else
            {
                Console.WriteLine("Error: Could not find the 'featureFilePath' field in FeatureContext.");
            }
        }
        else
        {
            Console.WriteLine("Error: Could not find the 'featureContext' field in FeatureContext.");
        }
    }

    private string ReadFeatureFileContent(string filePath)
    {
        // Implement your logic to read the content of the feature file
        // For example, you can use System.IO.File.ReadAllText
        try
        {
            return System.IO.File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading feature file: {ex.Message}");
            return null;
        }
    }
}


        System.IO.File.WriteAllLines(featureFile, lines);
    }
}
==================================================================

  private static string GetFeatureFilePath(string featureName)
{
    string startupPath = Environment.CurrentDirectory; 
    var splitStartupPath = startupPath.Split(new[] {"\\"}, StringSplitOptions.None);

    var featureFolder = splitStartupPath[0] + @"\\" + 
                        splitStartupPath[1] + @"\\" + 
                        splitStartupPath[2] + @"\\" +
                        splitStartupPath[3] + @"\\" +
                        splitStartupPath[4] + @"\\" + 
                        splitStartupPath[5] + @"\\Features\";

    var dir = new DirectoryInfo(featureFolder);

    foreach (var fi in dir.GetFiles())
    {
        if (fi.FullName.Contains(featureName))
            return fi.FullName;
    }

    return "No Feature File Found With Title: " + featureName;
}
