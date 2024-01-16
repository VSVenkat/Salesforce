using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class YourTestClass
{
    private TestContext testContextInstance;

    public TestContext TestContext
    {
        get { return testContextInstance; }
        set { testContextInstance = value; }
    }

    [TestMethod]
    public void YourTestMethod()
    {
        // Access shared parameters
        string parameterValue = TestContext.Properties["YourSharedParameter"]?.ToString();

        // If YourSharedParameter is not set, handle it accordingly
        if (string.IsNullOrEmpty(parameterValue))
        {
            Console.WriteLine("Shared parameter 'YourSharedParameter' is not provided.");
            return; // You might throw an exception or handle it based on your needs
        }

        // Download attachment using the utility class
        AttachmentUtility.DownloadAttachment(Convert.ToInt32(TestContext.Properties["TestPlanId"]));

        // Perform your assertions and other test logic
        Assert.AreEqual("ExpectedValue", parameterValue); // Replace with your actual test logic
    }
}
