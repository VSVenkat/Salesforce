using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Newtonsoft.Json;
using System;

[TestClass]
public class YourTestClass
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    [DataRow(1)] // This attribute indicates that the test method should be run with the provided data
    [DataRow(2)] // Add additional rows as needed
    public void YourTestMethod(int testCaseId)
    {
        // Access TestCaseId from the test parameters
        int retrievedTestCaseId = testCaseId;

        // Fetch Shared Parameters from ADO Test Plan
        var sharedParameters = FetchSharedParametersFromAdo(testCaseId);

        // Access Shared Parameters from TestContext.Properties
        string sharedParameter1 = TestContext.Properties["SharedParameter1"]?.ToString();
        string sharedParameter2 = TestContext.Properties["SharedParameter2"]?.ToString();

        // Use retrievedTestCaseId, sharedParameter1, sharedParameter2, and sharedParameters as needed in your test logic

        // Rest of your test logic...
    }

    private dynamic FetchSharedParametersFromAdo(int testCaseId)
    {
        // Replace these placeholders with your ADO organization, PAT, project, and other relevant details
        string adoUrl = "https://dev.azure.com/YourOrganization";
        string pat = "YourPersonalAccessToken";
        string project = "YourProject";
        string testPlanId = "YourTestPlanId";

        // Make a request to the Azure DevOps Services API to get test case details
        RestClient client = new RestClient($"{adoUrl}/{project}/_apis/test/plans/{testPlanId}/suites/{testCaseId}?api-version=7.1");
        RestRequest request = new RestRequest(Method.GET);
        request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{pat}"))}");

        IRestResponse response = client.Execute(request);

        if (response.IsSuccessful)
        {
            dynamic testCaseDetails = JsonConvert.DeserializeObject(response.Content);
            dynamic sharedParameters = testCaseDetails?.suiteTestCase?.pointAssignments[0]?.sharedParameters;

            return sharedParameters;
        }
        else
        {
            // Handle the case where the request fails
            throw new Exception($"Failed to fetch Shared Parameters from ADO Test Plan. Status: {response.StatusCode}, Response: {response.Content}");
        }
    }
}
