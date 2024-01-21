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

        System.IO.File.WriteAllLines(featureFile, lines);
    }
}
