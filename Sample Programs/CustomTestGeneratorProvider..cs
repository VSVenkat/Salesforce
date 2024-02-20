using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Generator.Interfaces;
using TechTalk.SpecFlow.Parser;

public class CustomTestGeneratorProvider : ITestGeneratorProvider
{
    private readonly CodeDomHelper _codeDomHelper;

    public CustomTestGeneratorProvider(CodeDomHelper codeDomHelper)
    {
        _codeDomHelper = codeDomHelper;
    }

    public IEnumerable<TestGeneratedTestInput> GenerateTests(Feature feature, SpecFlowDocument document)
    {
        // Implement custom test generation logic here
        // Combine all examples into a single test case

        var scenarios = feature.ScenarioDefinitions.Where(sd => sd is ScenarioOutline).ToList();
        foreach (var scenario in scenarios)
        {
            var exampleSet = ((ScenarioOutline)scenario).Examples.First();
            var mergedSteps = string.Join(Environment.NewLine, scenario.Steps.Select(step => _codeDomHelper.FormatCodeBlock(step.Text)));
            var mergedExamples = string.Join(Environment.NewLine, exampleSet.TableBody.Select(tb => _codeDomHelper.FormatCodeBlock(tb.ToString())));

            var testInput = new TestGeneratedTestInput(scenario, mergedSteps, mergedExamples);
            yield return testInput;
        }
    }
}
