using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Infrastructure;

[Binding]
public class CustomTestGenerationHooks
{
    private readonly IObjectContainer _objectContainer;

    public CustomTestGenerationHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeTestRun]
    public static void RegisterCustomTestGeneratorProvider()
    {
        var codeDomHelper = new TechTalk.SpecFlow.Generator.CodeDomHelper(TechTalk.SpecFlow.Generator.UnitTestProvider.MSTest);
        var customTestGeneratorProvider = new CustomTestGeneratorProvider(codeDomHelper);

        var generatorManager = (GeneratorManager)_objectContainer.Resolve<ITestGeneratorManager>();
        generatorManager.GeneratorProviders.Add(customTestGeneratorProvider);
    }
}
